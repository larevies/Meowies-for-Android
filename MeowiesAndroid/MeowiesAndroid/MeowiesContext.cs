using MeowiesAndroid.Models;
using Microsoft.EntityFrameworkCore;

namespace MeowiesAndroid;

public sealed class MeowiesContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Bookmark> Bookmarks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=meowies.db");
    }
}