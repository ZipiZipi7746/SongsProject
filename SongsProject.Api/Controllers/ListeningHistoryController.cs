using Microsoft.AspNetCore.Mvc;
using SongsProject.Models;
using SongsProject.Services.Interfaces;

namespace SongsProject.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ListeningHistoryController : ControllerBase
    {
        private readonly IListeningHistoryService _service;
        public ListeningHistoryController(IListeningHistoryService service) { _service = service; }

        [HttpGet]
        public IActionResult GetAll() => Ok(_service.GetAll());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var history = await _service.GetByIdAsync(id);
            if (history == null) return NotFound();
            return Ok(history);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ListeningHistoryRequest request)
        {
            var history = new ListeningHistory
            {
                SongId = request.SongId,
                UserId = request.UserId,
                ListenDate = DateTime.Now,
                Duration = request.Duration
            };
            return Ok(await _service.AddAsync(history));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result) return NotFound();
            return Ok();
        }
    }

    public class ListeningHistoryRequest
    {
        public int SongId { get; set; }
        public int UserId { get; set; }
        public int Duration { get; set; }
    }
}