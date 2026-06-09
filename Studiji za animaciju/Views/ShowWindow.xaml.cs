using Studiji_za_animaciju.Models;
using System.Windows;

namespace Studiji_za_animaciju.Views
{
    public partial class ShowWindow : Window
    {
        public Show ShowModel { get; private set; }

        public ShowWindow(Show show)
        {
            InitializeComponent();
            ShowModel = show;
            DataContext = this;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ShowModel.Title))
            {
                MessageBox.Show("Title is required.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DialogResult = true;
        }
    }
}
