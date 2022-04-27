using System;

namespace WpfLogin
{
    public class SuperUser : CommonUser
    {
        private static SuperUser instance;

        private SuperUser(string id, string pwd) : base(id, pwd) { }
        public static SuperUser GetOrCreate(string id, string pwd)
        {
            if (instance == null)
            {
                instance = new SuperUser(id, pwd);
            }

            return instance;
        }

        public static SuperUser GetOrCreate(CommonUser cu)
        {
            if (instance == null)
            {
                instance = new SuperUser(cu.UserID, cu.Password);
                instance.AddPasswordHistory();
            }

            return instance;
        }

        public static void Logout()
        {
            instance = null;
        }

        internal static void ChangePasswordTo(string text)
        {
            if (instance is not null)
            {
                instance.Password = text;
            }
        }
    }
}
