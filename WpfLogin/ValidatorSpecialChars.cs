using System.Text.RegularExpressions;

namespace WpfLogin
{
    public class ValidatorSpecialChars : IValidator
    {
        public bool IsPasswordValid(string password)
        {
            return password.Length > 7 && Regex.IsMatch(password, "w*\\Ww*");
        }
    }
}
