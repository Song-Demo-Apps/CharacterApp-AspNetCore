using CharacterApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CharacterApp.Data;

public class CharacterDbContext : DbContext
{
    public CharacterDbContext() : base() {}
    public CharacterDbContext(DbContextOptions options) : base(options) {}

    public DbSet<Character> Characters { get; set; }
    public DbSet<CharacterItem> CharacterItems { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<Species> Species { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("CharacterApp");

        modelBuilder.Entity<Character>(entity => {
            entity.Property(e => e.Name)
            .HasColumnType("nvarchar(100)")
            .IsRequired();

            entity.Property(e => e.Bio)
            .HasColumnType("nvarchar(1000)");

            entity.Property(e => e.Money)
            .HasColumnType("money")
            .IsRequired()
            .HasDefaultValue(0m);
        });

        modelBuilder.Entity<CharacterItem>(entity => {
            entity.Property(e => e.Quantity)
                .IsRequired()
                .HasDefaultValue(1);

            entity.ToTable(t => t.HasCheckConstraint("CK_CharacterItem_Quantity", "[Quantity] >= 1"));
        });

        modelBuilder.Entity<Item>(entity => {
            entity.Property(e => e.Name)
            .HasColumnType("nvarchar(50)")
            .IsRequired();

            entity.Property(e => e.Description)
            .HasColumnType("nvarchar(500)");

            entity.Property(e => e.Value)
            .HasColumnType("money")
            .IsRequired()
            .HasDefaultValue(0m);

            entity.ToTable(t => t.HasCheckConstraint("CK_Item_Value", "[Value] >= 0"));

        });

        modelBuilder.Entity<Species>(entity => {
            entity.Property(e => e.Name)
            .HasColumnType("nvarchar(50)")
            .IsRequired();

            entity.Property(e => e.Description)
            .HasColumnType("nvarchar(500)");
        });
    }
}