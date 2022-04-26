using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System.Diagnostics;

namespace WpfLogin
{
    public class ValidatorWithHistory : IValidator
    {
        readonly List<string> passwordHistory;
        readonly IValidator inputValidator;
        string historyFilePath;

        public ValidatorWithHistory(string historyFilePath, IValidator inputValidator)
        {
            try
            {
                string[] pwdHistory = File.ReadAllLines(historyFilePath);
                passwordHistory = new List<string>(pwdHistory);
                this.historyFilePath = historyFilePath;
            }
            catch (IOException)
            {
                Debug.WriteLine($"Soubor s historií hesel {historyFilePath} nenalezen");
                throw;
            }
            this.inputValidator = inputValidator;
        }

        public bool IsPasswordValid(string password)
        {
            return inputValidator.IsPasswordValid(password) && passwordHistory.Contains(password) == false;   
        }
    }
}
