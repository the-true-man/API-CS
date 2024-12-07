using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace registration
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Midname { get; set; } = "Нет отчества";
        public string Birthday { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        private static int _uniqID = 1;
        public bool isBanned = false;
        public DateTime TimeBan { get; set; } = DateTime.Now;
        public int failTry = 0;
        public string address = "";

        public string urlPhone = "";


        public User(string name, string lastname, string middlename, string birthday, string email, string password)
        {
            Id = _uniqID;
            _uniqID++;
            Name = name;
            Lastname = lastname;
            Midname = middlename;
            if (middlename == "")
            {
                Midname = "Нет отчества";
            }
            this.Birthday = birthday;
            this.Email = email;
            this.Password = password;

        }

    }
}
