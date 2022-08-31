using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LegaSport.Entities;
using LegaSport.Logic;

namespace LegaSport.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BusinessLogic logic;
        public MainWindow()
        {
            InitializeComponent();
            logic = new();
            foreach (var item in Enum.GetValues(typeof(ItemTypes)))
            {
                Itemsss.Items.Add(item);
            }
        }
        private void NumValidate(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if(e.Handled = new Regex("[^0-9,.,0-9]+").IsMatch(textBox.Text))
            {
                MessageBox.Show("Enter Numbers Only!!!");
            }
        }

        private void Btn1_Click(object sender, RoutedEventArgs e)
        {
            Dgrid.ItemsSource = logic.GetItems().ToList();
        }

        private void Btn2_Click(object sender, RoutedEventArgs e)
        {
            logic.AddNewItem(ItemName.Text, (ItemTypes)Enum.Parse(typeof(ItemTypes), Itemsss.Text), float.Parse(MyPrice.Text));
            Dgrid2.ItemsSource = logic.GetItems().ToList();
        }
    }
}
