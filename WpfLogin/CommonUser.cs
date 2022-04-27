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

        protected Guid uniqueID;
        protected string passwordHistoryFile;
        protected List<string> passwordHistory;

        public CommonUser(string id, string pwd)
        {
            this.UserID = id;
            this.Password = pwd;

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

            if (string.IsNullOrEmpty(pwd) == false)
            {
                passwordHistory.Add(pwd);
                File.WriteAllLines(passwordHistoryFile, passwordHistory.ToArray());
            }
        }

        public void AddPasswordHistory()
        {
            if (File.Exists(passwordHistoryFile))
            {
                try
                {
                    File.AppendText("\n" + Password);
                }
                catch (IOException e)
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
