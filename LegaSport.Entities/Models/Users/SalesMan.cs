using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegaSport.Entities.Models.Users
{
    public class SalesMan
    {
        public int Id { get; set; }
        public User User { get; set; }
        public DateTime HireDate { get; set; }
        public DateTime LastSale { get; set; }
        public int SalesBalance { get; set; }
        public int SalesCount { get; set; }
    }
}
