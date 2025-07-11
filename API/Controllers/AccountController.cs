using System;
using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController(AppDbContext dbContext, ITokenService tokenService) : BaseApiController
{
    [HttpPost("register")] // /api/controller/register
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        if (await EmailTaken(registerDto.Email)) { return BadRequest("Email is already taken"); }

        using var hmac = new HMACSHA512();

        var user = new AppUser()
        {
            UserName = registerDto.Username,
            Email = registerDto.Email,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            PasswordSalt = hmac.Key
        };

        await dbContext.Users.AddAsync(user);
        await dbContext.SaveChangesAsync();

        return user.ToUserDto(tokenService);
    }

    private async Task<bool> EmailTaken(string email)
    {
        return await dbContext.Users.AnyAsync(u => u.Email.ToLower() == email.ToLower());
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await dbContext.Users.SingleOrDefaultAsync(u => u.Email.ToLower() == loginDto.Email.ToLower());

        if (user == null) { return Unauthorized("Invalid email address"); }

        using var hmac = new HMACSHA512(user.PasswordSalt);

        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

        for (int i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != user.PasswordHash[i])
            {
                return Unauthorized("Invalid password");
            }
        }
        return user.ToUserDto(tokenService);
    }
}
