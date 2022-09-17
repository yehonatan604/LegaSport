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

            Dgrid.ItemsSource = reader.GetTable().ToList();
            Fill(new ItemTypes(), BoxItemType);
            Fill(new ColorTypes(), BoxColor);

            EditStock_Tab.IsEnabled = reader.CheckAuthorizationLevel() < 2;
            Sales_Tab.IsEnabled = reader.CheckAuthorizationLevel() < 2;
            Employees_Tab.IsEnabled = reader.CheckAuthorizationLevel() == 0;
            Logs_Tab.IsEnabled = reader.CheckAuthorizationLevel() == 0;
        }

        // Stock_Tab Event Handlers
        private void BtnSell_Click(object sender, RoutedEventArgs e)
        {
            writer.AddSale(Convert.ToInt16(TboxSellID.Text), Convert.ToInt16(TboxSellQuantity.Text));
        }
        private void Dgrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TboxInfo.Text = $"{GetDgridContent(1)}\n\n" +
                            $"Id: {GetDgridContent()}\n" +
                            $"Price: {GetDgridContent(3)}\n" +
                            $"In Stock: {GetDgridContent(4)}\n";

            TboxDescription.Text = $"{GetDgridContent(1)}\n\n" +
                                   $"is a {GetDgridContent(2)} " +
                                   $"and it costs {GetDgridContent(3)} Shekels.\n" +
                                   $"we are working with this product\nsince: {GetDgridContent(5)}";

            TboxSellID.Text = GetDgridContent();
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

        // Employees_Tab Event Handlers

        // Logs_Tab Event Handlers

        // Exit/Etart Event Handlers event handlers
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

        // Get Content from DataGrid
        private string GetDgridContent(int cell = 0)
        {
            return ((TextBlock)Dgrid.SelectedCells[cell].Column.GetCellContent(Dgrid.SelectedCells[cell].Item)).Text;
        }
    }
}
