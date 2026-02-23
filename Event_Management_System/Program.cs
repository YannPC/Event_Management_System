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
            Event event3 = new Event(2, "Art Exhibition", "London", new DateTime(2026, 10, 11));


            events.Add(event1);
            events.Add(event2);

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
                        Console.WriteLine("Invalid Choice. Please try again. \n");
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

        public static void EditAnEvent()
        {
            Console.Write("Enter Event ID to edit: ");
            if (!int.TryParse(Console.ReadLine(), out int eventId))
            {
                Console.WriteLine("Invalid Event ID.");
                return;
            }

            var ev = events.Find(e => e.Id == eventId);
            if (ev == null)
            {
                Console.WriteLine("Event not found.");
                return;
            }

            Console.WriteLine($"Editing event: {ev.Name} (ID: {ev.Id})");
            Console.Write("New name (leave empty to keep current): ");
            var input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input))
                ev.Name = input.Trim();

            Console.Write("New location (leave empty to keep current): ");
            input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input))
                ev.Location = input.Trim();

            Console.Write($"New date (yyyy-MM-dd) (current: {ev.Date:yyyy-MM-dd}) (leave empty to keep current): ");
            input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input) && DateTime.TryParse(input.Trim(), out var newDate))
                ev.Date = newDate;

            Console.WriteLine("Event updated.");
            Console.WriteLine();
        }
        public static void DeleteAnEvent()
        {
            Console.Write("Enter Event ID to delete: ");
            if (!int.TryParse(Console.ReadLine(), out int eventId))
            {
                Console.WriteLine("Invalid Event ID.");
                return;
            }

            var ev = events.Find(e => e.Id == eventId);
            if (ev == null)
            {
                Console.WriteLine("Event not found.");
                return;
            }

            Console.Write($"Are you sure you want to delete '{ev.Name}' (ID: {ev.Id})? (Y/N): ");
            var confirm = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(confirm) && (confirm.Equals("Y", StringComparison.OrdinalIgnoreCase) || confirm.Equals("yes", StringComparison.OrdinalIgnoreCase)))
            {
                events.Remove(ev);
                Console.WriteLine("Event deleted.");
            }
            else
            {
                Console.WriteLine("Delete cancelled.");
            }

            Console.WriteLine();
        }

        public static void ListAttendeesOfAnEvent()
        {
            Console.Write("Enter Event ID to list attendees: ");
            if (!int.TryParse(Console.ReadLine(), out int eventId))
            {
                Console.WriteLine("Invalid Event ID.");
                return;
            }

            var ev = events.Find(e => e.Id == eventId);
            if (ev == null)
            {
                Console.WriteLine("Event not found.");
                return;
            }

            var attendees = ev.GetAttendees();
            if (attendees.Count == 0)
            {
                Console.WriteLine("No attendees for this event.");
            }
            else
            {
                Console.WriteLine($"Attendees for '{ev.Name}':");
                foreach (var a in attendees)
                {
                    Console.WriteLine(a);
                }
            }

            Console.WriteLine();
        }

        public static void AddAttendeetoEvent()
        {
            Console.Write("Enter Event ID to add attendee to: ");
            if (!int.TryParse(Console.ReadLine(), out int eventId))
            {
                Console.WriteLine("Invalid Event ID.");
                return;
            }

            var ev = events.Find(e => e.Id == eventId);
            if (ev == null)
            {
                Console.WriteLine("Event not found.");
                return;
            }

            Console.Write("Attendee ID: ");
            if (!int.TryParse(Console.ReadLine(), out int attendeeId))
            {
                Console.WriteLine("Invalid Attendee ID.");
                return;
            }

            Console.Write("Attendee name: ");
            var name = Console.ReadLine() ?? string.Empty;

            Console.Write("Attendee email: ");
            var email = Console.ReadLine() ?? string.Empty;

            var attendee = new Attendee(attendeeId, name, email);
            ev.AddAttendee(attendee);

            Console.WriteLine("Attendee added.");
            Console.WriteLine();
        }

        public static void DeleteAttendeeFromEvent()
        {
            Console.Write("Enter Event ID to remove attendee from: ");
            if (!int.TryParse(Console.ReadLine(), out int eventId))
            {
                Console.WriteLine("Invalid Event ID.");
                return;
            }

            var ev = events.Find(e => e.Id == eventId);
            if (ev == null)
            {
                Console.WriteLine("Event not found.");
                return;
            }

            var attendees = ev.GetAttendees();
            if (attendees.Count == 0)
            {
                Console.WriteLine("No attendees to remove.");
                return;
            }

            Console.WriteLine("Attendees:");
            foreach (var a in attendees)
            {
                Console.WriteLine($"ID: {a.ID} - {a.Name} ({a.Email})");
            }

            Console.Write("Enter Attendee ID to remove: ");
            if (!int.TryParse(Console.ReadLine(), out int attendeeId))
            {
                Console.WriteLine("Invalid Attendee ID.");
                return;
            }

            var attendee = attendees.Find(a => a.ID == attendeeId);
            if (attendee == null)
            {
                Console.WriteLine("Attendee not found.");
                return;
            }

            ev.RemoveAttendee(attendee);
            Console.WriteLine("Attendee removed.");
            Console.WriteLine();
        }

    }
}
