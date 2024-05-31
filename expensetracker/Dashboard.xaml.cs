using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace expensetracker
{
    public partial class Dashboard : Page
    {
        private ObservableCollection<Transaction> incomeTransactions;
        private ObservableCollection<Transaction> expenseTransactions;
        private Frame _mainFrame;
        private ResourceDictionary lightMode;
        private ResourceDictionary darkMode;

        public Dashboard(Frame mainFrame)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
            incomeTransactions = new ObservableCollection<Transaction>();
            expenseTransactions = new ObservableCollection<Transaction>();
            lbIncomeTransactions.ItemsSource = incomeTransactions;
            lbExpenseTransactions.ItemsSource = expenseTransactions;
            lightMode = new ResourceDictionary() { Source = new Uri("LightMode.xaml", UriKind.Relative) };
            darkMode = new ResourceDictionary() { Source = new Uri("DarkMode.xaml", UriKind.Relative) };
            SetTheme(lightMode);
        }

        private void DarkModeRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton radioButton && radioButton.IsChecked == true && Application.Current.Resources.MergedDictionaries.Count > 0)
            {
                Application.Current.Resources.MergedDictionaries[0].Source = new Uri("DarkModeResource.xaml", UriKind.RelativeOrAbsolute);
            }
        }

        private void LightModeRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton radioButton && radioButton.IsChecked == true && Application.Current.Resources.MergedDictionaries.Count > 0)
            {
                Application.Current.Resources.MergedDictionaries[0].Source = new Uri("LightModeResource.xaml", UriKind.RelativeOrAbsolute);
            }
        }


        public void AddTransaction(Transaction transaction)
        {
            string iconPath = GetIconPath(transaction.Category);
            transaction.Icon = new BitmapImage(new Uri(iconPath, UriKind.Relative));

            UpdateTransactionsLists(transaction);
        }

        private string GetIconPath(string category)
        {
            string lowerCategory = category.ToLower();

            if (lowerCategory.Contains("jedzenie"))
            {
                return "/food.png";
            }
            else if (lowerCategory.Contains("rozrywka"))
            {
                return "/entertainment.png";
            }
            else if (lowerCategory.Contains("zakupy"))
            {
                return "/shopping.png";
            }
            else
            {
                return "/money.png";
            }
        }

        private void UpdateTransactionsLists(Transaction transaction)
        {
            double currentMoney = Convert.ToDouble(tbMoney.Text);
            double updatedMoney = transaction.Amount + currentMoney;
            if (transaction.Amount >= 0)
            {
                incomeTransactions.Add(transaction);
            }
            else
            {
                expenseTransactions.Add(transaction);
            }
            tbMoney.Text = updatedMoney.ToString();
        }

        private void btnAddTransaction_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.Navigate(new AddTransactions(_mainFrame, this));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            incomeTransactions.Clear();
            expenseTransactions.Clear();
            tbMoney.Text = "0";
        }

        private void RemoveIncome_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                Transaction transaction = button.DataContext as Transaction;
                if (transaction != null)
                {
                    incomeTransactions.Remove(transaction);

                    double currentMoney = Convert.ToDouble(tbMoney.Text);
                    double updatedMoney = currentMoney - transaction.Amount;
                    tbMoney.Text = updatedMoney.ToString();
                }
            }
        }

        private void RemoveExpense_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                Transaction transaction = button.DataContext as Transaction;
                if (transaction != null)
                {
                    expenseTransactions.Remove(transaction);

                    double currentMoney = Convert.ToDouble(tbMoney.Text);
                    double updatedMoney = currentMoney - transaction.Amount;
                    tbMoney.Text = updatedMoney.ToString();
                }
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if ((sender as RadioButton).Content.ToString() == "Ciemny")
            {
                SetTheme(darkMode);
            }
            else
            {
                SetTheme(lightMode);
            }
        }

        private void SetTheme(ResourceDictionary theme)
        {
            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(theme);
        }
    }

    public class Transaction
    {
        public string Name { get; set; }
        public double Amount { get; set; }
        public string Category { get; set; }
        public BitmapImage Icon { get; set; }

        public Transaction(string name, double amount, string category)
        {
            Name = name;
            Amount = amount;
            Category = category;
        }
    }
}
