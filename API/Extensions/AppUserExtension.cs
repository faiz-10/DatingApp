using System;
using API.DTOs;
using API.Entities;
using API.Interfaces;

namespace API.Extensions;

public static class AppUserExtension
{
    public static UserDto ToUserDto(this AppUser user, ITokenService tokenService)
    {
        return new UserDto
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            Token = tokenService.CreateToken(user)
        };
    }
}
