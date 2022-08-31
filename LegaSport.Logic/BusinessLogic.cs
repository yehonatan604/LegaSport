using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LegaSport.Entities;
using LegaSport.Entities.Models.Context;
using LegaSport.Entities.Models.Items;
using Microsoft.EntityFrameworkCore;

namespace LegaSport.Logic
{
    public class BusinessLogic
    {
        private readonly StoreContext db;
        public BusinessLogic()
        {
            db = DbConnector.GetInstance().GetDb();
        }

        public IEnumerable<object> GetItems()
        {
            return from item in db.Items
                   select item;
        }

        public void AddNewItem(string name, ItemTypes itemType, float price)
        {
            db.Items.Add(new Item(name, itemType, price));
            db.SaveChanges();
        }
    }
}
