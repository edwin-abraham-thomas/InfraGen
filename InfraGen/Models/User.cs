using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraGen.Models
{
    public class User
    {
        public User()
        {
            Info = new Info();
            Credentials = new Credentials();
        }


        public Info Info { get; set; }
        public Credentials Credentials { get; set; }
    }

    public class Info
    {
        public string Name { get; set; }
    }

    public class Credentials
    {
        public string AdoPAT { get; set; }
    }
}
