using Backend.Common.database;
using Backend.Interfaces;
using Backend.modules.user.Dto;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Backend.modules.user;

public class UserService
{
    private readonly AppDbContext _context;
    
    public UserService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User> Create(User user)
    { 
        var existingUser = await UserExists(user.Email);

        if (existingUser)
        {
            return null!;
        }
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync(); 
        
        return user;
    }

    public async Task<IBaseReturnPagination<UserListDto>>FindAll(int page, int perPage)
    {
        var skip = (page - 1) * perPage;

        var total = await _context.Users.CountAsync();

        var items = await _context.Users
            .AsNoTracking()
            .Skip(skip)
            .Take(perPage)
            .Select(user => new UserListDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            })
            .ToListAsync();
        
        return new BaseReturnPagination<UserListDto>(page, perPage, total, items);
    }

    public async Task<bool> UserExists(string email)
    {
        return await _context.Users.AnyAsync(user => user.Email == email);
    }
        
}