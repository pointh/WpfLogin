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
        public MainWindow()
        {
            InitializeComponent();
        }

        private bool IsPasswordOK(string password, IValidator valid)
        {
            return valid.IsPasswordValid(password);
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            App.Root = SuperUser.GetOrCreate(UserID.Text, Password.Text);
            Title = "Přihlášený " + App.Root.UserID;
        }

        private void Password_TextChanged(object sender, TextChangedEventArgs e)
        {
            // modifikuj pouze instanci IValidator, pokud chceš jiná kritéria
            if (IsPasswordOK((sender as TextBox).Text, new ValidatorSpecialChars()))
            {
                Error.Visibility = Visibility.Hidden;
                return;
            }
            Error.Visibility = Visibility.Visible;
        }
    }
}
