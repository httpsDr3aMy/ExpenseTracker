using System;
using System.Windows;
using System.Windows.Controls;

namespace expensetracker
{
    public partial class AddTransactions : Page
    {
        private Frame _mainFrame;
        private Dashboard _dashboardPage;

        public AddTransactions(Frame mainFrame, Dashboard dashboardPage)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
            _dashboardPage = dashboardPage;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.Navigate(_dashboardPage);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string transactionName = tbName.Text;
            string transactionAmount = tbAmount.Text;
            string category = (cbCategory.SelectedItem as ComboBoxItem)?.Content?.ToString();

            if (string.IsNullOrWhiteSpace(transactionName))
            {
                MessageBox.Show("Wprowadź nazwę transakcji.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!double.TryParse(transactionAmount, out double amount))
            {
                MessageBox.Show("Wprowadź poprawną kwotę.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(category))
            {
                MessageBox.Show("Wybierz kategorię.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (category.StartsWith("[-]") && (!(transactionAmount.Contains("-"))))
            {
                MessageBox.Show("Nie dopisałeś minusa.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Transaction transaction = new Transaction(transactionName, amount, category);


            _dashboardPage.AddTransaction(transaction);

            _mainFrame.Navigate(_dashboardPage);
        }
    }
}
