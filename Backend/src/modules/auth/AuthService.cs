using Backend.src.modules.auth.Dto;
using Microsoft.AspNetCore.Identity;

namespace Backend.src.modules.auth;

public class AuthService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public AuthService(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<SignInResult> SignIn(CreateAuthDto createAuthDto)
    {
        var user = await _userManager.FindByEmailAsync(createAuthDto.Email);

        if (user == null)
        {
            return SignInResult.Failed;
        }

        return await _signInManager.PasswordSignInAsync(
            createAuthDto.Email,
            createAuthDto.Password,
            false,
            false
         );
    }
}

