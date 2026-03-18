using Backend.Common.database;
using Backend.Interfaces;
using Backend.src.modules.role.Dto;
using Backend.src.modules.role.entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Backend.src.modules.role;

public class RoleService
{
    private readonly RoleManager<Role> _roleManager;

    public RoleService(RoleManager<Role> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<IdentityResult> Create(CreateRoleDto createRoleDto)
    {
        var roleExists = await _roleManager.RoleExistsAsync(createRoleDto.Name);

        if (roleExists)
        {
            return IdentityResult.Failed(new IdentityError { Description = "Role already exists" });
        }

        var identityRole = new Role { Name = createRoleDto.Name };
        return await _roleManager.CreateAsync(identityRole);
    }

    public async Task<IdentityResult> Update(Guid id, UpdateRoleDto updateRoleDto)
    {
        var role = await _roleManager.FindByIdAsync(id.ToString());

        if (role == null)
        {
            return IdentityResult.Failed(new IdentityError { Description = "Role not found" });
        }

        role.Name = updateRoleDto.Name;
        role.UpdatedAt = DateTime.UtcNow;
        return await _roleManager.UpdateAsync(role);
    }

    public async Task<IBaseReturnPagination<RoleListDto>> FindAll(int page, int perPage)
    {
        var skip = (page - 1) * perPage;
        var query = _roleManager.Roles;

        var total = await query.CountAsync();

        var items = await query
            .AsNoTracking()
            .Skip(skip)
            .Take(perPage)
            .Select(r => new RoleListDto
            {
                Id = r.Id,
                Name = r.Name!
            })
            .ToListAsync();

        return new BaseReturnPagination<RoleListDto>(page, perPage, total, items);
    }

    public async Task<IdentityResult> Delete(Guid id)
    {
        var role = await _roleManager.FindByIdAsync(id.ToString());

        if (role == null)
        {
            return IdentityResult.Failed(new IdentityError { Description = "Role not found" });
        }

        role.DeletedAt = DateTime.UtcNow;
        return await _roleManager.UpdateAsync(role);
    }
}
