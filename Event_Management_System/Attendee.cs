using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management_System
{
    internal class Attendee
    {
        public int ID {  get; set; }
        public string Name { get; set; }
        public string Email { get; set; }



        public Attendee(int id, string name, string email) 
        {
            ID = id;
            Name = name;
            Email = email;
        }

        public override string ToString()
        {
            return $"Attendee:{Name}, Email:{Email}";
        }
        
    }


}
