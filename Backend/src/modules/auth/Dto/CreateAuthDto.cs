namespace Backend.src.modules.auth.Dto;

public class CreateAuthDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}