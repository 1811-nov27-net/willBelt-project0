﻿using System;
using System.Collections.Generic;

namespace PizzaShopUserInterface
{
    public class LocationClass
    {
        Dictionary<int, int> inventory = new Dictionary<int, int> { {0,50}, {1,50},{2,50},{3,50}, {4,50}, {5,50}, {6,50} };
        public Dictionary<int, string> sizes = new Dictionary<int, string> { { 0, "Small" }, { 1, "Medium" }, { 2, "Large" } };
        public Dictionary<int, string> crustTypes = new Dictionary<int, string> { { 0, "Hand-Tossed" }, { 1, "Deep-Dish" }, { 2, "Thin Crust" } };
        public Dictionary<int, string> toppings = new Dictionary<int, string> {
            { 0, "Pepperoni" },
            { 1, "Canadian Bacon" },
            { 2, "Sausage" },
            { 3, "Mushrooms" },
            { 4, "Black Olives" },
            { 5, "Green Peppers" },
            { 6, "Onions" }
        };
        public List<OrderClass> OrderHistory;
        List<string> OrderRequestStrings;
        private string input;
        private bool[] toppingChoices;
        private int size, crust;
        OrderClass newOrder;
        public string LocationDescription;

        public LocationClass(string description, List<OrderClass> history)
        {
            LocationDescription = description;
            OrderHistory = history;
        }

        public LocationClass(string description, List<OrderClass> history, string menu, string inventory)
        {
            LocationDescription = description;
            OrderHistory = history;
            string[] menuSubstrings = menu.Split('/');
            BuildMenu(menuSubstrings[0], sizes);
            BuildMenu(menuSubstrings[1], crustTypes);
            BuildMenu(menuSubstrings[2], toppings);
            BuildInventory(inventory);
            
        }

        public void TakeOrder(UserClass user)
        {
            newOrder = new OrderClass();
            newOrder.customer = user;
            bool OrderCompleted = false;
            do {
                BuildOrderRequestStrings();
                GetSizeOrder();
                GetCrustOrder();
                GetToppigsOrder();
                CheckInventory();
                PizzaClass newPizza = new PizzaClass(sizes, crustTypes, toppings, size, crust, toppingChoices);
                Console.WriteLine(newOrder.AddPizza(newPizza));
                Console.WriteLine($"Total: ${newOrder.total}");
                Console.WriteLine("Would you like to order anything else? y/n");
                input = Console.ReadLine();
                if (input.ToLower() == "n" || input.ToLower() == "no")
                    OrderCompleted = true;

            } while (!OrderCompleted);
            newOrder.time = DateTime.Now;
            OrderHistory.Add(newOrder);
        }

        private void BuildOrderRequestStrings()
        {
            OrderRequestStrings = new List<string>();
            OrderRequestStrings.Add("What Size Pizza would you like?\n");
            foreach (KeyValuePair<int,string> pair in sizes)
            {
                OrderRequestStrings[0] += ($"{pair.Key+1}. {pair.Value}\n");
            }
            OrderRequestStrings.Add("What type of crust would you like?\n");
            foreach(KeyValuePair<int,string> pair in crustTypes)
            {
                OrderRequestStrings[1] += ($"{pair.Key+1}. {pair.Value}\n");
            }
            OrderRequestStrings.Add("What toppings would you like(enter your selections one at a time, then enter 'Done' when done)?\n");
            foreach(KeyValuePair<int,string> pair in toppings)
            {
                OrderRequestStrings[2] += ($"{pair.Key+1}. {pair.Value}\n");
            }
        }
        private void GetSizeOrder()
        {
            bool isValidInput = false;
            do
            {
                Console.Write(OrderRequestStrings[0]);
                input = Console.ReadLine();
                isValidInput = (int.TryParse(input, out int number) && (sizes.ContainsKey(number-1)));
                if (isValidInput)
                    size = number - 1;
                else
                    Console.WriteLine("Invalid entry, please enter the number of your selection");
            } while (!isValidInput);
        }
        private void GetCrustOrder()
        {
            bool isValidInput = false;
            do
            {
                Console.Write(OrderRequestStrings[1]);
                input = Console.ReadLine();
                isValidInput = (int.TryParse(input, out int number) && (crustTypes.ContainsKey(number-1)));
                if (isValidInput)
                    crust = number - 1;
                else
                    Console.WriteLine("Invalid entry, please enter the number of your selection");
            } while (!isValidInput);
        }
        private void GetToppigsOrder()
        {
            bool isValidInput = false;
            toppingChoices = new bool[toppings.Count];
            bool done = false;
            do
            {
                Console.Write(OrderRequestStrings[2]);
                input = Console.ReadLine();
                isValidInput = (int.TryParse(input, out int number) && toppings.ContainsKey(number-1));
                if (isValidInput)
                    toppingChoices[number-1] = true;
                else if (input.ToLower() == "done")
                    done = true;
                else
                    Console.WriteLine("Invalid entry, please enter the number of your selection");
            } while (!done);
        }
        private void CheckInventory()
        {
            for (int i = 0; i < toppingChoices.Length; i++)
            {
                if (toppingChoices[i] && inventory[i] != 0)
                {
                    inventory[i]--;
                }
                else if (inventory[i] == 0)
                    Console.WriteLine($"We cannot fulfill that order because we are out of {toppings[i]}");
            }
        }

        public string GetMenu()
        {
            string menuString = "";
            bool firstLoop = true;
            foreach (KeyValuePair<int, string> pair in sizes)
            {
                if (!firstLoop)
                    menuString += ",";
                menuString += $"{pair.Value}";
                if (firstLoop)
                    firstLoop = false;
            }
            menuString += "/";
            firstLoop = true;
            foreach (KeyValuePair<int, string> pair in crustTypes)
            {
                if (!firstLoop)
                    menuString += ",";
                menuString += $"{pair.Value}";
                if (firstLoop)
                    firstLoop = false;
            }
            menuString += "/";
            firstLoop = true;
            foreach (KeyValuePair<int, string> pair in toppings)
            {
                if (!firstLoop)
                    menuString += ",";
                menuString += $"{pair.Value}";
                if (firstLoop)
                    firstLoop = false;
            }
            return menuString;
        }

        public string GetInventory()
        {
            string inventoryString = "";
            bool firstLoop = true;
            foreach ( KeyValuePair<int,int> pair in inventory)
            {
                if (!firstLoop)
                    inventoryString += ",";
                inventoryString += $"{pair.Value}";
                if (firstLoop)
                    firstLoop = false;
            }
            return inventoryString;
        }

        private void BuildMenu(string Substring, Dictionary<int,string> menu)
        {
            string[] strings = Substring.Split(',');
            for (int i = 0; i < strings.Length; i++)
            {
                menu.Add(i, strings[i]);
            }
        }

        private void BuildInventory(string Substring)
        {
            string[] strings = Substring.Split(',');
            for (int i = 0; i < strings.Length; i++)
            {
                if (int.TryParse(strings[i], out int number))
                    inventory.Add(i, number);
                else
                    Console.WriteLine("Error Building Inventory, Inventory String contains Invalid Value");
            }
        }
    }
}