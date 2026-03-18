using Backend.Base;
using Backend.src.modules.role.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.src.modules.role;

[ApiController]
[Route("roles")]
[Authorize]
public class RoleController : ControllerBase
{
    private readonly RoleService _roleService;

    public RoleController(RoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRoleDto createRoleDto)
    {
        var result = await _roleService.Create(createRoleDto);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors.Select(e => e.Description));
        }

        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> FindAll([FromQuery] BaseQueryControllerPagination query)
    {
        int page = query.Page;
        int perPage = query.PerPage;

        var data = await _roleService.FindAll(page, perPage);

        return Ok(data);
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRoleDto updateRoleDto)
    {
        var result = await _roleService.Update(id, updateRoleDto);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors.Select(e => e.Description));
        }

        return Ok();
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var result = await _roleService.Delete(id);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors.Select(e => e.Description));
        }

        return NoContent();
    }
}
