using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Security.Cryptography;
using LegaSport.Logic.CRUD;
using LegaSport.View.Utilities;
using LegaSport.Entities.Enums;

namespace LegaSport.View
{
    public partial class RegisterWindow : Window
    {
        private readonly Write writer;
        private readonly Read reader;
        public RegisterWindow()
        {
            InitializeComponent();
            writer = new();
            reader = new();
        }

        private void RegBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Validate.Registeration(BoxFname.Text, BoxLname.Text, UserTypes.SalesMan,
                                       BoxEmail.Text, BoxPassword.Password, BoxConfirm.Password))
            {
                MessageBox.Show(writer.AddNewUser(BoxFname.Text, BoxLname.Text, UserTypes.SalesMan,
                                BoxEmail.Text, Md5Hash.Create(BoxConfirm.Password)) ?
                                "User Added Succecfuly" :
                                "Operation failed, could not register user.");
            }
            else
            {
                MessageBox.Show("Please make sure that all fields are full");
            }

        }

        private void BoxEmail_Check(object sender, RoutedEventArgs e)
        {
            Validate.IsEmailValid((TextBox)sender);
        }
    }
}
