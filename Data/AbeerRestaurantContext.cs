using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AbeerRestaurant.Models;

namespace AbeerRestaurant.Data
{
    public class AbeerRestaurantContext : DbContext
    {
        public AbeerRestaurantContext (DbContextOptions<AbeerRestaurantContext> options)
            : base(options)
        {
        }

        public DbSet<AbeerRestaurant.Models.FoodItem> FoodItem { get; set; } = default!;
        public DbSet<AbeerRestaurant.Models.Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FoodItem>().ToTable("FoodItem");
        }
    }
}
