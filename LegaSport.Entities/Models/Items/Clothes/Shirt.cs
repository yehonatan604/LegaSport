using LegaSport.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegaSport.Entities.Models.Items.Clothes
{
    public class Shirt
    {
        public int Id { get; set; }
        public Item Item { get; set; }
        public string ClothType { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
    }
}
