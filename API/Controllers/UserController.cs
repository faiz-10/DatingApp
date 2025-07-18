using API.Data;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    public class UserController(IMemberRepository repository) : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Member>>> GetUser()
        {
            return Ok(await repository.GetMembersAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Member>> GetUser(string id)
        {
            var user = await repository.GetMemberById(id);
            if (user == null) { return NotFound(); }
            return user;
        }

        [HttpGet("{id}/photos")]
        public async Task<ActionResult<IReadOnlyList<Photo>>> GetUserPhotos(string id)
        {
            return Ok(await repository.GetPhotosForMemberAsync(id));
        }
    }

}
