using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class UserController(AppDbContext dbContext) : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<AppUser>>> GetUser()
        {
            var users = await dbContext.Users.ToListAsync();
            return users;
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetUser(string id)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            if(user == null) { return NotFound(); }
            return user;
        }
    }

}
