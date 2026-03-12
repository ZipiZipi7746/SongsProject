using Microsoft.AspNetCore.Mvc;
using SongsProject.Models;
using SongsProject.Services.Interfaces;

namespace SongsProject.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SongTagsController : ControllerBase
    {
        private readonly ISongTagService _service;

        public SongTagsController(ISongTagService service) { _service = service; }

        [HttpGet]
        public IActionResult GetAll() => Ok(_service.GetAll());

        [HttpPost]
        public async Task<IActionResult> Add(SongTag songTag) => Ok(await _service.AddAsync(songTag));

        [HttpDelete("{songId}/{tagId}")]
        public async Task<IActionResult> Delete(int songId, int tagId)
        {
            var result = await _service.DeleteAsync(songId, tagId);
            if (!result) return NotFound();
            return Ok();
        }
    }
}