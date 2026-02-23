//csharp Event_Management_System_Test\UnitTest1.cs
using System.Reflection;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using Xunit;
using Program;


namespace Event_Management_System_Test
{
    public class ProgramTests
    {
        private readonly Type _programType = typeof(Program.Program);
        private readonly Assembly _appAssembly;
        private readonly Type _eventType;
        private readonly Type _attendeeType;
        private readonly FieldInfo _eventsField;

        public ProgramTests()
        {
            _appAssembly = _programType.Assembly;
            _eventType = _appAssembly.GetType("Event_Management_System.Event", throwOnError: true)!;
            _attendeeType = _appAssembly.GetType("Event_Management_System.Attendee", throwOnError: true)!;
            _eventsField = _programType.GetField("events", BindingFlags.NonPublic | BindingFlags.Static)!
;
        }

        // Helper: create Event instance
        private object CreateEvent(int id, string name, string location, DateTime date)
        {
            return Activator.CreateInstance(_eventType, new object[] { id, name, location, date }, true)!;
        }

        // Helper: create Attendee instance
        private object CreateAttendee(int id, string name, string email)
        {
            return Activator.CreateInstance(_attendeeType, new object[] { id, name, email }, true)!;
        }

        // Helper: set Program.events to a fresh List<Event> containing provided event objects
        private object PrepareEventsList(params object[] events)
        {
            var listType = typeof(System.Collections.Generic.List<>).MakeGenericType(_eventType);
            var list = Activator.CreateInstance(listType)!;
            var add = listType.GetMethod("Add")!;
            foreach (var ev in events)
            {
                add.Invoke(list, new[] { ev });
            }

            _eventsField.SetValue(null, list);
            return list;
        }

        // Helper: get attendees IList for an event (returns non-generic IList)
        private IList GetAttendeesList(object ev)
        {
            var getAtt = _eventType.GetMethod("GetAttendees", BindingFlags.Public | BindingFlags.Instance)!;
            var attendeesObj = getAtt.Invoke(ev, null)!;
            return (IList)attendeesObj;
        }

        [Fact]
        public void ListAllEvent_PrintsEventNames()
        {
            var ev1 = CreateEvent(1010, "Tech Conference", "NY", new DateTime(2026, 10, 11));
            PrepareEventsList(ev1);

            var sw = new StringWriter();
            var originalOut = Console.Out;
            try
            {
                Console.SetOut(sw);
                // call method
                var mi = _programType.GetMethod("ListAllEvent", BindingFlags.Public | BindingFlags.Static)!;
                mi.Invoke(null, null);

                var outStr = sw.ToString();
                Assert.Contains("Tech Conference", outStr);
            }
            finally
            {
                Console.SetOut(originalOut);
            }
        }

        [Fact]
        public void ListIndivudualEvent_ShowsEventAndAttendees()
        {
            var ev1 = CreateEvent(2, "Art Expo", "London", new DateTime(2026, 10, 11));
            var attendee = CreateAttendee(11, "Alice", "a@x.com");
            // add attendee via Event.AddAttendee
            var addMethod = _eventType.GetMethod("AddAttendee", BindingFlags.Public | BindingFlags.Instance)!;
            addMethod.Invoke(ev1, new[] { attendee });

            PrepareEventsList(ev1);

            var input = new StringReader("2\n");
            var sw = new StringWriter();
            var originalIn = Console.In;
            var originalOut = Console.Out;
            try
            {
                Console.SetIn(input);
                Console.SetOut(sw);

                var mi = _programType.GetMethod("ListIndivudualEvent", BindingFlags.Public | BindingFlags.Static)!;
                mi.Invoke(null, null);

                var output = sw.ToString();
                Assert.Contains("Art Expo", output);
                Assert.Contains("Attendees:", output);
                Assert.Contains("Alice", output);
            }
            finally
            {
                Console.SetIn(originalIn);
                Console.SetOut(originalOut);
            }
        }

        [Fact]
        public void EditAnEvent_UpdatesProperties()
        {
            var ev1 = CreateEvent(3, "Old Name", "Old Loc", new DateTime(2026, 1, 1));
            PrepareEventsList(ev1);

            // provide inputs: eventId, newName, newLocation, newDate
            var input = new StringReader("3\nNew Name\nNew Loc\n2026-12-31\n");
            var sw = new StringWriter();
            var originalIn = Console.In;
            var originalOut = Console.Out;
            try
            {
                Console.SetIn(input);
                Console.SetOut(sw);

                var mi = _programType.GetMethod("EditAnEvent", BindingFlags.Public | BindingFlags.Static)!;
                mi.Invoke(null, null);

                // verify via reflection: Name, Location, Date
                var nameProp = _eventType.GetProperty("Name")!;
                var locProp = _eventType.GetProperty("Location")!;
                var dateProp = _eventType.GetProperty("Date")!;

                Assert.Equal("New Name", nameProp.GetValue(ev1));
                Assert.Equal("New Loc", locProp.GetValue(ev1));
                Assert.Equal(new DateTime(2026, 12, 31), dateProp.GetValue(ev1));
            }
            finally
            {
                Console.SetIn(originalIn);
                Console.SetOut(originalOut);
            }
        }

        [Fact]
        public void DeleteAnEvent_RemovesEventWhenConfirmed()
        {
            var ev1 = CreateEvent(4, "ToDelete", "Loc", DateTime.Today);
            PrepareEventsList(ev1);

            var input = new StringReader("4\nY\n");
            var sw = new StringWriter();
            var originalIn = Console.In;
            var originalOut = Console.Out;
            try
            {
                Console.SetIn(input);
                Console.SetOut(sw);

                var mi = _programType.GetMethod("DeleteAnEvent", BindingFlags.Public | BindingFlags.Static)!;
                mi.Invoke(null, null);

                // check Program.events count == 0
                var list = _eventsField.GetValue(null) as IList;
                Assert.NotNull(list);
                Assert.Empty(list!);
            }
            finally
            {
                Console.SetIn(originalIn);
                Console.SetOut(originalOut);
            }
        }

        [Fact]
        public void ListAttendeesOfAnEvent_PrintsAttendeeList()
        {
            var ev1 = CreateEvent(5, "WithAtt", "Loc", DateTime.Today);
            var att = CreateAttendee(21, "Bob", "bob@x.com");
            var addMethod = _eventType.GetMethod("AddAttendee", BindingFlags.Public | BindingFlags.Instance)!;
            addMethod.Invoke(ev1, new[] { att });

            PrepareEventsList(ev1);

            var input = new StringReader("5\n");
            var sw = new StringWriter();
            var originalIn = Console.In;
            var originalOut = Console.Out;
            try
            {
                Console.SetIn(input);
                Console.SetOut(sw);

                var mi = _programType.GetMethod("ListAttendeesOfAnEvent", BindingFlags.Public | BindingFlags.Static)!;
                mi.Invoke(null, null);

                var output = sw.ToString();
                Assert.Contains("Bob", output);
                Assert.Contains("Attendees for", output);
            }
            finally
            {
                Console.SetIn(originalIn);
                Console.SetOut(originalOut);
            }
        }

        [Fact]
        public void AddAttendeetoEvent_AddsAttendeeToEvent()
        {
            var ev1 = CreateEvent(6, "AddTo", "Loc", DateTime.Today);
            PrepareEventsList(ev1);

            // inputs: eventId, attendeeId, name, email
            var input = new StringReader("6\n101\nCharlie\ncharlie@x.com\n");
            var sw = new StringWriter();
            var originalIn = Console.In;
            var originalOut = Console.Out;
            try
            {
                Console.SetIn(input);
                Console.SetOut(sw);

                var mi = _programType.GetMethod("AddAttendeetoEvent", BindingFlags.Public | BindingFlags.Static)!;
                mi.Invoke(null, null);

                // verify attendee added
                var attendees = GetAttendeesList(ev1);
                Assert.Single(attendees);

                var attObj = attendees[0];
                var idProp = _attendeeType.GetProperty("ID")!;
                var nameProp = _attendeeType.GetProperty("Name")!;
                var emailProp = _attendeeType.GetProperty("Email")!;

                Assert.Equal(101, idProp.GetValue(attObj));
                Assert.Equal("Charlie", nameProp.GetValue(attObj));
                Assert.Equal("charlie@x.com", emailProp.GetValue(attObj));
            }
            finally
            {
                Console.SetIn(originalIn);
                Console.SetOut(originalOut);
            }
        }

        [Fact]
        public void DeleteAttendeeFromEvent_RemovesSpecifiedAttendee()
        {
            var ev1 = CreateEvent(7, "RemoveFrom", "Loc", DateTime.Today);
            var att1 = CreateAttendee(201, "Dana", "dana@x.com");
            var addMethod = _eventType.GetMethod("AddAttendee", BindingFlags.Public | BindingFlags.Instance)!;
            addMethod.Invoke(ev1, new[] { att1 });

            PrepareEventsList(ev1);

            // inputs: eventId, attendeeId
            var input = new StringReader("7\n201\n");
            var sw = new StringWriter();
            var originalIn = Console.In;
            var originalOut = Console.Out;
            try
            {
                Console.SetIn(input);
                Console.SetOut(sw);

                var mi = _programType.GetMethod("DeleteAttendeeFromEvent", BindingFlags.Public | BindingFlags.Static)!;
                mi.Invoke(null, null);

                var attendees = GetAttendeesList(ev1);
                Assert.Empty(attendees);
            }
            finally
            {
                Console.SetIn(originalIn);
                Console.SetOut(originalOut);
            }
        }
    }
}