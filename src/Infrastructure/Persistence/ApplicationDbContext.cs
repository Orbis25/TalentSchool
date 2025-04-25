using FoundationKit.Domain.Persistence;
using Microsoft.EntityFrameworkCore;
using TalentSchool.Domain.Models;

namespace TalentSchool.Infrastructure.Persistence;

public class ApplicationDbContext : FoundationKitIdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        if (Database.GetPendingMigrations().Any())
        {
            Database.Migrate();
        }
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<User>()
            .Property(x => x.FullName)
            .IsRequired()
            .HasMaxLength(80);
        
        builder.Entity<User>()
            .HasQueryFilter(x => !x.IsDeleted);

        builder.Entity<Module>()
            .Property(x => x.Category)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.Entity<Module>()
            .HasQueryFilter(x => !x.IsDeleted);
        
        builder.Entity<Module>()
            .Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Entity<Progress>()
            .HasQueryFilter(x => !x.IsDeleted);
        
        builder.Entity<StaticContent>()
            .Property(x => x.Index)
            .IsRequired();
        
        builder.Entity<StaticContent>()
            .HasQueryFilter(x => !x.IsDeleted);

        builder.Entity<StaticContent>()
            .Property(x => x.Content).IsRequired();
        
        
        base.OnModelCreating(builder);
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Module> Modules { get; set; }
    public DbSet<Progress> Progresses { get; set; }
    public DbSet<StaticContent> StaticContents { get; set; }
}