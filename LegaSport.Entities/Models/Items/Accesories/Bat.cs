using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegaSport.Entities.Models.Items.Accesories
{
    public class Bat
    {
        public int Id { get; set; }
        public Item Item { get; set; }
        public BatTypes BatType { get; set; }
    }
}
