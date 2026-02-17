using Event_Management_System;
using System;
using System.Collections.Generic;

namespace Program
{
    public class Program
    {
        private static List<Event> events = new List<Event>();
        public static void Main(string[] args)
        {
            // Fix CS7036: Provide all required parameters for Event constructor
            // Fix IDE0090: Use object initializer for List<Attendee>
            Event event1 = new Event(
                1,
                "Conference",
                "Main Hall",
                new DateTime(2024, 10, 15) 
            );

            events.Add(event1);
            Console.WriteLine(event1);
            Console.WriteLine(events.Count);
            

            Attendee attendee1 = new Attendee(001, "John", "John@gmal.com");
            Console.WriteLine("Attendees");
            Console.WriteLine(attendee1);
        }
    }
}
