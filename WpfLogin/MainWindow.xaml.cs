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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;

namespace WpfLogin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CommonUser user;
        LoginError loginError;

        public MainWindow()
        {
            InitializeComponent();
            user = new CommonUser("root", "");
            UserID.Text = "root";
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            loginError = user.ValidatePassword(
                new List<IValidator>(
                    new IValidator[] {
                        new ValidatorSpecialChars(),
                        new ValidatorNumbers()
                    }));

            if (loginError == LoginError.OK)
            {
                App.Root = SuperUser.GetOrCreate(UserID.Text, Password.Text);
                Title = "Přihlášený " + App.Root.UserID;
                Password.Text = string.Empty;
            }
            else
            {
                Error.Content = loginError.ToString();
                Error.Visibility = Visibility.Visible;
            }
        }

        private void Password_TextChanged(object sender, TextChangedEventArgs e)
        {
            user.Password = (sender as TextBox).Text;
            // modifikuj pouze instanci IValidator[], pokud chceš jiná kritéria
            loginError = user.ValidatePassword(
                new List<IValidator>(
                    new IValidator[] {
                        new ValidatorSpecialChars(),
                        new ValidatorNumbers()
                    }));

            if (loginError == LoginError.OK)
            {
                user.Password = Password.Text;
                Error.Visibility = Visibility.Hidden;
                return;
            }

            Error.Content = loginError.ToString();
            Error.Visibility = Visibility.Visible;
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            if (App.Root is not null)
            {
                SuperUser.Logout();
                Title = string.Empty;
            }
        }

        private void Change_Click(object sender, RoutedEventArgs e)
        {
            if (App.Root == null)
            {
                Error.Content = "Změna pouze po přihlášení";
                Error.Visibility = Visibility.Visible;
                return;
            }

            loginError = user.ValidatePassword(
                new List<IValidator>(
                    new IValidator[] {
                        new ValidatorSpecialChars(),
                        new ValidatorNumbers()
            }));

            if (loginError == LoginError.OK)
            {
                SuperUser.ChangePasswordTo(Password.Text);
                Password.Text = string.Empty;
                Error.Content = "heslo změněno";
            }
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
