using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace WpfLogin
{
    public enum LoginError { OK, RepeatedPassword, PasswordRuleBreak };
    public class CommonUser
    {
        public string UserID { get; set; }
        public string Password { get; set; }
        public bool LoggedIn { get; set; }

        string passwordHistoryFile;
        List<string> passwordHistory;

        public CommonUser(string id, string pwd)
        {
            UserID = id;
            Password = pwd;
            LoggedIn = false;

            SHA256 sha = SHA256.Create();
            byte[] shaB = sha.ComputeHash(Encoding.GetEncoding("UTF-8").GetBytes(id));
            passwordHistoryFile = Convert.ToBase64String(shaB);

            if (File.Exists(passwordHistoryFile))
            {
                try
                {
                    passwordHistory = new List<string>(File.ReadLines(passwordHistoryFile));
                }
                catch (IOException e)
                {
                    Console.WriteLine(
                        $"Error in {MethodBase.GetCurrentMethod()}:" + 
                        " Password history file cannot be open.");
                    throw;
                }
            }
            else
            {
                passwordHistory = new List<string>();
            }

            if (string.IsNullOrEmpty(Password) == false)
            {
                AddPasswordToHistory();
            }
        }

        public bool Login(string openPassword)
        {
            if (passwordHistory.Count > 0)
            {
                string currentHashedPassword = passwordHistory[passwordHistory.Count - 1];
                if (HashString(openPassword) == currentHashedPassword)
                {
                    LoggedIn = true;
                    return true;
                }
            }

            LoggedIn = false;
            return false;
        }

        string HashString(string s)
        {
            SHA256 sha = SHA256.Create();
            byte[] shaB = sha.ComputeHash(Encoding.GetEncoding("UTF-8").GetBytes(s));
            return Convert.ToBase64String(shaB);
        }

        public void AddPasswordToHistory()
        {
            if (File.Exists(passwordHistoryFile))
            {
                try
                {
                    passwordHistory.Add(HashString(Password));
                    File.WriteAllLines(passwordHistoryFile, passwordHistory.ToArray());
                }
                catch (IOException)
                {
                    Console.WriteLine(
                        $"Error in {MethodBase.GetCurrentMethod()}:" +
                        " Password cannot be added to password history file.");
                    throw;
                }
            }
        }

        public virtual LoginError ValidatePassword(List<IValidator> validators)
        {
            foreach (var v in validators)
            {
                if (v.IsPasswordValid(Password) == false)
                {
                    return LoginError.PasswordRuleBreak;
                }
            }

            if (passwordHistory.Contains(Password))
            {
                return LoginError.RepeatedPassword;
            }

            return LoginError.OK;
        }
    }
}
