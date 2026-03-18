namespace Backend.src.modules.role.Dto;

public class CreateRoleDto
{
    public string Name { get; set; } = string.Empty;
    public string? CustomName { get; set; }
}
