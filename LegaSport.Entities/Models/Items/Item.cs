using LegaSport.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegaSport.Entities.Models.Items
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ItemType { get; set; }
        public double Price { get; set; }
        public DateTime Created { get; set; }

        public Item(string name, string itemType, double price)
        {
            Name = name;
            ItemType = itemType;
            Price = price;
            Created = DateTime.Now;
        }
    }

    
}
