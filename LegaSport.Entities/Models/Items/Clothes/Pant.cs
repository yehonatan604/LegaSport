using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegaSport.Entities.Models.Items.Clothes
{
    public class Pant
    {
        public int Id { get; set; }
        public Item Item { get; set; }
        public ColorTypes Color { get; set; }
        public int Size { get; set; }
        public PantsTypes PantsType { get; set; }
    }
}
