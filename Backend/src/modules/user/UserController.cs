using Microsoft.AspNetCore.Mvc;

namespace Backend.modules.user;

[ApiController]
[Route("users")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [Route("/")]
    public IActionResult FindAll()
    {
        var data =  _userService.FindAll();

        return Ok(data);
    }
}