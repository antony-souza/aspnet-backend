using Backend.Base;
using Backend.modules.user;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("users")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(User user)
    {
        var data = await _userService.Create(user);

        if (data == null)
        {
            return BadRequest("Already exists");
        }
        
        return Ok(data);
    }

    [HttpGet]
    public async Task<IActionResult> FindAll([FromQuery] BaseQueryControllerPagination query)
    {
        int page = query.Page;
        int perPage = query.PerPage;
        
        var data =  await _userService.FindAll(page, perPage);
        
        return Ok(data);
    }
}