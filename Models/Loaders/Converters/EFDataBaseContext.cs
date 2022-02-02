using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Carshops_MVC
{
    public class EFDataBaseContext : DbContext
    {
        public DbSet<Office> Offices => Set<Office>();
        public DbSet<CarItem> CarItems => Set<CarItem>();
        public EFDataBaseContext()
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json").AddEnvironmentVariables().Build();
            optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Office>().HasData
            (
                new Office("Audi", "SQL")
                {
                    OfficeId = 1,
                },
                new Office("Scoda", "SQL")
                {
                    OfficeId = 2,
                }
            );
            modelBuilder.Entity<CarItem>(entity =>
            {
                entity.HasOne(item => item.Office).WithMany(office => office.Cars).HasForeignKey(item => item.OfficeId).OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<SparePart>(entity =>
            {
                entity.HasOne(item => item.Office).WithMany(office => office.Parts).HasForeignKey(item => item.OfficeId).OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<CarItem>().HasData
            (
                new CarItem { OfficeId = 1, ItemId = 1, Brand = "Audi", Model = "A228", Price = 4, StockBalance = 14 },
                new CarItem { OfficeId = 1, ItemId = 2, Brand = "Audi", Model = "Super", Price = 44, StockBalance = 5 },
                new CarItem { OfficeId = 2, ItemId = 3, Brand = "Scoda", Model = "Logan", Price = 47, StockBalance = 9 },
                new CarItem { OfficeId = 2, ItemId = 4, Brand = "Scoda", Model = "Spider", Price = 78, StockBalance = 3 }
            );
        }
    }
}
