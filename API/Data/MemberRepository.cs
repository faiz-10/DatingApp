using System;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class MemberRepository(AppDbContext dbContext) : IMemberRepository
{
    public async Task<Member?> GetMemberById(string id)
    {
        return await dbContext.Members.FindAsync(id);
    }

    public async Task<IReadOnlyList<Member>> GetMembersAsync()
    {
        return await dbContext.Members.ToListAsync();
    }

    public async Task<IReadOnlyList<Photo>> GetPhotosForMemberAsync(string id)
    {
        return await dbContext.Members.Where(m => m.Id == id).SelectMany(m => m.Photos).ToListAsync();
    }

    public async Task<bool> SaveAllAsync()
    {
        return await dbContext.SaveChangesAsync() > 0;
    }

    public void Update(Member member)
    {
        dbContext.Entry(member).State = EntityState.Modified;
    }
}
