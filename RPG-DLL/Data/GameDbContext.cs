using Microsoft.EntityFrameworkCore;
using RPG_DLL.Entities;
using RPG_DLL.Systems;

public class GameContext : DbContext
{
    public DbSet<Player> Players { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<Equipement> Equipements { get; set; }
    public DbSet<Skill> Skills { get; set; }
    public DbSet<Stats> Stats { get; set; }
    public DbSet<Monster> Monsters { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=game.db");
    }
}
