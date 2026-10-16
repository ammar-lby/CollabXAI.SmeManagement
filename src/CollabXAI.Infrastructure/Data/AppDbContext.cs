using CollabXAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CollabXAI.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Sme> Smes => Set<Sme>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Sme>(entity =>
        {
            entity.HasKey(s => s.Id);

            entity.Property(s => s.OrganisationName)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(s => s.ContactEmail)
                .IsRequired()
                .HasMaxLength(254);

            // backup for the service check, the db also refuses duplicate emails
            entity.HasIndex(s => s.ContactEmail).IsUnique();
        });
    }
}
