using Backend.src.modules.organization.entitiy;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class User : IdentityUser<Guid>
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(11)]
    public string? Cpf { get; set; }

    [Column("organization_id")]
    public Guid? OrganizationId { get; set; } = null;

    public Organization? Organization { get; set; }

    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }
}