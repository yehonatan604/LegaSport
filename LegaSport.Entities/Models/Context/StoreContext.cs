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
        public DbSet<SalesMan> SalesMen { get; set; }

        public DbSet<Item> Items { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Sale> Sales { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {
            optionBuilder.UseSqlServer("Server=DESKTOP-T74S10A;Database=LegaSport;Trusted_Connection = True;");
        }
    }
}
