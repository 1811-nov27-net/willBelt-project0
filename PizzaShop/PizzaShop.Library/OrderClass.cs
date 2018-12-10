using System;
using System.Collections.Generic;

namespace PizzaShop.Library
{
    public class OrderClass
    {
        public UserClass customer { get; set; }
        public List<PizzaClass> pizzas = new List<PizzaClass>();
        public DateTime time { get; set; }
        public LocationClass location { get; set; }
        public decimal total { get; set; }
        public int OrderID { get; set; }

        public OrderClass(int orderID, UserClass user, LocationClass location)
        {
            OrderID = orderID;
            customer = user;
            this.location = location;
            total = 0.0m;
        }

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

        public override string ToString()
        {
            var orderString = $"Customer: {customer.FirstName} {customer.LastName}\n";
            foreach (var pizza in pizzas)
            {
                orderString += $"{pizza.ToString()}\n";
            }
            orderString += $"Total: ${total}";
            return orderString;
        }
    }
}