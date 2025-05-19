using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyBudgetly.Application.Interfaces;
using MyBudgetly.Domain.Common;
using MyBudgetly.Domain.Users;

namespace MyBudgetly.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            ConfigureBaseEntity(entity);

            entity.Property(x => x.FirstName)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(x => x.LastName)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(x => x.Email)
                  .IsRequired()
                  .HasMaxLength(200);

            entity.HasIndex(x => x.Email)
                  .IsUnique();
        });
    }

    private void ConfigureBaseEntity<TEntity>(EntityTypeBuilder<TEntity> builder) where TEntity : BaseEntity
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.CreatedAt)
               .HasDefaultValueSql("GETUTCDATE()");
        builder.Property(e => e.UpdatedAt)
               .HasDefaultValueSql("GETUTCDATE()")
               .IsRequired(false);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = now;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}