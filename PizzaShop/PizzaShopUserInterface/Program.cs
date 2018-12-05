using System;
using System.Collections.Generic;

namespace PizzaShopUserInterface
{
    class Program
    {
        static void Main(string[] args)
        {
            bool done = false;
            string input;
            List<Location> LocationList = new List<Location>
                {
                new Location("This Location", new List<Order>()), new Location("That Location", new List<Order>()), new Location("The Other Location", new List<Order>())
                };
            Console.WriteLine("Enter First Name:");
            string inputFirstName = Console.ReadLine();
            Console.WriteLine("Enter Last Name:");
            string inputLastName = Console.ReadLine();
            User user = new User(inputFirstName, inputLastName, LocationList[0]);
            do
            {

                user.GetLocation(LocationList);
                user.location.TakeOrder(user);
                Console.WriteLine("Done?(y/n)");
                input = Console.ReadLine();
                if (input.ToLower() == "y" || input.ToLower() == "yes")
                    done = true;
            } while (!done);
            List < Order > list = user.location.OrderHistory;
            foreach(Order order in list)
            {
                foreach(Pizza pizza in order.pizzas)
                {
                    Console.WriteLine($"{pizza.ToString()}");
                }
                Console.WriteLine($"Total: ${order.total}");
            }
        }
    }
}
