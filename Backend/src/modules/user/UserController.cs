using Backend.Base;
using Backend.modules.user;
using Backend.src.modules.user.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sprache;

[ApiController]
[Route("users")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateUserDto user)
    {
        var result = await _userService.Create(user);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors.Select(e => e.Description));
        }

        return Ok(result);
    }

    [HttpPut]
    [Route("{userId}")]
    public async Task<IActionResult> Update([FromRoute] Guid userId, UpdateUserDto updateUserDto)
    {
        var result = await _userService.Update(userId, updateUserDto);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors.Select(e => e.Description));
        }

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> FindAll([FromQuery] BaseQueryControllerPagination query)
    {
        int page = query.Page;
        int perPage = query.PerPage;

        var data = await _userService.FindAll(page, perPage);

        return Ok(data);
    }

    [HttpDelete]
    [Route("{userId}")]
    public async Task<IActionResult> Delete(Guid userId)
    {
        var data = await _userService.SoftDelete(userId);

        if (!data)
        {
            return NotFound("Teste");
        }

        return Ok(data);
    }
}