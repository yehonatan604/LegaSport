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
        public ItemTypes ItemType { get; set; }
        public float Price { get; set; }
        public DateTime Created { get; set; }

        public Item(string name, ItemTypes itemType, float price)
        {
            Name = name;
            ItemType = itemType;
            Price = price;
            Created = DateTime.Now;
        }
    }

    
}
