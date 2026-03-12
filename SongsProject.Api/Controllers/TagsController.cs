using Microsoft.AspNetCore.Mvc;
using SongsProject.Models;
using SongsProject.Services.Interfaces;

namespace SongsProject.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TagsController : ControllerBase
    {
        private readonly ITagService _service;

        public TagsController(ITagService service) { _service = service; }

        [HttpGet]
        public IActionResult GetAll() => Ok(_service.GetAll());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var tag = await _service.GetByIdAsync(id);
            if (tag == null) return NotFound();
            return Ok(tag);
        }

        [HttpPost]
        public async Task<IActionResult> Add(Tag tag) => Ok(await _service.AddAsync(tag));

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Dictionary<string, object> fields)
        {
            var result = await _service.UpdateAsync(id, fields);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result) return NotFound();
            return Ok();
        }
    }
}