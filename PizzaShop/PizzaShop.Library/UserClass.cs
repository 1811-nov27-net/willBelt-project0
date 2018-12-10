using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaShop.Library
{
    public class UserClass
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public LocationClass DefaultLocation;
        public LocationClass location;
        public int UserID { get; set; }

        public UserClass(int userID, string first, string last, LocationClass defaultLocation)
        {
            UserID = userID;
            FirstName = first;
            LastName = last;
            DefaultLocation = defaultLocation;
        }

        public void GetLocation(IList<LocationClass> locations)
        {
            bool isValidInput = false;
            string locationsString = "";
            string input;
            for (int i = 0; i < locations.Count; i++)
            {
                locationsString += $"{i+1}. {locations[i].LocationDescription}\n";
            }
            do
            {
                Console.Write($"Your Default Location is: {DefaultLocation.LocationDescription}. Is this Correct?(y/n)\n");
                input = Console.ReadLine();
                if (input.ToLower() == "y" || input.ToLower() == "yes")
                {
                    location = DefaultLocation;
                    isValidInput = true;
                }
                else if (input.ToLower() == "n" || input.ToLower() == "no")
                {
                    Console.Write($"Which location do you wish to order from?\n{locationsString}");
                    input = Console.ReadLine();
                    isValidInput = (int.TryParse(input, out int number) && number - 1 > -1 && number - 1 < locations.Count);
                    if (isValidInput)
                    {
                        location = locations[number - 1];
                        Console.WriteLine("Would you like to set this as your default location? (y/n)");
                        input = Console.ReadLine();
                        if (input.ToLower() == "y" || input.ToLower() == "yes")
                            DefaultLocation = location;
                    }
                    else
                        Console.WriteLine("Invalid entry, please enter the number of your selection");
                }
                else
                    Console.WriteLine("Invalid entry, please enter the number of your selection");
            } while (!isValidInput);
        }
    }
}
