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
            //setup database connection options
            var connectionString = SecretConfiguration.ConnectionString;
            var optionsBuilder = new DbContextOptionsBuilder<ProjectsContext>();
            optionsBuilder.UseSqlServer(connectionString);
            options = optionsBuilder.Options;
            //declare some necessary variables
            bool done = false;
            string input;
            IPizzaShopRepo repo;
            //create dbcontext object
            using (var db = new ProjectsContext(options))
            {
                //instantiate repo
                repo = new PizzaShopRepo(db);
                //initialize locations
                IList<LocationClass> LocationList = repo.GetAllLocations();
                //declare user
                UserClass user;
                //ask ser for first name
                Console.WriteLine("Enter First Name:");
                string inputFirstName = Console.ReadLine();
                //if user inputs admin, ask for password and go to admin functions
                if (inputFirstName.ToLower() == "admin")
                {
                    Console.WriteLine("Password:");
                    string password = Console.ReadLine();
                    if (password == "password123")
                    {
                        do
                        {
                            //ask user what admin functions they want
                            Console.Write("What Admin fuction do you wish to use?\n1. Open New Location\n2. Restock Location Inventory\n3. Show Order History for Location\n4. Add Topping to Location Menu\n5. Remove Topping from Location Menu\n");
                            input = Console.ReadLine();
                            //parse user input
                            if (int.TryParse(input, out int number) && number > 0 && number < 6)
                            {
                                switch (number)
                                {
                                    case 1:
                                        //call OpenNewLocation method
                                        OpenNewLocation(repo);
                                        //refresh location list
                                        LocationList = repo.GetAllLocations();
                                        break;
                                    case 2:
                                        //call RestockInventory method
                                        RestockInventory(repo, LocationList);
                                        break;
                                    case 3:
                                        //Call ShowLocationHistory method
                                        ShowLocationHistory(LocationList);
                                        break;
                                    case 4:
                                        //call AddToppingToLocation method
                                        AddToppingToLocationMenu(repo, LocationList);
                                        break;
                                    case 5:
                                        //call RemoveToppingFromLocation method
                                        RemoveToppingFromLocationMenu(repo, LocationList);
                                        break;
                                }
                            }
                            else
                            {
                                //inform user their input was invalid
                                Console.WriteLine("Invalid Input");
                            }
                            //ask user if they are done
                            Console.WriteLine("Done?(y/n)");
                            input = Console.ReadLine();
                            if (input.ToLower() == "y" || input.ToLower() == "yes")
                                done = true;
                            } while (!done);
                    }
                }
                else
                {
                    //ask user for last name
                    Console.WriteLine("Enter Last Name:");
                    string inputLastName = Console.ReadLine();
                    //check if user exists in database
                    if (repo.UserIsInDB(inputFirstName, inputLastName))
                        //if so, retrieve their entry from the database and build a UserCLass object from it
                        user = repo.GetUserByName(inputFirstName, inputLastName);
                    else
                    {
                        //if not, add them 
                        user = new UserClass(0, inputFirstName, inputLastName, LocationList[0]);
                        repo.AddNewUser(user);
                        repo.SaveChanges();
                        user = repo.GetUserByName(user.FirstName, user.LastName);
                    }
                    do
                    {

                        //get location to order from
                        user.GetLocation(LocationList);
                        //update user database entry in case they changed their default location
                        repo.UpdateUser(user);
                        repo.SaveChanges();
                        //build locations order history
                        repo.BuildLocationOrderHistory(user.location);
                        //take users oreder and add it to the database
                        var order = user.location.TakeOrder(user);
                        if (order != null)
                        {
                            repo.CreateOrder(order);
                            repo.UpdateLocation(user.location);
                            repo.SaveChanges();
                        }
                        //ask user if they are done
                        Console.WriteLine("Done?(y/n)");
                        input = Console.ReadLine();
                        if (input.ToLower() == "y" || input.ToLower() == "yes")
                            done = true;
                    } while (!done);
                }
            }
        }
        /// <summary>
        /// method to add a new location
        /// </summary>
        /// <param name="repo">DataAccess repository</param>
        public static void OpenNewLocation(IPizzaShopRepo repo)
        {
            //ask user for location description
            Console.WriteLine("New Location Description:");
            string description = Console.ReadLine();
            //declare and instantiate LocationClass Object
            LocationClass newLocation = new LocationClass(description, new List<OrderClass>());
            //add location to database
            repo.AddNewLocation(newLocation);
            repo.SaveChanges();
        }
        /// <summary>
        /// asks user to specify a location, and calls that locations RestockInventory method
        /// </summary>
        /// <param name="repo">DataAccess Repository</param>
        /// <param name="list">list of locations</param>
        public static void RestockInventory(IPizzaShopRepo repo, IList<LocationClass> list)
        {
            //ask user to specify location
            Console.WriteLine("Which Location Would you like to restock?");
            for(int i = 0; i < list.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {list[i].LocationDescription}");
            }
            string input = Console.ReadLine();
            //parse user input, and call locations RestockInventory
            if(int.TryParse(input, out int number) && number > 0 && number <= list.Count)
            {
                list[number - 1].RestockInventory();
            }
        }
        /// <summary>
        /// Asks user to specify location and calls that locations ShowOrderHistory method
        /// </summary>
        /// <param name="list">list of locations</param>
        public static void ShowLocationHistory(IList<LocationClass> list)
        {
            //Ask user to specify location
            Console.WriteLine("Which Location Would you like to view Order History for?");
            for (int i = 0; i < list.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {list[i].LocationDescription}");
            }
            string input = Console.ReadLine();
            //parse user input and call locations ShowOrderHistory method
            if (int.TryParse(input, out int number) && number > 0 && number <= list.Count)
            {
                list[number - 1].ShowOrderHistory();
            }
        }
        /// <summary>
        /// asks user to specify location and calls that locations AddToppingToMenu method
        /// </summary>
        /// <param name="repo">DataAccess Repository</param>
        /// <param name="list">list of locations</param>
        public static void AddToppingToLocationMenu(IPizzaShopRepo repo, IList<LocationClass> list)
        {
            //ask user to specify location
            Console.WriteLine("For which Location Would you like to add a topping to the menu?");
            for (int i = 0; i < list.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {list[i].LocationDescription}");
            }
            string input = Console.ReadLine();
            //parse user input and call locations AddToppingToMenu method
            if (int.TryParse(input, out int number) && number > 0 && number <= list.Count)
            {
                list[number - 1].AddToppingToMenu();
            }
            //update location's database entry
            repo.UpdateLocation(list[number - 1]);
            repo.SaveChanges();
        }
        /// <summary>
        /// asks the user to specify location and calls that locations RemoveToppingFromMenu method
        /// </summary>
        /// <param name="repo">DataAccess repository</param>
        /// <param name="list">list of locations</param>
        public static void RemoveToppingFromLocationMenu(IPizzaShopRepo repo, IList<LocationClass> list)
        {
            //ask user to specify location
            Console.WriteLine("For which Location Would you like to Remove a topping from the menu?");
            for (int i = 0; i < list.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {list[i].LocationDescription}");
            }
            string input = Console.ReadLine();
            //parse user input and call locations RemoveToppingFromMenu method
            if (int.TryParse(input, out int number) && number > 0 && number <= list.Count)
            {
                list[number - 1].RemoveToppingFromMenu();
            }
            //update locations database entry
            repo.UpdateLocation(list[number - 1]);
            repo.SaveChanges();
        }
    }
}
