using System;
using System.Collections.Generic;

namespace PizzaShopUserInterface
{
    public class Location
    {
        Dictionary<int, int> inventory = new Dictionary<int, int> { {0,50}, {1,50},{2,50},{3,50}, {4,50}, {5,50}, {6,50} };
        static Dictionary<int, string> sizes = new Dictionary<int, string> { { 0, "Small" }, { 1, "Medium" }, { 2, "Large" } };
        static Dictionary<int, string> crustTypes = new Dictionary<int, string> { { 0, "Hand-Tossed" }, { 1, "Deep-Dish" }, { 2, "Thin Crust" } };
        static Dictionary<int, string> toppings = new Dictionary<int, string> { { 0, "Pepperoni" }, { 1, "Canadian Bacon" }, { 2, "Sausage" }, { 3, "Mushrooms" }, { 4, "Black Olives" }, { 5, "Green Peppers" }, { 6, "Onions" } };
        public List<Order> OrderHistory;
        List<string> OrderRequestStrings;
        private bool isValidInput = false;
        private string input;
        private bool[] toppingChoices;
        private int size, crust, inputIndex;
        Order newOrder;
        public string LocationDescription;

        public Location(string description, List<Order> history)
        {
            LocationDescription = description;
            OrderHistory = history;
        }
        public void TakeOrder(User user)
        {
            newOrder = new Order();
            newOrder.customer = user;
            bool OrderCompleted = false;
            do {
                BuildOrderRequestStrings();
                GetSizeOrder();
                isValidInput = false;
                GetCrustOrder();
                isValidInput = false;
                GetToppigsOrder();
                CheckInventory();
                Pizza newPizza = new Pizza(sizes, crustTypes, toppings, size, crust, toppingChoices);
                Console.WriteLine(newOrder.AddPizza(newPizza));
                Console.WriteLine($"Total: ${newOrder.total}");
                Console.WriteLine("Would you like to order anything else? y/n");
                input = Console.ReadLine();
                if (input.ToLower() == "n" || input.ToLower() == "no")
                    OrderCompleted = true;

            } while (!OrderCompleted);
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
    }
}