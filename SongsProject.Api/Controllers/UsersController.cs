using Microsoft.AspNetCore.Mvc;
using SongsProject.Models;
using SongsProject.Services.Interfaces;

namespace SongsProject.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;

        public UsersController(IUserService service) { _service = service; }

        [HttpGet]
        public IActionResult GetAll() => Ok(_service.GetAll());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _service.GetByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Add(Users user) => Ok(await _service.AddAsync(user));

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