using Backend.Common.database;
using Backend.Interfaces;
using Backend.modules.user.Dto;
using Backend.src.modules.role.entity;
using Backend.src.modules.user.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Backend.modules.user;

public class UserService
{
    private readonly AppDbContext    _context;
    private readonly UserManager<User> _userManagerContext;
    private readonly RoleManager<Role> _roleManagerContext;

    public UserService(AppDbContext context, UserManager<User> userManager, RoleManager<Role> roleManager)
    {
        _context = context;
        _userManagerContext = userManager;
        _roleManagerContext = roleManager;
    }

    public async Task<IdentityResult> Create(CreateUserDto createUserDto)
    {
        if (!createUserDto.RoleId.HasValue)
        {
            return IdentityResult.Failed(
                new IdentityError { Description = "RoleId not found" }
            );
        }
        var existingUser = await _userManagerContext.FindByEmailAsync(createUserDto.Email);

        if (existingUser != null)
        {
            return IdentityResult.Failed(
                new IdentityError { Description = "User already exists" }
            );
        }

        if (!string.IsNullOrEmpty(createUserDto.Cpf) && createUserDto.Cpf.Length > 11)
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
            Cpf = createUserDto.Cpf
        };

        var result = await _userManagerContext.CreateAsync(user, createUserDto.Password);

        if (!result.Succeeded)
        {
            return IdentityResult.Failed(
                new IdentityError { Description = "Creation failure" }
            );
        }

        if (createUserDto.RoleId.HasValue)
        {
            var role = await _roleManagerContext.FindByIdAsync(createUserDto.RoleId.Value.ToString());
            if (role != null)
            {
                var roleResult = await _userManagerContext.AddToRoleAsync(user, role.Name!);
                if (!roleResult.Succeeded)
                {
                    return roleResult;
                }
            }
            else
            {
                return IdentityResult.Failed(
                    new IdentityError { Description = "Role not found in database" }
                );
            }
        }

        return result;
    }

    public async Task<IdentityResult> Update(Guid id, UpdateUserDto updateUserDto)
    {
        var existingUser = await _userManagerContext.FindByIdAsync(id.ToString());

        if (existingUser == null)
        {
            return IdentityResult.Failed(
                new IdentityError { Description = "User not found" }
            );
        }

        if (!string.IsNullOrEmpty(updateUserDto.Cpf) && updateUserDto.Cpf.Length > 11)
        {
            return IdentityResult.Failed(
                new IdentityError { Description = "CPF number too long" }
            );
        }

        if (!string.IsNullOrEmpty(updateUserDto.Name))
        {
            existingUser.Name = updateUserDto.Name;
        }

        if (!string.IsNullOrEmpty(updateUserDto.Email))
        {
            existingUser.Email = updateUserDto.Email;
            existingUser.UserName = updateUserDto.Email;
        }

        existingUser.Cpf = updateUserDto.Cpf ?? existingUser.Cpf;
        existingUser.OrganizationId = updateUserDto.OrganizationId ?? existingUser.OrganizationId;

        var result = await _userManagerContext.UpdateAsync(existingUser);

        if (result.Succeeded && updateUserDto.RoleId.HasValue)
        {
            var role = await _roleManagerContext.FindByIdAsync(updateUserDto.RoleId.Value.ToString());
            if (role != null)
            {
                var existingRoles = await _userManagerContext.GetRolesAsync(existingUser);
                if (!existingRoles.Contains(role.Name!))
                {
                    await _userManagerContext.RemoveFromRolesAsync(existingUser, existingRoles);
                    await _userManagerContext.AddToRoleAsync(existingUser, role.Name!);
                }
            }
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
                Name = user.Name,
                Email = user.Email,
                Role = _context.UserRoles
                    .Where(userRole => userRole.UserId == user.Id)
                    .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.CustomName)
                    .FirstOrDefault()
            })
            .ToListAsync();

        return new BaseReturnPagination<UserListDto>(page, perPage, total, items);
    }

    public async Task<bool> SoftDelete(Guid userId)
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