using DogsHouse.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogsHouse.Infrastructure.Data
{
    public class DogsContext : DbContext
    {
        public DogsContext(DbContextOptions<DogsContext> options) : base(options) { }

        public DbSet<Dog> Dogs { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dog>()
                .HasIndex(d => d.Name)
                .IsUnique();

            // Seed data
            modelBuilder.Entity<Dog>().HasData(
                new Dog { Id = 1, Name = "Neo", Color = "red & amber", TailLength = 22, Weight = 32 },
                new Dog { Id = 2, Name = "Jessy", Color = "black & white", TailLength = 7, Weight = 14 }
            );
        }
    }
}
