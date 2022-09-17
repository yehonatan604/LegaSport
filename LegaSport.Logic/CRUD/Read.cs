using LegaSport.Entities.Enums;
using LegaSport.Entities.Models.Context;
using LegaSport.Entities.Models.Users;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LegaSport.Logic.CRUD
{
    public class Read
    {
        // Db Connector Access
        private readonly StoreContext db;

        // Constructor
        public Read()
        {
            db = DbConnector.GetInstance().GetDb();
        }

        // Checks
        public bool CheckLastSessionExit()
        {
            ActionTypes actionType = (from action in db.Logs
                                      orderby action.Id ascending
                                      select action.ActionType).Last();

            return actionType == ActionTypes.Exit;
        }
        public bool CheckAvailability(string email)
        {
            var record = from user in db.Users
                         where user.Email == email
                         select user;

            return !record.Any();
        }
        public bool CheckLogin(string email, string password)
        {
            var record = from user in db.Users
                         where user.Email == email && user.Password == password
                         select user.Id;

            ActionTypes actionType = record.Any() ? ActionTypes.Login : ActionTypes.LoginFailed;
            string text = record.Any() ? $"{email} Logged in succesfully" : "Login failed";
            return record.Any();
        }
        public int CheckAuthorizationLevel()
        {
            return (int)(Write.LoggedInUser.UserType);
        }

        // Return Table Methods
        public IEnumerable<object> GetTable(string s = "*")
        {
            switch (s)
            {
                case nameof(db.Stocks): { return db.Stocks; }
                default:
                    {
                        return from item in db.Items
                               join stock in db.Stocks
                               on item.Id equals stock.Item.Id
                               select new { item.Id, item.Name, item.ItemType, 
                                            item.Price, stock.Quantity, item.Created, 
                                            stock.LastAdded };
                    }
            };
        }
        public IEnumerable<object> Search(string str)
        {
            return from items in db.Items
                   where (items.Name).Contains(str)
                   select items;
        }
    }
}
