﻿using System.Collections.Generic;

namespace PizzaShopUserInterface
{
    public class PizzaClass
    {
        Dictionary<int, string> sizes;
        Dictionary<int, string> crustTypes;
        Dictionary<int, string> toppings;
        public int size { get; set; }
        public int crustSelection { get; set; }
        public bool[] toppingSelection = new bool[7];
        public double price { get; set; }
        /// <summary>
        /// Pizza constructor
        /// </summary>
        /// <param name="sizes">Menu of Size options</param>
        /// <param name="crustTypes">Menu of crust types</param>
        /// <param name="toppings">menu of toppings</param>
        /// <param name="chosenSize">selected size</param>
        /// <param name="crust">selected crust type</param>
        /// <param name="toppingChoices">desired toppings</param>
        public PizzaClass(Dictionary<int, string> sizes, Dictionary<int, string> crustTypes, Dictionary<int, string> toppings, int chosenSize, int crust, bool[] toppingChoices)
        {
            //import menu from location
            this.sizes = sizes;
            this.crustTypes = crustTypes;
            this.toppings = toppings;
            //ensure topping selections are valid
            bool toppingsAreValid = true;
            for(int i = 0; i < toppingChoices.Length; i++)
            {
                if (toppingChoices[i] && !toppings.ContainsKey(i))
                    toppingsAreValid = false;
            }
            if (sizes.ContainsKey(chosenSize) && crustTypes.ContainsKey(crust) && toppingsAreValid)
            {
                size = chosenSize;
                crustSelection = crust;
                toppingSelection = toppingChoices;
                price = this.CalculatePrice();
            }
        }
        /// <summary>
        /// method that calculates the price of the individual pizza
        /// </summary>
        /// <returns></returns>
        private double CalculatePrice()
        {
            int numberOfToppings = 0;
            double sizePrice = 0.0;
            switch (this.size)
            {
                case 0:
                    sizePrice = 5.99;
                    break;
                case 1:
                    sizePrice = 7.99;
                    break;
                case 2:
                    sizePrice = 9.99;
                    break;
            }
            foreach(bool choice in toppingSelection)
            {
                if(choice)
                {
                    numberOfToppings++;
                }
            }
            if (numberOfToppings > 2)
                return sizePrice + ((numberOfToppings - 2) * .50);
            else
                return sizePrice;
        }
        /// <summary>
        /// Method that returns a string describing the pizza
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            List<string> toppingsList = new List<string>();
            string toppingsString = "";
            bool noToppings = true;
            for (int i = 0; i < toppingSelection.Length; i++)
            {
                if (toppingSelection[i])
                {
                    toppingsList.Add(toppings[i]);
                    noToppings = false;
                }   
            }
            for(int i = 0; i < toppingsList.Count; i++)
            {
                toppingsString += toppingsList[i];
                if (i + 1 < toppingsList.Count - 1)
                    toppingsString += ", ";
                else if (i + 1 == toppingsList.Count - 1)
                    toppingsString += ", and ";
            }
            if (noToppings)
                toppingsString = "no toppings";
            return ($"{sizes[size]} {crustTypes[crustSelection]} pizza with {toppingsString}: ${price}");
        }
    }
}