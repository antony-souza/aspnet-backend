using Microsoft.AspNetCore.Identity;

namespace Backend.src.modules.role.entity;

public class Role : IdentityRole<Guid>
{
    public string CustomName { get; set; }

    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }
}
