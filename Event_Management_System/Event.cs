using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management_System
{
    internal class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location{ get; set; }
        public DateTime Date { get; set; }

        List<Attendee> attendees = new List<Attendee>();

        public Event (int eventId, string eventName, string eventLocation, DateTime eventDate)
        {
            Id = eventId;
            Name = eventName;
            Location = eventLocation;
            Date = eventDate;
        }

        public override string ToString()
        {
            return $"Event:{Name}, Date: {Date.ToShortTimeString()},Location: {Location}, Attendee: {attendees.Count}";
        }

        public  void AddAttendee(Attendee attendee)
        {
            attendees.Add(attendee);
        }

        public void RemoveAttendee(Attendee attendee)
        {
            attendees.Remove(attendee);
        }

        public List<Attendee> GetAttendees()
        {
            return attendees;
        }

    }
}
