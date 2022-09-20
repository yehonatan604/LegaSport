using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LegaSport.Entities.Models.Context;
using LegaSport.Entities.Models.Items;
using LegaSport.Entities.Models.Users;
using LegaSport.Entities.Enums;
using LegaSport.Entities.Models.Items.Clothes;
using LegaSport.Entities.Models.Items.Accesories;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Cryptography;

namespace LegaSport.Logic.CRUD
{
    public class Write
    {
        // Db Connector Access
        private readonly StoreContext db;

        // Properties
        public static User? LoggedInUser { get; set; }
        private static string LoggedUserEmail { get; set; } = string.Empty;
        public static bool IsRememberMe { get; set; } = false;
        public int NoUserId { get; set; }

        // Constructor
        public Write()
        {
            db = DbConnector.GetInstance().GetDb();
            NoUserId = db.Users.Single(x => x.UserType == UserTypes.No_User).Id;
        }

        // Public Static Method for Changing Current Logged-In User Email
        public static void ChangeLoggedUserEmail(string email)
        {
            LoggedUserEmail = email;
        }

        // Public Methods for changing selected user details
        public void ChangeUserType(int id, string type)
        {
            db.Users.Single(user => user.Id == id).UserType = (UserTypes)Enum.Parse(typeof(UserTypes), type);
            db.SaveChanges();
        }
        public void ChangeUserEmail(int id, string email)
        {
            db.Users.Single(user => user.Id == id).Email = email;
            db.SaveChanges();
        }
        public void ChangeUserHireDate(int id, string date)
        {
            db.Users.Single(user => user.Id == id).HireDate = Convert.ToDateTime(date).Date;
            db.SaveChanges();
        }
        public void ChangeUserPassword(int id, string password)
        {
            db.Users.Single(user => user.Id == id).Password = password;
            db.SaveChanges();
        }
        public void RemoveUser(int id)
        {
            db.Users.Remove(db.Users.Single(user => user.Id == id));
            db.SaveChanges();
        }

        // Add to log Methods
        public void AddLoggedIn(int id)
        {
            db.LoggedIns.Add(new Logged() { DateTime = DateTime.Now, User = db.Users.Single(x => x.Id == id) });
            db.SaveChanges();
            LoggedUserEmail = db.Users.Single(x => x.Id == id).Email;
            AddLog(id, $"id: {id} email:{LoggedUserEmail} logged in", ActionTypes.Login);
        }
        public void AddLog(int id, string description, ActionTypes actionType)
        {
            db.Logs.Add(new Log()
            {
                User = db.Users.Single(x => x.Id == id),
                DateTime = DateTime.Now,
                Description = description,
                ActionType = actionType
            });
            db.SaveChanges();
        }

        // Add to db Methods
        public void AddStock(int itemId, int quantity)
        {
            var id = from stock in db.Stocks
                     where stock.Item.Id == itemId
                     select stock;

            if (id.Any())
            {
                db.Stocks.Single(x => x.Item.Id == itemId).Quantity += quantity;
                db.Stocks.Single(x => x.Item.Id == itemId).LastAdded = DateTime.Now;
                db.SaveChanges();
                return;
            }

            db.Stocks.Add(new Stock()
            {
                Item = db.Items.Single(x => x.Id == itemId),
                Quantity = quantity,
                LastAdded = DateTime.Now
            });
            db.SaveChanges();
        }
        public bool AddSale(int itemId, int quantity)
        {
            var id = from stock in db.Stocks
                     where stock.Item.Id == itemId
                     select stock;

            if (quantity <= db.Stocks.Single(x => x.Item.Id == itemId).Quantity)
            {
                double tprice = db.Items.Single(x => x.Id == itemId).Price * quantity;
                int? scount = db.Users.Single(x => x.Id == LoggedInUser.Id).SalesCount;
                double? stotal = db.Users.Single(x => x.Id == LoggedInUser.Id).SalesTotal;

                scount = scount == null ? 1 : scount++;
                stotal = stotal == null ? tprice : stotal += tprice;

                db.Stocks.Single(x => x.Item.Id == itemId).Quantity -= quantity;
                db.Users.Single(x => x.Id == LoggedInUser.Id).LastSale = DateTime.Now;
                db.SaveChanges();


                db.Sales.Add(new Sale()
                {
                    Item = db.Items.Single(x => x.Id == itemId),
                    Quantity = quantity,
                    TotalPrice = tprice,
                    SaleDate = DateTime.Now,
                    User = LoggedInUser
                });
                db.SaveChanges();
                AddLog(LoggedInUser.Id, $"Sale! {LoggedInUser.Id} made a sale: {tprice}$", ActionTypes.Sale);
                return true;
            }
            return false;
        }
        public void AddNewItem(string name, ItemTypes itemType, double price,
                               BallTypes balltype = BallTypes.Soccer, ColorTypes color = ColorTypes.White,
                               ShirtSizeTypes shirtSize = ShirtSizeTypes.Medium, int size = 0,
                               BatTypes bat = BatTypes.BaseBall, NetTypes net = NetTypes.VolleyBall,
                               ClotheType clothe = ClotheType.Shirt)
        {
            db.Items.Add(new Item(name, itemType.ToString(), price));
            db.SaveChanges();

            int id = db.Items.Skip(db.Items.Count() - 1).First().Id;

            switch (itemType)
            {
                case ItemTypes.Ball:
                    {
                        db.Balls.Add(new Ball()
                        {
                            Item = db.Items.Single(x => x.Id == id),
                            BallType = balltype.ToString(),
                            Color = color.ToString()
                        });
                        break;
                    }
                case ItemTypes.Bat:
                    {
                        db.Bats.Add(new Bat()
                        {
                            BatType = bat.ToString(),
                            Item = db.Items.Single(x => x.Id == id)
                        });
                        break;
                    }
                case ItemTypes.Net:
                    {
                        db.Nets.Add(new Net()
                        {
                            Item = db.Items.Single(x => x.Id == id),
                            NetType = net.ToString()
                        });
                        break;
                    }
                case ItemTypes.Clothe:
                    {
                        switch (clothe)
                        {
                            case ClotheType.Shirt:
                                {
                                    db.Shirts.Add(new Shirt()
                                    {
                                        Item = db.Items.Single(x => x.Id == id),
                                        ClothType = clothe.ToString(),
                                        Color = color.ToString(),
                                        Size = shirtSize.ToString()
                                    });
                                    break;
                                }
                            case ClotheType.Pants:
                                {
                                    db.Pants.Add(new Pant()
                                    {
                                        Item = db.Items.Single(x => x.Id == id),
                                        ClothType = clothe.ToString(),
                                        Color = color.ToString(),
                                        Size = size
                                    });
                                    break;
                                }
                            case ClotheType.Shorts:
                                {
                                    db.Shorts.Add(new Short()
                                    {
                                        Item = db.Items.Single(x => x.Id == id),
                                        ClothType = clothe.ToString(),
                                        Color = color.ToString(),
                                        Size = size
                                    });
                                    break;
                                }
                            case ClotheType.Shoes:
                                {
                                    db.Shoes.Add(new Shoe()
                                    {
                                        Item = db.Items.Single(x => x.Id == id),
                                        ClothType = clothe.ToString(),
                                        Color = color.ToString(),
                                        Size = size
                                    });
                                    break;
                                }
                        }
                        break;
                    }
            }
            db.SaveChanges();
            AddLog(id, $"id: {LoggedInUser.Id} added a new item: {name}", ActionTypes.AddItem);
            AddStock(id, 0);
        }
        public bool AddNewUser(string firstName, string lastName, UserTypes userType,
                               string email, string password)
        {
            try
            {
                db.Users.Add(new User(firstName, lastName, userType, email, password));
                db.SaveChanges();
                int id = db.Users.Skip(db.Users.Count() - 1).First().Id;
                string description = $"{firstName} Registered Succesfully";
                AddLog(id, description, ActionTypes.Register);
            }
            catch
            {
                string description = "Registeration Failed";
                AddLog(NoUserId, description, ActionTypes.RegisterFailed);
                return false;
            }
            return true;
        }

        // Strart/End Event Handlers
        public void ExitProgram()
        {
            int userId = LoggedInUser != null ? LoggedInUser.Id : NoUserId;

            if (userId != NoUserId)
            {
                if (!LoggedInUser.RememberMe)
                {
                    db.LoggedIns.Clear(db);
                    AddLog(LoggedInUser.Id, $"{LoggedInUser.Id} logged off", ActionTypes.Logoff);
                }
            }

            AddLog(userId, $"id:{userId} exit program", ActionTypes.Exit);
        }
        public void StartProgram()
        {
            int userId = NoUserId;
            Read reader = new();

            if (!reader.CheckLastSessionExit())
            {
                AddLog(NoUserId, $"crush was found, previous session did not exit", ActionTypes.Crush);
            }

            if (LoggedInUser == null && LoggedUserEmail != string.Empty)
            {
                userId = (from user in db.Users
                          where user.Email == LoggedUserEmail
                          orderby user.Id
                          select user.Id).Last();

            }
            else if (db.LoggedIns.Any())
            {
                userId = (from user in db.LoggedIns
                          orderby user.Id
                          select user.User.Id).Last();

                LoggedUserEmail = (from user in db.LoggedIns
                                   orderby user.Id
                                   select user.User.Email).Last();
                AddLog(userId, $"id:{userId} start program", ActionTypes.StartUp);
                return;

            }
            else if (LoggedInUser == null && LoggedUserEmail == string.Empty)
            {
                Environment.Exit(0);
            }


            db.LoggedIns.Clear(db);
            AddLoggedIn(userId);
            LoggedInUser = (from user in db.LoggedIns
                            orderby user.Id
                            select user.User).Last();

            if (IsRememberMe)
            {
                LoggedInUser.RememberMe = true;
                LoggedInUser = (from user in db.LoggedIns
                                orderby user.Id
                                select user.User).Last();
            }

            AddLog(LoggedInUser.Id, $"id:{LoggedInUser.Id} start program", ActionTypes.StartUp);
        }
        public void OnStartProgram()
        {
            if (db.LoggedIns.Any())
            {
                IsRememberMe = true;
                LoggedInUser = (from user in db.LoggedIns
                                orderby user.Id
                                select user.User).Last();
            }
        }
    }
}
