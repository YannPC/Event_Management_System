// See https://aka.ms/new-console-template for more information
Console.WriteLine("Please choose of the following Events");

Console.WriteLine("1.List All Events:");
Console.WriteLine("2.List an individual event:");
Console.WriteLine("3.Edit an event:");
Console.WriteLine("4.Delete an event:");
Console.WriteLine("5.List the attendees attending an event:");

string selectAnEvent = Console.ReadLine();

// convert selectAnEvent to int for comparison
int selectedOption;
if (int.TryParse(selectAnEvent, out selectedOption))
{
    while (selectedOption == 1)
    {
        selectAnEvent = Console.ReadLine();
        selectAnEvent += "\n";
        int.TryParse(selectAnEvent, out selectedOption);
    }
}