using System.ComponentModel.DataAnnotations;

namespace Backend.src.modules.user.Dto
{
    public class CreateUserDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        public string Password { get; set; } = string.Empty;
        
        public string? Cpf { get; set; }
        
        [Required(ErrorMessage = "RoleId is required")]
        public Guid? RoleId { get; set; }
    }
}
