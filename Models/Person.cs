using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace registration
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string lastName { get; set; }
        public string middleName { get; set; } = "Нет отчества";
        public string birthday { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        private static int _uniqID = 1;
        public bool isBanned = false;
        public DateTime timeBan { get; set; } = DateTime.Now;
        public int failTry = 0;
        public Person(string name, string lastname, string middlename, string birthday, string email, string password)
        {
            Id = _uniqID;
            _uniqID++;
            Name = name;
            lastName = lastname;
            middleName = middlename;
            if (middlename == "")
            {
                middleName = "Нет отчества";
            }
            this.birthday = birthday;
            this.email = email;
            this.password = password;

        }

    }
}
