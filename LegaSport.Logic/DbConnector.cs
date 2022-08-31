using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using LegaSport.Entities.Models.Context; 

namespace LegaSport.Logic
{
    public class DbConnector
    {
        private readonly StoreContext db;
        private static DbConnector? instance;
        private static readonly object key = new();

        private DbConnector()
        {
            db = new StoreContext();
        }

        public static DbConnector GetInstance()
        {
            lock (key)
            {
                instance ??= new DbConnector();
                return instance;
            }
        }
        public StoreContext GetDb()
        {
            return db;
        }
    }
}
