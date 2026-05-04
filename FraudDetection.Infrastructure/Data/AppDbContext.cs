using FraudDetection.Domain.Entities;
using FraudDetection.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FraudDetection.Infrastructure.Data;

public class AppDbContext : IdentityDbContext<AppUser>  // <- mudou aqui
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Transaction> Transactions => Set<Transaction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // <- obrigatório pro Identity

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(t => t.Id);

            entity.Property(t => t.UserId)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(t => t.Amount)
                .HasColumnType("numeric(18,2)");

            entity.Property(t => t.Location)
                .IsRequired()
                .HasMaxLength(10);

            entity.Property(t => t.RiskLevel)
                .HasConversion<string>();

            entity.Property(t => t.CreatedAt)
                .IsRequired();
        });
    }
}