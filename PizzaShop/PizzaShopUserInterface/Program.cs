using Microsoft.EntityFrameworkCore;
using PizzaShop.DataAccess;
using System;
using System.Collections.Generic;

namespace PizzaShopUserInterface
{
    class Program
    {
        static DbContextOptions<ProjectsContext> options = null;

        static void Main(string[] args)
        {
            var connectionString = SecretConfiguration.ConnectionString;

            var optionsBuilder = new DbContextOptionsBuilder<ProjectsContext>();
            optionsBuilder.UseSqlServer(connectionString);
            options = optionsBuilder.Options;
            bool done = false;
            string input;
            List<LocationClass> LocationList = new List<LocationClass>
                {
                new LocationClass("This Location", new List<OrderClass>()), new LocationClass("That Location", new List<OrderClass>()), new LocationClass("The Other Location", new List<OrderClass>())
                };
            Console.WriteLine("Enter First Name:");
            string inputFirstName = Console.ReadLine();
            Console.WriteLine("Enter Last Name:");
            string inputLastName = Console.ReadLine();
            UserClass user = new UserClass(inputFirstName, inputLastName, LocationList[0]);
            do
            {

                user.GetLocation(LocationList);
                user.location.TakeOrder(user);
                Console.WriteLine("Done?(y/n)");
                input = Console.ReadLine();
                if (input.ToLower() == "y" || input.ToLower() == "yes")
                    done = true;
            } while (!done);
            List < OrderClass > list = user.location.OrderHistory;
            foreach(OrderClass order in list)
            {
                foreach(PizzaClass pizza in order.pizzas)
                {
                    Console.WriteLine($"{pizza.ToString()}");
                }
                Console.WriteLine($"Total: ${order.total}");
            }
        }
    }
}
