using System.Windows;
using System.Windows.Navigation;

namespace Sharponzo.GraphicalInterface
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();

        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Uri.ToString());
        }

        private void EnterApplication()
        {
            var mainWindow = new MainWindow(txtAccess.Text);
            App.Current.MainWindow = mainWindow;
            this.Close();
            mainWindow.Show();
        }

        private void btnEnter_Click(object sender, RoutedEventArgs e)
        {
            // Run Some validation on the monzo access code here.
            // Use the HTTP response from monzo
            EnterApplication();
        }
    }
}
