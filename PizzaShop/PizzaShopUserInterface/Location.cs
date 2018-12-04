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
    }
}