using LegaSport.Entities.Models.Items;
using LegaSport.Entities.Models.Items.Clothes;
using LegaSport.Entities.Models.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LegaSport.Entities.Models.Items.Accesories;

namespace LegaSport.Entities.Models.Context
{
    public class StoreContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Logged> LoggedIns { get; set; }
        public DbSet<Log> Logs { get; set; }

        public DbSet<Item> Items { get; set; }
        public DbSet<Shirt> Shirts { get; set; }
        public DbSet<Pant> Pants { get; set; }
        public DbSet<Short> Shorts { get; set; }
        public DbSet<Shoe> Shoes { get; set; }
        public DbSet<Ball> Balls { get; set; }
        public DbSet<Bat> Bats { get; set; }
        public DbSet<Net> Nets { get; set; }

        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Sale> Sales { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {
            optionBuilder.UseSqlServer("Server=DESKTOP-T74S10A;Database=LegaSport;Trusted_Connection = True;");
        }
    }
}
