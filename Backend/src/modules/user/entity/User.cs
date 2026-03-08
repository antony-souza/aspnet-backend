using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("users")]
public class User
{
    [Key] [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public Guid Id  { get; set; }

    [Column("name")]
    [Required(ErrorMessage = "Name is required")]
    public required string Name { get; set; }

    [Column("cpf")]
    [StringLength(11)]
    public string Cpf  { get; set; }

    [Column("email")]
    [Required(ErrorMessage = "Email is required")]
    public required string Email  { get; set; }

    [Column("password")]
    [Required(ErrorMessage = "Password is required")]
    public required string Password  { get; set; }
    
    [Column("created_at")]
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }
}