using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using API.DTOs;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class Seed
{
    public static async Task SeedUsers(AppDbContext dbContext)
    {
        if (await dbContext.Users.AnyAsync()) { return; }

        var memberData = await File.ReadAllTextAsync("Data/UserSeedData.json");

        var members = JsonSerializer.Deserialize<List<SeedUserDto>>(memberData);

        if (members == null) { Console.WriteLine("no users"); return; }

        

        foreach (var member in members)
        {
            using var hmac = new HMACSHA512();
            var user = new AppUser
            {
                Id = member.Id,
                Email = member.Email,
                UserName = member.DisplayName,
                ImageUrl = member.ImageUrl,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd")),
                PasswordSalt = hmac.Key,
                Member = new Member
                {
                    Id = member.Id,
                    DateOfBirth = member.DateOfBirth,
                    DisplayName = member.DisplayName,
                    LastActive = member.LastActive,
                    City = member.City,
                    Country = member.Country,
                    Created = member.Created,
                    ImageUrl = member.ImageUrl,
                    Gender = member.Gender,
                    Description = member.Description
                }
            };

            user.Member.Photos.Add(new Photo
            {
                Url = member.ImageUrl!,
                MemberId = member.Id,
            });

            dbContext.Users.Add(user);
        }

        await dbContext.SaveChangesAsync();
    }
}
