using Microsoft.EntityFrameworkCore;
using VerificationService.Models;

namespace VerificationService.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<VerificationCode> VerificationCodes => Set<VerificationCode>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<VerificationCode>(entity =>
        {
            entity.HasKey(v => v.Id);

            entity.Property(v => v.Email)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(v => v.Code)
                .IsRequired()
                .HasMaxLength(10);

            entity.Property(v => v.CreatedAt)
                .IsRequired();

            entity.Property(v => v.IsUsed)
                .HasDefaultValue(false);

            entity.HasIndex(v => v.Email);
        });
    }
}