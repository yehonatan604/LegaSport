using LegaSport.Entities.Enums;
using LegaSport.Entities.Models.Context;
using LegaSport.Entities.Models.Items;
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
        public IEnumerable<object> GetTable(string s = "*", string arg1 = "", string arg2 = "")
        {
            switch (s)
            {
                case "ByItemId":
                    {
                        return from sale in db.Sales
                               where sale.Item.Id == Convert.ToInt16(arg1)
                               select new
                               {
                                   ItemID = sale.Item.Id,
                                   ItemName = sale.Item.Name,
                                   sale.Item.ItemType,
                                   sale.Item.Price,
                                   sale.Quantity,
                                   sale.TotalPrice,
                                   salesManId = sale.User.Id,
                                   salesManFName = sale.User.FirstName,
                                   salesManLName = sale.User.LastName,
                                   sale.SaleDate
                               };
                    }
                case "ByType":
                    {
                        return from sale in db.Sales
                               where sale.Item.ItemType == arg1
                               select new
                               {
                                   ItemID = sale.Item.Id,
                                   ItemName = sale.Item.Name,
                                   sale.Item.ItemType,
                                   sale.Item.Price,
                                   sale.Quantity,
                                   sale.TotalPrice,
                                   salesManId = sale.User.Id,
                                   salesManFName = sale.User.FirstName,
                                   salesManLName = sale.User.LastName,
                                   sale.SaleDate
                               };
                    }
                case "BySalseMan":
                    {
                        return from sale in db.Sales
                               where sale.User.Id == Convert.ToInt16(arg1)
                               select new
                               {
                                   ItemID = sale.Item.Id,
                                   ItemName = sale.Item.Name,
                                   sale.Item.ItemType,
                                   sale.Item.Price,
                                   sale.Quantity,
                                   sale.TotalPrice,
                                   salesManId = sale.User.Id,
                                   salesManFName = sale.User.FirstName,
                                   salesManLName = sale.User.LastName,
                                   sale.SaleDate
                               };
                    }
                case "ByDate":
                    {
                        return from sale in db.Sales
                               where sale.SaleDate.Date.ToString() == arg1
                               select new
                               {
                                   ItemID = sale.Item.Id,
                                   ItemName = sale.Item.Name,
                                   sale.Item.ItemType,
                                   sale.Item.Price,
                                   sale.Quantity,
                                   sale.TotalPrice,
                                   salesManId = sale.User.Id,
                                   salesManFName = sale.User.FirstName,
                                   salesManLName = sale.User.LastName,
                                   sale.SaleDate
                               };
                    }
                case "ByTPrice":
                    {
                        return from sale in db.Sales
                               where sale.TotalPrice > Convert.ToInt32(arg1) && sale.TotalPrice < Convert.ToInt32(arg2)
                               select new
                               {
                                   ItemID = sale.Item.Id,
                                   ItemName = sale.Item.Name,
                                   sale.Item.ItemType,
                                   sale.Item.Price,
                                   sale.Quantity,
                                   sale.TotalPrice,
                                   salesManId = sale.User.Id,
                                   salesManFName = sale.User.FirstName,
                                   salesManLName = sale.User.LastName,
                                   sale.SaleDate
                               };
                    }
                case nameof(db.Sales): 
                    {
                        return from sale in db.Sales
                               select new 
                               {
                                   ItemID = sale.Item.Id,
                                   ItemName = sale.Item.Name,
                                   sale.Item.ItemType,
                                   sale.Item.Price,
                                   sale.Quantity,
                                   sale.TotalPrice,
                                   salesManId = sale.User.Id,
                                   salesManFName = sale.User.FirstName,
                                   salesManLName = sale.User.LastName,
                                   sale.SaleDate
                               }; 
                    }
                default:
                    {
                        return from item in db.Items
                               join stock in db.Stocks
                               on item.Id equals stock.Item.Id
                               select new
                               {
                                   item.Id,
                                   item.Name,
                                   item.ItemType,
                                   item.Price,
                                   stock.Quantity,
                                   item.Created,
                                   stock.LastAdded
                               };
                    }
            };
        }
        public IEnumerable<object> Search(string str)
        {
            return from items in db.Items
                   where (items.Name).Contains(str)
                   select items;
        }

        // Return List Methods
        public List<string> GetList(string s)
        {
            switch (s)
            {
                case "ByItem":
                    {
                        return (from sale in db.Sales
                               select sale.Item.ItemType.ToString()).Distinct().ToList();
                    }
                case "BySalesMan":
                    {
                        return(from sale in db.Sales
                                select sale.User.Id.ToString()).Distinct().ToList();
                    }
                case "ByDate":
                    {
                        return (from sale in db.Sales
                                select sale.SaleDate.Date.ToString()).Distinct().ToList();
                    }
                default:
                    {
                        return new List<string>();
                    }
            }
        }
    }
}
