using System.Numerics;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

public class MobileContext : DbContext
{
    public DbSet<Company> Companies { get; set; }
    public DbSet<Phone> Phones { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UploadedFile> Files { get; set; }

    public MobileContext()
    {
        Database.EnsureCreated();
    }

 /*   protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Company>()
        .HasMany(e => e.Phones)
        .WithOne(e => e.Company);
        

         modelBuilder.Entity<Phone>()
        .HasOne(e => e.Company);
    }*/
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Filename=Mobile.db");
    }
}