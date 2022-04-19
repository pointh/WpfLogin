namespace WpfLogin
{
    public class SuperUser
    {
        public string UserID { get; set; }
        public string Password { get; set; }

        private static SuperUser instance;

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
