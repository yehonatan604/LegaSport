using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics.Metrics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using LegaSport.Entities.Enums;
using LegaSport.Logic.CRUD;
using LegaSport.View.Utilities;
using System.Windows.Threading;

namespace LegaSport.View
{
    public partial class MainWindow : Window
    {
        // Fields
        private readonly Write writer;
        private readonly Read reader;
        private readonly LogInWindow? loginWindow;

        // Constructor
        public MainWindow()
        {
            InitializeComponent();
            writer = new();
            reader = new();
            writer.OnStartProgram();

            if (!Write.IsRememberMe)
            {
                loginWindow = new();
                loginWindow.ShowDialog();
            }
            Dgrid2.Visibility= Visibility.Collapsed;

            Dgrid.ItemsSource = reader.GetTable().ToList();
            Dgrid2.ItemsSource = reader.GetTable("Sales").ToList();

            Fill(new ItemTypes(), BoxItemType);
            Fill(new ColorTypes(), BoxColor);

            EditStock_Tab.IsEnabled = reader.CheckAuthorizationLevel() < 2;
            Sales_Tab.IsEnabled = reader.CheckAuthorizationLevel() < 2;
            Employees_Tab.IsEnabled = reader.CheckAuthorizationLevel() == 0;
            Logs_Tab.IsEnabled = reader.CheckAuthorizationLevel() == 0;

            StartClock();

            LblUser.Content = $"{Write.LoggedInUser.FirstName} {Write.LoggedInUser.LastName}";

            FillSalesBoxes();
        }

        // Digital Clock
        private void StartClock()
        {
            DispatcherTimer clock = new DispatcherTimer();
            clock.Interval = TimeSpan.FromSeconds(1);
            clock.Tick += timer_Tick;
            clock.Start();
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            LblTime.Content = DateTime.UtcNow.ToString("dddd, dd MMMM yyyy HH:mm:ss");
        }

        // Stock_Tab Event Handlers
        private void BtnSell_Click(object sender, RoutedEventArgs e)
        {
            writer.AddSale(Convert.ToInt16(TboxSellID.Text), Convert.ToInt16(TboxSellQuantity.Text));
            MessageBox.Show("Sale!");
        }
        private void Dgrid2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TboxSaleInfo.Text = $"Item: {GetDgridContent(Dgrid2, 1)}\n" +
                                $"Type: {GetDgridContent(Dgrid2, 2)}\n\n" + 
                                $"{GetDgridContent(Dgrid2, 4)} units were soled for total of {GetDgridContent(Dgrid2, 5)}\n" +
                                $"at {GetDgridContent(Dgrid2, 9)}\n" +
                                $"by: {GetDgridContent(Dgrid2, 7)} {GetDgridContent(Dgrid2, 8)}, id: {GetDgridContent(Dgrid2, 6)}";
        }
        private void Dgrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TboxInfo.Text = $"{GetDgridContent(Dgrid, 1)}\n" +
                            $"Id: {GetDgridContent(Dgrid)}\n" +
                            $"Price: {GetDgridContent(Dgrid, 3)}\n" +
                            $"In Stock: {GetDgridContent(Dgrid, 4)}\n";

            TboxDescription.Text = $"{GetDgridContent(Dgrid, 1)}\n" +
                                   $"is a {GetDgridContent(Dgrid, 2)} " +
                                   $"and it costs {GetDgridContent(Dgrid, 3)} Shekels.\n" +
                                   $"we are working with this product\nsince: {GetDgridContent(Dgrid, 5)}";

            TboxSellID.Text = GetDgridContent(Dgrid);
        }
        private void TboxSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            TboxSearch.Text = string.Empty;
            TboxInfo.Text = string.Empty;
            TboxDescription.Text = string.Empty;
        }
        private void TboxSearch_OnKeyUp(object sender, KeyEventArgs e)
        {
            Dgrid.ItemsSource = reader.Search(TboxSearch.Text).ToList();

        }
        private void Stocks_Tabs_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Dgrid.ItemsSource = reader.GetTable().ToList();
            Dgrid2.Visibility = Visibility.Collapsed;
            Dgrid.Visibility = Visibility.Visible;
        }

        // EditStock_Tab Event Handlers
        private void BtnAddItem_Click(object sender, RoutedEventArgs e)
        {
            if (Validate.ItemCreation(BoxItemName.Text, BoxItemPrice.Text, BoxItemType.Text,
                                      BoxItemInnerType.Text,
                                      BoxColor.IsEnabled ? BoxColor.Text : "*",
                                      BoxSizeTypes.IsEnabled ? BoxSizeTypes.Text : "*"))
            {
                switch ((ItemTypes)Enum.Parse(typeof(ItemTypes), BoxItemType.Text))
                {
                    case ItemTypes.Clothe:
                        {
                            switch ((ClotheType)Enum.Parse(typeof(ClotheType), BoxItemInnerType.Text))
                            {
                                case ClotheType.Shirt:
                                    {
                                        writer.AddNewItem(BoxItemName.Text,
                                             (ItemTypes)Enum.Parse(typeof(ItemTypes),
                                             BoxItemType.Text), Convert.ToDouble(BoxItemPrice.Text),
                                             shirtSize: (ShirtSizeTypes)Enum.Parse(typeof(ShirtSizeTypes), BoxSizeTypes.Text),
                                             color: (ColorTypes)Enum.Parse(typeof(ColorTypes), BoxColor.Text),
                                             clothe: (ClotheType)Enum.Parse(typeof(ClotheType), BoxItemInnerType.Text));
                                        break;
                                    }
                                case ClotheType.Pants:
                                    {
                                        writer.AddNewItem(BoxItemName.Text,
                                             (ItemTypes)Enum.Parse(typeof(ItemTypes),
                                             BoxItemType.Text), Convert.ToDouble(BoxItemPrice.Text),
                                             size: Convert.ToInt32(BoxSizeTypes.Text),
                                             color: (ColorTypes)Enum.Parse(typeof(ColorTypes), BoxColor.Text),
                                             clothe: (ClotheType)Enum.Parse(typeof(ClotheType), BoxItemInnerType.Text));

                                        break;
                                    }
                                case ClotheType.Shorts:
                                    {
                                        writer.AddNewItem(BoxItemName.Text,
                                             (ItemTypes)Enum.Parse(typeof(ItemTypes),
                                             BoxItemType.Text), Convert.ToDouble(BoxItemPrice.Text),
                                             size: Convert.ToInt32(BoxSizeTypes.Text),
                                             color: (ColorTypes)Enum.Parse(typeof(ColorTypes), BoxColor.Text),
                                             clothe: (ClotheType)Enum.Parse(typeof(ClotheType), BoxItemInnerType.Text));
                                        break;
                                    }
                                case ClotheType.Shoes:
                                    {
                                        writer.AddNewItem(BoxItemName.Text,
                                             (ItemTypes)Enum.Parse(typeof(ItemTypes),
                                             BoxItemType.Text), Convert.ToDouble(BoxItemPrice.Text),
                                             size: Convert.ToInt32(BoxSizeTypes.Text),
                                             color: (ColorTypes)Enum.Parse(typeof(ColorTypes), BoxColor.Text),
                                             clothe: (ClotheType)Enum.Parse(typeof(ClotheType), BoxItemInnerType.Text));
                                        break;
                                    }
                            }
                            break;
                        }
                    case ItemTypes.Ball:
                        {
                            writer.AddNewItem(BoxItemName.Text,
                                             (ItemTypes)Enum.Parse(typeof(ItemTypes),
                                             BoxItemType.Text), Convert.ToDouble(BoxItemPrice.Text),
                                             color: (ColorTypes)Enum.Parse(typeof(ColorTypes), BoxColor.Text),
                                             balltype: (BallTypes)Enum.Parse(typeof(BallTypes), BoxItemInnerType.Text));
                            ;
                            break;
                        }
                    case ItemTypes.Bat:
                        {
                            writer.AddNewItem(BoxItemName.Text,
                                             (ItemTypes)Enum.Parse(typeof(ItemTypes),
                                             BoxItemType.Text), Convert.ToDouble(BoxItemPrice.Text),
                                             bat: (BatTypes)Enum.Parse(typeof(BatTypes), BoxItemInnerType.Text));
                            ;
                            break;
                        }
                    case ItemTypes.Net:
                        {
                            writer.AddNewItem(BoxItemName.Text,
                                             (ItemTypes)Enum.Parse(typeof(ItemTypes),
                                             BoxItemType.Text), Convert.ToDouble(BoxItemPrice.Text),
                                             net: (NetTypes)Enum.Parse(typeof(NetTypes), BoxItemInnerType.Text));
                            ;
                            break;
                        }
                }
                Dgrid.ItemsSource = reader.GetTable().ToList();
                MessageBox.Show("Item Added Succecfuly");
                Dgrid.ItemsSource = reader.GetTable().ToList();
            }
            else { MessageBox.Show("Operation failed, could not Add Item."); }
        }
        private void BtnAddStock_Click(object sender, RoutedEventArgs e)
        {
            writer.AddStock(Convert.ToInt16(TboxItemId.Text), Convert.ToInt16(TboxQuantity.Text));
            MessageBox.Show("Stock Added Succecfuly");
            Dgrid.ItemsSource = reader.GetTable().ToList();
        }
        private void BoxItemPrice_LostFocus(object sender, RoutedEventArgs e)
        {
            Validate.IsStringNaN((TextBox)sender, e);
        }
        private void BoxItemType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BoxItemInnerType.Items.Clear();
            switch (BoxItemType.SelectedItem)
            {
                case ItemTypes.Clothe:
                    {
                        Fill(new ColorTypes(), BoxColor);
                        BoxColor.IsEnabled = true;
                        BoxSizeTypes.IsEnabled = true;
                        Fill(new ClotheType(), BoxItemInnerType);

                        break;
                    }
                case ItemTypes.Ball:
                    {
                        Fill(new BallTypes(), BoxItemInnerType);
                        BoxColor.IsEnabled = true;
                        break;
                    }
                case ItemTypes.Bat: { Fill(new BatTypes(), BoxItemInnerType); break; }
                case ItemTypes.Net: { Fill(new NetTypes(), BoxItemInnerType); break; }

            }
            BoxItemInnerType.IsEnabled = true;
        }
        private void BoxItemInnerType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (BoxItemInnerType.SelectedItem)
            {
                case ClotheType.Shirt: { FillSizeTypes(ClotheType.Shirt); break; }
                case ClotheType.Pants: { FillSizeTypes(ClotheType.Pants); break; }
                case ClotheType.Shorts: { FillSizeTypes(ClotheType.Shorts); break; }
                case ClotheType.Shoes: { FillSizeTypes(ClotheType.Shoes); break; }
            }
        }

        // Sales_Tab Event Handlers

        private void Sales_Tab_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (reader.CheckAuthorizationLevel() > 1)
            {
                MessageBox.Show("You are not authorized to do that!");
                return;
            }
            Dgrid.Visibility = Visibility.Collapsed;
            Dgrid2.Visibility = Visibility.Visible;
        }
        private void TboxByItem_KeyUp(object sender, KeyEventArgs e)
        {
            Dgrid2.ItemsSource = reader.GetTable("ByItemId", TboxByItem.Text).ToList();
        }
        private void TboxByItemType_DropDownClosed(object sender, EventArgs e)
        {
            Dgrid2.ItemsSource = reader.GetTable("ByType", TboxByItemType.Text).ToList();
        }
        private void TboxBySalesman_DropDownClosed(object sender, EventArgs e)
        {
            Dgrid2.ItemsSource = reader.GetTable("BySalseMan", TboxBySalesman.Text).ToList();
        }
        private void TboxByDate_DropDownClosed(object sender, EventArgs e)
        {
            Dgrid2.ItemsSource = reader.GetTable("ByDate", TboxByDate.Text).ToList();
        }
        private void BtnMinMax_Click(object sender, RoutedEventArgs e)
        {
            Dgrid2.ItemsSource = reader.GetTable("ByTPrice", TboxMin.Text, TboxMax.Text).ToList();
        }
        private void TboxMin_GotFocus(object sender, RoutedEventArgs e)
        {
            TboxMin.Text = string.Empty;
        }
        private void TboxMax_GotFocus(object sender, RoutedEventArgs e)
        {
            TboxMax.Text = string.Empty;
        }

        // Employees_Tab Event Handlers

        // Logs_Tab Event Handlers

        // Exit/Etart Event Handlers
        private void OnExit(object sender, CancelEventArgs e)
        {
            writer.ExitProgram();
        }
        private void OnStartUp(object sender, RoutedEventArgs e)
        {
            writer.StartProgram();
        }

        // ComboBox Fillers
        private void Fill(Enum sender, ComboBox box)
        {
            foreach (var item in Enum.GetValues(sender.GetType()))
            {
                box.Items.Add(item);
            }
        }
        private void FillSizeTypes(ClotheType type)
        {
            BoxSizeTypes.Items.Clear();
            switch (type)
            {
                case ClotheType.Shirt:
                    {
                        foreach (var item in Enum.GetValues(typeof(ShirtSizeTypes)))
                        {
                            BoxSizeTypes.Items.Add(item);
                        }
                        break;
                    }
                default:
                    {
                        for (int i = 26; i <= 52; i++)
                        {
                            BoxSizeTypes.Items.Add(i);
                        }
                        break;
                    }
            }
        }
        private void FillSalesBoxes()
        {
            foreach (string str in reader.GetList("ByItem"))
            {
                TboxByItemType.Items.Add(str);
            }
            foreach (string str in reader.GetList("BySalesMan"))
            {
                TboxBySalesman.Items.Add(str);
            }
            foreach (string str in reader.GetList("ByDate"))
            {
                TboxByDate.Items.Add(str);
            }
        }

        // Get Content from DataGrid
        private string GetDgridContent(DataGrid dGrid, int cell = 0)
        {
            return ((TextBlock)dGrid.SelectedCells[cell].Column.GetCellContent(dGrid.SelectedCells[cell].Item)).Text;
        }
    }
}
