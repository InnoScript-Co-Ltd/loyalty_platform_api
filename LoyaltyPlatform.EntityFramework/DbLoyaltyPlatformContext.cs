using LoyaltyPlatform.EntityFramework.EntityModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyPlatform.EntityFramework
{
    public class DbLoyaltyPlatformContext : DbContext
    {
        public DbLoyaltyPlatformContext(DbContextOptions<DbLoyaltyPlatformContext> options) : base(options)
        {

        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }        
        public DbSet<City> Cities { get; set; }
        public DbSet<Township> Township { get; set; }
        public DbSet<Merchant> Township { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>().ToTable("Country");
            modelBuilder.Entity<State>().ToTable("State");
            modelBuilder.Entity<City>().ToTable("City");
            modelBuilder.Entity<Township>().ToTable("Township");
            modelBuilder.Entity<Merchant>().ToTable("Merchant");
        }
    }
}
