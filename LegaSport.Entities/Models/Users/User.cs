using LegaSport.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegaSport.Entities.Models.Users
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedDate { get; set; }
        public UserTypes UserType { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }

        public User(string firstName, string lastName, UserTypes userType, 
                    string email, string password)
        {
            FirstName = firstName;
            LastName = lastName;
            CreatedDate = DateTime.Now;
            UserType = userType;
            Email = email;
            Password = password;
            RememberMe = false;
        }
    }
}
