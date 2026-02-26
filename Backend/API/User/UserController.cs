using Backend.Application.User;
using Microsoft.AspNetCore.Mvc;

namespace Backend.API.User;

[ApiController]
[Route("/users")] 
public class UserController
{
    private readonly UserService _userService;
    
    public UserController(UserService userService)
    {
        _userService = userService;
    }
    
    [HttpGet(Name = "GetUsers")]

    public IActionResult GetUsers()
    { 
         var response = _userService.GetMessage();
         return Ok(response);
    }
    
    
}