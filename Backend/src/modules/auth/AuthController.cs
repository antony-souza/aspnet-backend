using Backend.src.modules.auth.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.src.modules.auth
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(CreateAuthDto createAuthDto)
        {
            var result = await _authService.SignIn(createAuthDto);

            if (!result.Succeeded)
            {
                return Unauthorized("Not Authorized");
            }

            return Ok();
        }

    }
}
