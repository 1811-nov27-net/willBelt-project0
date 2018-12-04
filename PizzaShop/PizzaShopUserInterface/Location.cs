using System;
using System.Collections.Generic;

namespace PizzaShopUserInterface
{
    internal class Location
    {
        Dictionary<int, int> inventory = new Dictionary<int, int> { {0,50}, {1,50},{2,50},{3,50}, {4,50}, {5,50}, {6,50} };
        static Dictionary<int, string> sizes = new Dictionary<int, string> { { 0, "Small" }, { 1, "Medium" }, { 2, "Large" } };
        static Dictionary<int, string> crustTypes = new Dictionary<int, string> { { 0, "Hand-Tossed" }, { 1, "Deep-Dish" }, { 2, "Thin Crust" } };
        static Dictionary<int, string> toppings = new Dictionary<int, string> { { 0, "Pepperoni" }, { 1, "Canadian Bacon" }, { 2, "Sausage" }, { 3, "Mushrooms" }, { 4, "Black Olives" }, { 5, "Green Peppers" }, { 6, "Onions" } };
        List<Order> OrderHistory = new List<Order>();
        List<string> OrderRequestStrings;
        private bool isValidInput = false;
        private string input;
        private bool[] toppingChoices = new bool[toppings.Count];
        private int size = 0, crust = 0;
        private int inputIndex;

        public void TakeOrder()
        {
            BuildOrderRequestStrings();
            GetSizeOrder();
            isValidInput = false;
            GetCrustOrder();
            isValidInput = false;
            GetToppigsOrder();
            for (int i = 0; i < toppingChoices.Length; i++)
            {
                if (toppingChoices[i] && inventory[i] != 0)
                {
                    inventory[i]--;
                }
                else if (inventory[i] == 0)
                    Console.WriteLine($"We cannot fulfill that order because we are out of {toppings[i]}");
            }
            Order newOrder = new Order();
            newOrder.AddPizza(new Pizza(sizes, crustTypes, toppings, size, crust, toppingChoices));
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
            OrderRequestStrings.Add("What toppings would you like(enter your selections one at a time, then enter -1 when done)?\n");
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
                isValidInput = (input.Length == 1 && (sizes.ContainsKey((int)input[0])));
                if (isValidInput)
                    size = (int)input[0];
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
                isValidInput = (input.Length == 1 && (crustTypes.ContainsKey((int)input[0])));
                if (isValidInput)
                    crust = (int)input[0];
                else
                    Console.WriteLine("Invalid entry, please enter the number of your selection");
            } while (!isValidInput);
        }
        private void GetToppigsOrder()
        {
            do
            {
                int temporary = 0;
                inputIndex = 0;
                Console.Write(OrderRequestStrings[2]);
                input = Console.ReadLine();
                if (input.Length == 1)
                    inputIndex = input[0];
                else if (input.Length > 1)
                {
                    for (int i = 0; i < input.Length; i++)
                    {
                        temporary = input[i];
                        for (int j = (input.Length - 1) - i; j > 0; j--)
                            temporary *= 10;
                        inputIndex += temporary;
                    }
                }
                isValidInput = inputIndex > -2 && toppings.ContainsKey(inputIndex);

                if (isValidInput)
                    toppingChoices[inputIndex] = true;
                else
                    Console.WriteLine("Invalid entry, please enter the number of your selection");
            } while (inputIndex != -1);
        }
    }
}