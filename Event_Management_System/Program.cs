using Event_Management_System;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace Program
{
    public class Program
    {
        private static List<Event> events = new List<Event>();
        public static void Main(string[] args)
        {
            Event event1 = new Event(1010, "Tech Conference", "New York", new DateTime(2026, 10, 11));
            Event event2 = new Event(1, "Music Festival", "Paris", new DateTime(2026, 10, 11));


            events.Add(event1);
            //events.Add(event2);

            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("*****************************");
                Console.WriteLine("   EVENT MANAGEMENT SYSTEM ");
                Console.WriteLine("*****************************");
                Console.WriteLine("1. List all Events");
                Console.WriteLine("2. List an Indivudual Event");
                Console.WriteLine("3. Edit an Event");
                Console.WriteLine("4. Delete an Event");
                Console.WriteLine("5. List Attendee to an Event");
                Console.WriteLine("6. Add Attendee to an Event");
                Console.WriteLine("7. Delete Attendee from an Event");
                Console.WriteLine("8. Exit");

                Console.WriteLine("Enter your Choice");

                string choice = Console.ReadLine()!;
                Console.WriteLine();

                switch (choice)

                {
                    case "1": ListAllEvent();
                        
                        break;
                    case "2": ListIndivudualEvent();
                        
                        break;
                    case "3":EditAnEvent();
                      
                        break;
                    case "4":DeleteAnEvent();
                        break;
                    case "5":ListAttendeesOfAnEvent();
                        break;
                    case "6":AddAttendeetoEvent();
                        break;
                    case "7":
                        DeleteAttendeeFromEvent();
                        break;
                    case "8"
:
                        exit = true;
                        break;

                    default:
                        Console.WriteLine("Invalid Choise. Please try again. \n");
                        break;

                }

            }
        }

        public static void ListAllEvent()
        {
            if (events.Count ==0)
            {
                Console.WriteLine("No event found");
                return;
            } foreach (var ev in events)
            {
                Console.WriteLine(ev);
            }

            Console.WriteLine();

        }

        public static void ListIndivudualEvent() 
        {
            Console.WriteLine("Enter Event ID: ");

            if (int.TryParse(Console.ReadLine(), out int eventId))
            {
                var ev = events.Find(e => e.Id == eventId);
                if (ev != null) {

                    Console.WriteLine(ev);
                    var attendees = ev.GetAttendees();
                    if(attendees.Count > 0)
                    {
                        Console.WriteLine("Attendees:");
                        foreach (var att in attendees)
                        {
                            Console.WriteLine(att);
                        }
                    } else
                    {
                        Console.WriteLine("No attendees for this event");
                    }

                } else
                {
                    Console.WriteLine("Event not found");
                }
            }
            else
            {
                Console.WriteLine("Invalid Event ID.");
            }
            Console.WriteLine() ;

        }

        public static void EditAnEvent() { }
        
        public static void DeleteAnEvent() { }

        public static void ListAttendeesOfAnEvent() { }

        public static void AddAttendeetoEvent() { }

        public static void DeleteAttendeeFromEvent() { }
    }
}
