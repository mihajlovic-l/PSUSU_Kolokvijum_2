using Studiji_za_animaciju.Models;
using System.Windows;

namespace Studiji_za_animaciju.Views
{
    public partial class StudioWindow : Window
    {
        public Studio Studio { get; private set; }

        public StudioWindow(Studio studio)
        {
            InitializeComponent();
            Studio = studio;
            DataContext = this;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            // Simple validation
            if (string.IsNullOrWhiteSpace(Studio.Name))
            {
                MessageBox.Show("Name is required.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DialogResult = true;
        }
    }
}
