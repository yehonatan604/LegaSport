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
        public ColorTypes Color { get; set; }
        public ShirtSizeTypes Size { get; set; }
    }
}
