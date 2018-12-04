using System;
using System.Collections.Generic;

namespace PizzaShopUserInterface
{
    public class Order
    {
        public User customer { get; set; }
        public List<Pizza> pizzas { get; set; }
        public DateTime time { get; set; }
        public double total { get; set; }

        public string AddPizza(Pizza newPizza)
        {
            if (pizzas.Count < 12 && newPizza.price != 0.0)
            {
                this.pizzas.Add(newPizza);
                if (total + newPizza.price < 500.00)
                {
                    total += newPizza.price;
                    return newPizza.ToString() + " has been added to your order.";
                }
                else
                    return "New pizza not added to your order, total may not exceed $500.00";
            }
            else if(pizzas.Count == 12)
            {
                return "No more than 12 pizzas may be added to a single order";
            }
            else
                return "Invalid Selection, please try again.";
        }
    }
}