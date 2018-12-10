using Microsoft.EntityFrameworkCore;
using PizzaShop.Library;
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
            IPizzaShopRepo repo;

            using (var db = new ProjectsContext(options))
            {
                repo = new PizzaShopRepo(db);
                IList<LocationClass> LocationList = repo.GetAllLocations();
                foreach (var location in LocationList)
                {
                    repo.BuildLocationOrderHistory(location);
                }
                /*List<LocationClass> LocationList = new List<LocationClass>
                {
                new LocationClass("This Location", new List<OrderClass>()), new LocationClass("That Location", new List<OrderClass>()), new LocationClass("The Other Location", new List<OrderClass>())
                };*/
                UserClass user;
                Console.WriteLine("Enter First Name:");
                string inputFirstName = Console.ReadLine();
                if (inputFirstName.ToLower() == "admin")
                {
                    Console.WriteLine("Password:");
                    string password = Console.ReadLine();
                    if (password == "password123")
                    {
                        do
                        {
                            Console.Write("What Admin fuction do you wish to use?\n1. Open New Location\n2. Restock Location Inventory\n3. Show Order History for Location\n");
                            input = Console.ReadLine();
                            if (int.TryParse(input, out int number))
                            {
                                switch (number)
                                {
                                    case 1:
                                        OpenNewLocation(repo);
                                        LocationList = repo.GetAllLocations();
                                        foreach (var location in LocationList)
                                        {
                                            repo.BuildLocationOrderHistory(location);
                                        }
                                        break;
                                    case 2:
                                        RestockInventory(repo, LocationList);
                                        break;
                                    case 3:
                                        ShowLocationHistory(LocationList);
                                        break;
                                }
                            }
                            else
                            {
                                Console.WriteLine("Invalid Input");
                            }
                            Console.WriteLine("Done?(y/n)");
                            input = Console.ReadLine();
                            if (input.ToLower() == "y" || input.ToLower() == "yes")
                                done = true;
                            } while (!done);
                    }
                }
                else
                {
                    Console.WriteLine("Enter Last Name:");
                    string inputLastName = Console.ReadLine();
                    if (repo.UserIsInDB(inputFirstName, inputLastName))
                        user = repo.GetUserByName(inputFirstName, inputLastName);
                    else
                    {
                        user = new UserClass(0, inputFirstName, inputLastName, LocationList[0]);
                        repo.AddNewUser(user);
                        repo.SaveChanges();
                    }
                    do
                    {

                        user.GetLocation(LocationList);
                        var order = user.location.TakeOrder(user);
                        if (order != null)
                        {
                            repo.CreateOrder(order);
                            repo.SaveChanges();
                        }
                        Console.WriteLine("Done?(y/n)");
                        input = Console.ReadLine();
                        if (input.ToLower() == "y" || input.ToLower() == "yes")
                            done = true;
                    } while (!done);
                }
                //List<OrderClass> list = user.location.OrderHistory;
                //foreach (OrderClass order in list)
                //{
                //    foreach (PizzaClass pizza in order.pizzas)
                //    {
                //        Console.WriteLine($"{pizza.ToString()}");
                //    }
                //    Console.WriteLine($"Total: ${order.total}");
                //}
            }
        }

        public static void OpenNewLocation(IPizzaShopRepo repo)
        {
            Console.WriteLine("New Location Description:");
            string description = Console.ReadLine();
            LocationClass newLocation = new LocationClass(description, new List<OrderClass>());
            repo.AddNewLocation(newLocation);
            repo.SaveChanges();
        }

        public static void RestockInventory(IPizzaShopRepo repo, IList<LocationClass> list)
        {
            Console.WriteLine("Which Location Would you like to restock?");
            for(int i = 0; i < list.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {list[i].LocationDescription}");
            }
            string input = Console.ReadLine();
            if(int.TryParse(input, out int number) && number > 0 && number <= list.Count)
            {
                list[number - 1].RestockInventory();
            }
        }

        public static void ShowLocationHistory(IList<LocationClass> list)
        {
            Console.WriteLine("Which Location Would you like to view Order History for?");
            for (int i = 0; i < list.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {list[i].LocationDescription}");
            }
            string input = Console.ReadLine();
            if (int.TryParse(input, out int number) && number > 0 && number <= list.Count)
            {
                list[number - 1].ShowOrderHistory();
            }
        }
    }
}
