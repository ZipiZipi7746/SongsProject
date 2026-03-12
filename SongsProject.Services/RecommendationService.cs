using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SongsProject.Models;
using SongsProject.Repositories.Interfaces;

namespace SongsProject.Services
{
    public class RecommendationEngineService
    {
        private readonly ISongRepository _songRepo;
        private readonly IListeningHistoryRepository _historyRepo;
        private readonly IRecommendationRepository _recommendationRepo;
        private readonly string _apiKey;

        public RecommendationEngineService(
            ISongRepository songRepo,
            IListeningHistoryRepository historyRepo,
            IRecommendationRepository recommendationRepo,
            IConfiguration config)
        {
            _songRepo = songRepo;
            _historyRepo = historyRepo;
            _recommendationRepo = recommendationRepo;
            _apiKey = config["OpenAI:ApiKey"] ?? "";
        }

        public async Task<List<Song>> GenerateRecommendations(int userId)
        {
            var history = _historyRepo.GetAll()
                .Where(h => h.UserId == userId)
                .ToList();

            var allSongs = _songRepo.GetAll().ToList();

            if (!history.Any())
                return new List<Song>();

            var historySummary = history
                .GroupBy(h => h.SongId)
                .Select(g => {
                    var song = allSongs.FirstOrDefault(s => s.SongId == g.Key);
                    return new
                    {
                        songId = g.Key,
                        title = song?.Title ?? "",
                        artist = song?.ArtistName ?? "",
                        genre = song?.Genre ?? "",
                        summary = song?.LyricsSummary ?? "",
                        listenCount = g.Count(),
                        totalSeconds = g.Sum(h => h.Duration)
                    };
                })
                .OrderByDescending(x => x.totalSeconds)
                .ToList();

            var heardIds = historySummary.Select(x => x.songId).ToHashSet();

            var unheardSongs = allSongs.Where(s => !heardIds.Contains(s.SongId)).Select(s => new {
                id = s.SongId,
                title = s.Title,
                artist = s.ArtistName,
                genre = s.Genre,
                summary = s.LyricsSummary
            }).ToList();

            if (!unheardSongs.Any())
                return allSongs
                    .Where(s => heardIds.Contains(s.SongId))
                    .OrderByDescending(s => historySummary.FirstOrDefault(h => h.songId == s.SongId)?.totalSeconds ?? 0)
                    .Take(5)
                    .ToList();

            var prompt = $@"
אתה מנוע המלצות מוזיקה חכם.

היסטוריית האזנה של המשתמש (ממוין לפי זמן האזנה):
{JsonConvert.SerializeObject(historySummary, Formatting.Indented)}

שירים זמינים שהמשתמש עוד לא שמע:
{JsonConvert.SerializeObject(unheardSongs, Formatting.Indented)}

בהתבסס על הז'אנרים, הזמרים, ותוכן השירים שהמשתמש אהב הכי הרבה (לפי זמן האזנה),
המלץ על עד 5 שירים מהרשימה שיתאימו לטעם האישי שלו.

החזר JSON בלבד ללא טקסט נוסף:
{{""recommendedIds"": [1, 2, 3]}}";

            try
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _apiKey);

                var body = new
                {
                    model = "gpt-4o-mini",
                    messages = new[] { new { role = "user", content = prompt } },
                    max_tokens = 200
                };

                var response = await client.PostAsync(
                    "https://api.openai.com/v1/chat/completions",
                    new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json")
                );

                var json = await response.Content.ReadAsStringAsync();
                Console.WriteLine("OpenAI response: " + json);
                dynamic result = JsonConvert.DeserializeObject(json)!;
                string content = result.choices[0].message.content;
                var clean = content.Replace("```json", "").Replace("```", "").Trim();
                var parsed = JsonConvert.DeserializeObject<dynamic>(clean)!;
                var ids = ((Newtonsoft.Json.Linq.JArray)parsed.recommendedIds)
                    .Select(x => (int)x).ToList();

                foreach (var id in ids)
                {
                    var existing = _recommendationRepo.GetAll()
                        .Any(r => r.UserId == userId && r.SongId == id);
                    if (!existing)
                    {
                        await _recommendationRepo.AddAsync(new Recommendation
                        {
                            UserId = userId,
                            SongId = id,
                            RecommendedAt = DateTime.Now
                        });
                    }
                }

                return allSongs.Where(s => ids.Contains(s.SongId)).ToList();
            }
            catch
            {
                return allSongs
                    .Where(s => heardIds.Contains(s.SongId))
                    .OrderByDescending(s => historySummary.FirstOrDefault(h => h.songId == s.SongId)?.totalSeconds ?? 0)
                    .Take(5)
                    .ToList();
            }
        }

        public async Task<List<Song>> GetTimeBasedSongs(int userId)
        {
            var allSongs = _songRepo.GetAll().ToList();
            var now = DateTime.Now;
            var dayOfWeek = now.DayOfWeek;
            var month = now.Month;
            var hour = now.Hour;

            var historyRaw = _historyRepo.GetAll()
                .Where(h => h.UserId == userId)
                .ToList();

            var history = historyRaw
                .GroupBy(h => h.SongId)
                .Select(g => {
                    var song = allSongs.FirstOrDefault(s => s.SongId == g.Key);
                    return new
                    {
                        title = song?.Title ?? "",
                        genre = song?.Genre ?? "",
                        listenCount = g.Count(),
                        totalSeconds = g.Sum(h => h.Duration)
                    };
                })
                .OrderByDescending(x => x.totalSeconds)
                .Take(10)
                .ToList();

            var catalog = allSongs.Select(s => new {
                id = s.SongId,
                title = s.Title,
                artist = s.ArtistName,
                genre = s.Genre,
                summary = s.LyricsSummary
            }).ToList();

            var prompt = $@"
אתה מנוע המלצות מוזיקה.

פרטי הזמן הנוכחי:
- יום בשבוע: {dayOfWeek}
- חודש: {month}
- שעה: {hour}:00

היסטוריית האזנה של המשתמש:
{JsonConvert.SerializeObject(history, Formatting.Indented)}

קטלוג שירים:
{JsonConvert.SerializeObject(catalog, Formatting.Indented)}

המלץ על עד 5 שירים שמתאימים לזמן הנוכחי (יום בשבוע, שעה, עונה, חג) 
ולטעם האישי של המשתמש.

החזר JSON בלבד:
{{""recommendedIds"": [1, 2, 3]}}";

            try
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _apiKey);

                var body = new
                {
                    model = "gpt-4o-mini",
                    messages = new[] { new { role = "user", content = prompt } },
                    max_tokens = 200
                };

                var response = await client.PostAsync(
                    "https://api.openai.com/v1/chat/completions",
                    new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json")
                );

                var json = await response.Content.ReadAsStringAsync();
                dynamic result = JsonConvert.DeserializeObject(json)!;
                string content = result.choices[0].message.content;
                var clean = content.Replace("```json", "").Replace("```", "").Trim();
                var parsed = JsonConvert.DeserializeObject<dynamic>(clean)!;
                var ids = ((Newtonsoft.Json.Linq.JArray)parsed.recommendedIds)
                    .Select(x => (int)x).ToList();

                return allSongs.Where(s => ids.Contains(s.SongId)).ToList();
            }
            catch
            {
                return allSongs.Take(5).ToList();
            }
        }

        public async Task UpdateRecommendationsForAllUsers(int newSongId)
        {
            var allSongs = _songRepo.GetAll().ToList();
            var newSong = allSongs.FirstOrDefault(s => s.SongId == newSongId);
            if (newSong == null) return;

            var allHistory = _historyRepo.GetAll().ToList();
            var userIds = allHistory.Select(h => h.UserId).Distinct().ToList();

            foreach (var userId in userIds)
            {
                var userHistory = allHistory.Where(h => h.UserId == userId).ToList();

                var historySummary = userHistory
                    .GroupBy(h => h.SongId)
                    .Select(g => {
                        var song = allSongs.FirstOrDefault(s => s.SongId == g.Key);
                        return new
                        {
                            title = song?.Title ?? "",
                            genre = song?.Genre ?? "",
                            listenCount = g.Count(),
                            totalDuration = g.Sum(h => h.Duration)
                        };
                    }).ToList();

                var prompt = $@"
משתמש האזין לשירים הבאים:
{JsonConvert.SerializeObject(historySummary)}

שיר חדש נוסף למאגר:
כותרת: {newSong.Title}
זמר: {newSong.ArtistName}
ז'אנר: {newSong.Genre}
תיאור: {newSong.LyricsSummary}

האם השיר החדש מתאים למשתמש לפי טעמו? ענה רק ב-JSON:
{{""suitable"": true}} או {{""suitable"": false}}";

                try
                {
                    using var client = new HttpClient();
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", _apiKey);

                    var body = new
                    {
                        model = "gpt-4o-mini",
                        messages = new[] { new { role = "user", content = prompt } },
                        max_tokens = 50
                    };

                    var response = await client.PostAsync(
                        "https://api.openai.com/v1/chat/completions",
                        new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json")
                    );

                    var json = await response.Content.ReadAsStringAsync();
                    dynamic result = JsonConvert.DeserializeObject(json)!;
                    string content = result.choices[0].message.content;
                    var parsed = JsonConvert.DeserializeObject<dynamic>(content)!;

                    bool suitable = (bool)parsed.suitable;
                    if (suitable)
                    {
                        var existing = _recommendationRepo.GetAll()
                            .Any(r => r.UserId == userId && r.SongId == newSongId);
                        if (!existing)
                        {
                            await _recommendationRepo.AddAsync(new Recommendation
                            {
                                UserId = userId,
                                SongId = newSongId,
                                RecommendedAt = DateTime.Now
                            });
                        }
                    }
                }
                catch { }
            }
        }
    }
}
