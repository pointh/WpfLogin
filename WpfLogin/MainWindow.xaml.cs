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

        private bool IsPasswordOK(string password)
        {
            return password.Length > 7 && Regex.IsMatch(password, "w*\\dw*");
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            App.Root = SuperUser.GetOrCreate(UserID.Text, Password.Text);
            Title = "Přihlášený " + App.Root.UserID;
        }

        private void Password_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsPasswordOK((sender as TextBox).Text))
            {
                Error.Visibility = Visibility.Hidden;
                return;
            }
            Error.Visibility = Visibility.Visible;
        }
    }

    public class SuperUser
    {
        public string UserID { get; set; }
        public string Password { get; set; }

        static SuperUser instance;

        private SuperUser(string id, string pwd)
        {
            UserID = id;
            Password = pwd;
        }
        public static SuperUser GetOrCreate(string id, string pwd)
        {
            if (instance == null)
            {
                instance = new SuperUser(id, pwd);
            }

            return instance;
        }
    }
}
