using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.src.modules.organization.entitiy
{
    [Table("organization")]
    public class Organization
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("slug")]
        public string Slug { get; set; } = string.Empty;

        [Column("cnpj")]
        [MaxLength(14)]
        public int Cnpj { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [Column("deleted_at")]
        public DateTime? DeletedAt { get; set; }

        public ICollection<User>? Users { get; set; }

    }
}
