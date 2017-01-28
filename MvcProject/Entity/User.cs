using System;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcProject.Entity
{
    class User
    {
        public string Id { get; set; }
        public string Login {get; set;}
        public string Password {get; set;}
        public string Firstname { get; set;}
        public string Lastname { get; set; }
        public string Position { get; set; }

        public void Hydrator(string[] values)
        {
            this.Id = values[0];
            this.Login = values[1];
            this.Password = values[2];
            this.Firstname = values[3];
            this.Lastname = values[4];
            this.Position = values[5];
        }
        public String[] Dehydrator()
        {
            string[] serialization = 
            {
            this.Id,
            this.Login,
            this.Password,
            this.Firstname,
            this.Lastname,
            this.Position
            };

            return serialization;
        }
    }
}
