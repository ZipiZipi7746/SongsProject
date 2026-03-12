using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SongsProject.Services
{
    public class SongAnalysisResult
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Genre { get; set; }
        public string Summary { get; set; }
    }
    public class AIService
    {
        // כאן מכניסים את ה-API Key שקיבלת מהאתר של OpenAI
        private const string ApiKey = "YOUR_OPENAI_API_KEY";
        public async Task<SongAnalysisResult> AnalyzeSongMetadata(string filePath)
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            try
            {
                // שלב 1: תמלול השמע לטקסט בעזרת Whisper
                string lyrics = await TranscribeAudioWithWhisper(filePath);

                // שלב 2: ניתוח הטקסט וחילוץ מטא-דאטה בעזרת GPT
                var metadata = await ExtractMetadataFromLyrics(lyrics);

                return metadata;
            }
            catch (Exception ex)
            {
                // במקרה של שגיאה, מחזירים אובייקט ריק כדי שהמערכת לא תקרוס
                return new SongAnalysisResult { Title = "שגיאה בניתוח", Artist = "לא ידוע", Genre = "כללי", Summary = ex.Message };
            }
        }

        private async Task<string> TranscribeAudioWithWhisper(string path)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ApiKey);

                using (var content = new MultipartFormDataContent())
                {
                    var fileContent = new ByteArrayContent(File.ReadAllBytes(path));
                    fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("audio/mpeg");

                    content.Add(fileContent, "file", Path.GetFileName(path));
                    content.Add(new StringContent("whisper-1"), "model");

                    var response = await client.PostAsync("https://api.openai.com/v1/audio/transcriptions", content);
                    response.EnsureSuccessStatusCode();

                    var result = await response.Content.ReadAsStringAsync();
                    dynamic json = JsonConvert.DeserializeObject(result);
                    return json.text;
                }
            }
        }

        private async Task<SongAnalysisResult> ExtractMetadataFromLyrics(string lyrics)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ApiKey);

                var requestBody = new
                {
                    model = "gpt-4o", // המודל החדש והמהיר ביותר
                    messages = new[]
                    {
                        new { role = "system", content = "You are a music expert. Analyze the lyrics and return ONLY a JSON object with: Title, Artist, Genre (one word like 'Sad', 'Happy', 'Pop'), and Summary (short description in Hebrew)." },
                        new { role = "user", content = $"Lyrics: {lyrics}" }
                    },
                    response_format = new { type = "json_object" } // מבטיח שנקבל JSON תקין
                };

                var content = new StringContent(JsonConvert.SerializeObject(requestBody), System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync("https://api.openai.com/v1/chat/completions", content);
                response.EnsureSuccessStatusCode();

                var responseString = await response.Content.ReadAsStringAsync();
                dynamic jsonResponse = JsonConvert.DeserializeObject(responseString);
                string jsonContent = jsonResponse.choices[0].message.content;

                // הפיכת ה-JSON מה-AI לאובייקט C# שלנו
                return JsonConvert.DeserializeObject<SongAnalysisResult>(jsonContent);
            }
        }
    }
}