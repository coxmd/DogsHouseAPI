using DogsHouse.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace DogsHouse.Infrastructure.Data
{
    public class DogsContext : DbContext
    {
        public DogsContext(DbContextOptions<DogsContext> options) : base(options) { }

        public DbSet<Dog> Dogs { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Dog>(entity =>
            {
                entity.HasIndex(e => e.Name).IsUnique();

                // Seed data
                entity.HasData(
                    new Dog { Id = 1, Name = "Neo", Color = "red & amber", TailLength = 22, Weight = 32 },
                    new Dog { Id = 2, Name = "Jessy", Color = "black & white", TailLength = 7, Weight = 14 }
                );
            });
        }
    }
}
