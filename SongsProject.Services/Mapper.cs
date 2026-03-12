using SongsProject.Models;
using SongsProject.Models.DTOs;

namespace SongsProject.Services
{
    public static class Mapper
    {
        public static SongDto ToDto(Song song) => new SongDto
        {
            SongId = song.SongId,
            Title = song.Title,
            ArtistName = song.ArtistName,
            Genre = song.Genre,
            ReleaseDate = song.ReleaseDate,
            LyricsSummary = song.LyricsSummary,
            AudioUrl = song.AudioUrl
        };

        public static UserDto ToDto(Users user) => new UserDto
        {
            UserId = user.UserId,
            Username = user.Username,
            Email = user.Email,
            CreatedAt = user.CreatedAt
        };

        public static TagDto ToDto(Tag tag) => new TagDto
        {
            TagId = tag.TagId,
            TagName = tag.TagName
        };

        public static RecommendationDto ToDto(Recommendation rec) => new RecommendationDto
        {
            RecommendationId = rec.RecommendationId,
            UserId = rec.UserId,
            SongId = rec.SongId,
            Score = rec.Score,
            IsSeen = rec.IsSeen
        };

        public static ListeningHistoryDto ToDto(ListeningHistory history) => new ListeningHistoryDto
        {
            HistoryId = history.HistoryId,
            UserId = history.UserId,
            SongId = history.SongId,
            ListenDate = history.ListenDate,
            Duration = history.Duration
        };

        public static SongTagDto ToDto(SongTag songTag) => new SongTagDto
        {
            SongId = songTag.SongId,
            TagId = songTag.TagId
        };
    }
}