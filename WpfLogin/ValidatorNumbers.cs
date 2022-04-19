using System.Text.RegularExpressions;

namespace WpfLogin
{
    public class ValidatorNumbers : IValidator
    {
        public bool IsPasswordValid(string password)
        {
            return password.Length > 7 && Regex.IsMatch(password, "w*\\dw*");
        }
    }
}
