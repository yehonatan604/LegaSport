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
using LegaSport.Entities.Models.Users;
using System.IO;

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

            DgridController(Dgrid1);
            RefreshDataGrid();

            Fill(new ItemTypes(), BoxItemType);
            Fill(new ColorTypes(), BoxColor);
            Fill(new UserTypes(), CmboBoxChangeType);
            FillSalesBoxes();
            FillLogsBoxes();

            EditStock_Tab.IsEnabled = reader.CheckAuthorizationLevel() < 2;
            Sales_Tab.IsEnabled = reader.CheckAuthorizationLevel() < 2;
            Users_Tab.IsEnabled = reader.CheckAuthorizationLevel() == 0;
            Logs_Tab.IsEnabled = reader.CheckAuthorizationLevel() == 0;

            StartClock();

            LblUser.Content = $"{Write.LoggedInUser.FirstName} {Write.LoggedInUser.LastName}";
        }

        // Digital Clock
        private void StartClock()
        {
            DispatcherTimer clock = new()
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            clock.Tick += Tick;
            clock.Start();
        }
        private void Tick(object sender, EventArgs e)
        {
            LblTime.Content = DateTime.UtcNow.ToString("dddd, dd MMMM yyyy HH:mm:ss");
        }

        // Stock_Tab Event Handlers
        private void BtnSell_Click(object sender, RoutedEventArgs e)
        {
            writer.AddSale(Convert.ToInt16(TboxSellID.Text), Convert.ToInt16(TboxSellQuantity.Text));
            MessageBox.Show("Sale!");
        }
        private void Dgrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TboxInfo.Text = $"{GetDgridContent(Dgrid1, 1)}\n" +
                            $"Id: {GetDgridContent(Dgrid1)}\n" +
                            $"Price: {GetDgridContent(Dgrid1, 3)}\n" +
                            $"In Stock: {GetDgridContent(Dgrid1, 4)}\n";

            TboxDescription.Text = $"{GetDgridContent(Dgrid1, 1)}\n" +
                                   $"is a {GetDgridContent(Dgrid1, 2)} " +
                                   $"and it costs {GetDgridContent(Dgrid1, 3)} Shekels.\n" +
                                   $"we are working with this product\nsince: {GetDgridContent(Dgrid1, 5)}";

            TboxSellID.Text = GetDgridContent(Dgrid1);
        }
        private void TboxSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            TboxSearch.Text = string.Empty;
            TboxInfo.Text = string.Empty;
            TboxDescription.Text = string.Empty;
        }
        private void TboxSearch_OnKeyUp(object sender, KeyEventArgs e)
        {
            Dgrid1.ItemsSource = reader.Search(TboxSearch.Text).ToList();

        }
        private void Stocks_Tabs_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            RefreshDataGrid();
            DgridController(Dgrid1);
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
                Dgrid1.ItemsSource = reader.GetTable().ToList();
                MessageBox.Show("Item Added Succecfuly");
                Dgrid1.ItemsSource = reader.GetTable().ToList();
            }
            else { MessageBox.Show("Operation failed, could not Add Item."); }
        }
        private void BtnAddStock_Click(object sender, RoutedEventArgs e)
        {
            writer.AddStock(Convert.ToInt16(TboxItemId.Text), Convert.ToInt16(TboxQuantity.Text));
            MessageBox.Show("Stock Added Succecfuly");
            Dgrid1.ItemsSource = reader.GetTable().ToList();
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
        private void Sales_Tab_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (reader.CheckAuthorizationLevel() > 1)
            {
                MessageBox.Show("You are not authorized to do that!");
                return;
            }
            RefreshDataGrid(Dgrid2);
            DgridController(Dgrid2);
        }
        private void Dgrid2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TboxSaleInfo.Text = $"Item: {GetDgridContent(Dgrid2, 1)}\n" +
                                $"Type: {GetDgridContent(Dgrid2, 2)}\n\n" +
                                $"{GetDgridContent(Dgrid2, 4)} units were soled for total of {GetDgridContent(Dgrid2, 5)}\n" +
                                $"at {GetDgridContent(Dgrid2, 9)}\n" +
                                $"by: {GetDgridContent(Dgrid2, 7)} {GetDgridContent(Dgrid2, 8)}, id: {GetDgridContent(Dgrid2, 6)}";
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

        // Users_Tab Event Handlers
        private void Users_Tab_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (reader.CheckAuthorizationLevel() != 0)
            {
                MessageBox.Show("You are not authorized to do that!");
                return;
            }
            RefreshDataGrid(Dgrid3);
            DgridController(Dgrid3);
        }
        private void Dgrid3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                string salesCount = GetDgridContent(Dgrid3, 6) == string.Empty ? "0" : GetDgridContent(Dgrid3, 6);
                string salesTotal = GetDgridContent(Dgrid3, 5) == string.Empty ? "0" : GetDgridContent(Dgrid3, 5);

                TboxUserInfo.Text = $"User: {GetDgridContent(Dgrid3, 1)} {GetDgridContent(Dgrid3, 2)}, " +
                                    $"{reader.ReturnUserType(Convert.ToInt16(GetDgridContent(Dgrid3)))}\n" +
                                    $"id: {GetDgridContent(Dgrid3)}\n" +
                                    $"Email: {GetDgridContent(Dgrid3, 3)}\n\n" +
                                    $"Sales: {salesCount}\n" +
                                    $"Sales Total: {salesTotal}$\n" +
                                    $"Hire Date: {GetDgridContent(Dgrid3, 4)}\n";

                LblUserId.Text = $"User Id: {GetDgridContent(Dgrid3)}";
            }
            catch
            {
                RefreshDataGrid(Dgrid3);
            }
        }
        private void BtnChangeUserType_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show($"This will change the user type to {CmboBoxChangeType.Text}.\n " +
                                 "are you sure)?", "Change User Type",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                writer.ChangeUserType(Convert.ToInt16(GetDgridContent(Dgrid3)), CmboBoxChangeType.Text);
                RefreshDataGrid(Dgrid3);
            }

        }
        private void BtnChangeEmail_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show($"This will change user email to {TboxChangeMail.Text}.\n " +
                                 "are you sure)?", "Change User Email",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                writer.ChangeUserEmail(Convert.ToInt16(GetDgridContent(Dgrid3)), TboxChangeMail.Text);
                RefreshDataGrid(Dgrid3);
            }
        }
        private void TboxChangeMail_LostFocus(object sender, RoutedEventArgs e)
        {
            Validate.IsEmailValid(TboxChangeMail);
        }
        private void BtnChangeHireDate_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show($"This will change user hire date.\n " +
                                 "are you sure)?", "Change User Hire Date",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                writer.ChangeUserHireDate(Convert.ToInt16(GetDgridContent(Dgrid3)), TboxChangeHireDate.Text);
            }
        }
        private void BtnResetPass_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show($"This will reset user password to '12345'.\n " +
                                 "are you sure)?", "Change User Password",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                writer.ChangeUserPassword(Convert.ToInt16(GetDgridContent(Dgrid3)), Md5Hash.Create("12345"));
            }
        }
        private void BtnRemoveUser_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show($"This will remove user!.\n " +
                                 "are you sure)?", "Remove User",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                writer.RemoveUser(Convert.ToInt16(GetDgridContent(Dgrid3)));
                RefreshDataGrid(Dgrid3);
            }
        }

        // Logs_Tab Event Handlers
        private void Logs_Tab_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (reader.CheckAuthorizationLevel() != 0)
            {
                MessageBox.Show("You are not authorized to do that!");
                return;
            }
            RefreshDataGrid(Dgrid4);
            DgridController(Dgrid4);
        }
        private void Dgrid4_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                TboxLogInfo.Text = $"User: {GetDgridContent(Dgrid4, 2)} {GetDgridContent(Dgrid4, 3)}, " +
                                    $"{reader.ReturnUserType(Convert.ToInt16(GetDgridContent(Dgrid4, 1)))}\n" +
                                    $"id: {GetDgridContent(Dgrid4, 1)}\n\n" +
                                    $"Action Type: {GetDgridContent(Dgrid4, 4)}\n" +
                                    $"Date: {GetDgridContent(Dgrid4, 5)}\n";
            }
            catch
            {
                RefreshDataGrid(Dgrid4);
            }
        }
        private void BtnLogsGo_Click(object sender, RoutedEventArgs e)
        {
            bool byUser = CmboBoxLogByUserId.Text != string.Empty;
            bool byDate = CmboBoxLogByDate.Text != string.Empty;
            bool byAction = CmboBoxLogByAction.Text != string.Empty;

            if (byUser && byDate)
            {
                Dgrid4.ItemsSource = reader.GetTable("LogsByUserIdDate",
                                                     CmboBoxLogByDate.Text,
                                                     CmboBoxLogByUserId.Text).ToList();
            }
            if (byAction && byDate)
            {
                Dgrid4.ItemsSource = reader.GetTable("LogsByActionDate",
                                                      CmboBoxLogByAction.Text,
                                                      CmboBoxLogByDate.Text).ToList();
            }
            if (byAction && byUser)
            {
                Dgrid4.ItemsSource = reader.GetTable("LogsByActionId",
                                                      CmboBoxLogByUserId.Text,
                                                      CmboBoxLogByAction.Text).ToList();
            }
            if (byAction && byUser && byDate)
            {
                Dgrid4.ItemsSource = reader.GetTable("LogsByAll",
                                                      CmboBoxLogByDate.Text,
                                                      CmboBoxLogByUserId.Text,
                                                      CmboBoxLogByAction.Text).ToList();
            }
            return;
        }
        private void BtnMakeLogFile_Click(object sender, RoutedEventArgs e)
        {
            List<string> list = new();
            StreamWriter file = new(@"D:\CSharpProjects\LegaSport\LegaSport.View\Report.txt");
            foreach (var item in Dgrid4.Items)
            {
                list.Add(item.ToString() + "\n");
            }
            foreach (var item in list)
            {
                file.WriteLine(item);
            }
            file.Close();
        }

        // Exit & Start Event Handlers
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
        private void FillLogsBoxes()
        {
            foreach (string str in reader.GetList("ByLogUserId"))
            {
                CmboBoxLogByUserId.Items.Add(str);
            }
            foreach (string str in reader.GetList("ByLogAction"))
            {
                CmboBoxLogByAction.Items.Add(str);
            }
            foreach (string str in reader.GetList("ByLogDate"))
            {
                CmboBoxLogByDate.Items.Add(str);
            }
        }


        // DataGrids Handlers
        private string GetDgridContent(DataGrid dGrid, int cell = 0)
        {
            try
            {
                return ((TextBlock)dGrid.SelectedCells[cell].Column.GetCellContent(dGrid.SelectedCells[cell].Item)).Text;

            }
            catch
            {
                return String.Empty;
            }
        }
        private void DgridController(DataGrid currentGrid)
        {
            foreach (var control in MainGrid.Children)
            {
                if (control is DataGrid grid)
                {
                    grid.Visibility = Visibility.Collapsed;
                }
            }
            currentGrid.Visibility = Visibility.Visible;
        }
        private void RefreshDataGrid(DataGrid? dGrid = null)
        {
            if (dGrid == null)
            {
                Dgrid1.ItemsSource = reader.GetTable().ToList();
            }
            if (dGrid == Dgrid2)
            {
                Dgrid2.ItemsSource = reader.GetTable("Sales").ToList();
            }
            if (dGrid == Dgrid3)
            {
                Dgrid3.ItemsSource = reader.GetTable("Users").ToList();
            }
            if (dGrid == Dgrid4)
            {
                Dgrid4.ItemsSource = reader.GetTable("Logs").ToList();
            }
        }

    }
}

