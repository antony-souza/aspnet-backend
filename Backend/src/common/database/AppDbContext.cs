using Backend.src.modules.organization.entitiy;
using Backend.src.modules.role.entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Backend.Common.database;

public class AppDbContext : IdentityDbContext<User, Role, Guid>
{

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Organization> Organizations { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Organization>()
            .HasMany(org => org.Users)
            .WithOne(user => user.Organization)
            .HasForeignKey(user => user.OrganizationId);

        builder.Entity<User>().HasQueryFilter(user => user.DeletedAt == null);
        builder.Entity<Organization>().HasQueryFilter(user => user.DeletedAt == null);
        builder.Entity<Role>().HasQueryFilter(role => role.DeletedAt == null);
    }
}