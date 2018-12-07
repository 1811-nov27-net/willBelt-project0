using System;
using System.Collections.Generic;

namespace PizzaShopUserInterface
{
    public class OrderClass
    {
        public UserClass customer { get; set; }
        public List<PizzaClass> pizzas = new List<PizzaClass>();
        public DateTime time { get; set; }
        public LocationClass location { get; set; }
        public decimal total { get; set; }

        public string AddPizza(PizzaClass newPizza)
        {
            if (pizzas.Count < 12 && newPizza.price != 0.0m)
            {
                this.pizzas.Add(newPizza);
                if (total + newPizza.price < 500.00m)
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