using Backend.Common.database;
using Backend.Interfaces;
using Backend.modules.user.Dto;
using Backend.src.modules.user.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Backend.modules.user;

public class UserService
{
    private readonly AppDbContext _context;
    private readonly UserManager<User> _userManagerContext;


    public UserService(AppDbContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManagerContext = userManager;
    }

    public async Task<IdentityResult> Create(CreateUserDto createUserDto)
    {
        var existingUser = await _userManagerContext.FindByEmailAsync(createUserDto.Email);

        if (existingUser != null)
        {
            return IdentityResult.Failed(
                new IdentityError { Description = "User already exists" }
            );
        }

        if (createUserDto.Cpf.Length > 11)
        {
            return IdentityResult.Failed(
                new IdentityError { Description = "CPF number too long " }
            );
        }

        var user = new User
        {
            Name = createUserDto.Name,
            UserName = createUserDto.Email,
            Email = createUserDto.Email,
            Cpf = createUserDto.Cpf,
        };

        var result = await _userManagerContext.CreateAsync(user, createUserDto.Password);

        if (!result.Succeeded)
        {
            return IdentityResult.Failed(
                new IdentityError { Description = "Creation failure" }
            );
        }

        return result;
    }

    public async Task<IBaseReturnPagination<UserListDto>> FindAll(int page, int perPage)
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
                Name = user.UserName,
                Email = user.Email
            })
            .ToListAsync();

        return new BaseReturnPagination<UserListDto>(page, perPage, total, items);
    }

    public async Task<bool> SoftDelete(string userId)
    {
        var rows = await _context.Users
            .Where(user => user.Id == userId)
            .ExecuteUpdateAsync(set =>
                set.SetProperty(user => user.DeletedAt, DateTime.UtcNow)
            );

        return rows > 0;
    }

    public async Task<bool> UserExists(string email)
    {
        return await _context.Users.AnyAsync(user => user.Email == email);
    }
}