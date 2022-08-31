using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegaSport.Entities.Models.Items.Accesories
{
    public class Ball
    {
        public int Id { get; set; }
        public Item Item { get; set; }
        public ColorTypes Color { get; set; }
        public BallTypes BallType { get; set; }
    }
}
