using SongsProject.Models;
using SongsProject.Models.DTOs;
using SongsProject.Repositories.Interfaces;
using SongsProject.Services.Interfaces;

namespace SongsProject.Services
{
    public class TagServiceImpl : ITagService
    {
        private readonly ITagRepository _repo;

        public TagServiceImpl(ITagRepository repo) { _repo = repo; }

        public IEnumerable<TagDto> GetAll() => _repo.GetAll().ToList().Select(Mapper.ToDto);

        public async Task<TagDto?> GetByIdAsync(int id)
        {
            var tag = await _repo.GetByIdAsync(id);
            return tag == null ? null : Mapper.ToDto(tag);
        }

        public async Task<TagDto> AddAsync(Tag tag)
        {
            await _repo.AddAsync(tag);
            return Mapper.ToDto(tag);
        }

        public async Task<TagDto?> UpdateAsync(int id, Dictionary<string, object> fields)
        {
            var tag = await _repo.GetByIdAsync(id);
            if (tag == null) return null;
            if (fields.ContainsKey("tagName")) tag.TagName = fields["tagName"].ToString()!;
            await _repo.UpdateAsync(tag);
            return Mapper.ToDto(tag);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var tag = await _repo.GetByIdAsync(id);
            if (tag == null) return false;
            await _repo.DeleteAsync(tag);
            return true;
        }
    }
}