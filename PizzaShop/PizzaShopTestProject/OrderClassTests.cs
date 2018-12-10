﻿using PizzaShop.Library;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace PizzaShopTestProject
{
    public class OrderClassTests
    {
        [Fact]
        public void DefaultOrderHasNoPizzas()
        {
            LocationClass location = new LocationClass("Test Location", new List<OrderClass>());
            UserClass user = new UserClass(0, "Test", "User", location);
            OrderClass sut = new OrderClass(0, user, location);

            bool actual = (sut.pizzas.Count == 0);

            Assert.True(actual);
        }

        [Theory]
        [InlineData(0, 0, new bool[] { false, false, false, false, false, false, false }, 5.99)]
        [InlineData(0, 2, new bool[] { true, false, true, false, true, false, true }, 6.99)]
        [InlineData(1, 2, new bool[] { true, false, true, false, true, false, true }, 8.99)]
        [InlineData(2, 2, new bool[] { true, true, true, true, true, true, true }, 12.49)]
        public void OrderCalculatesPriceCorrectly(int size, int crust, bool[] toppingChoices, decimal expected)
        {
            Dictionary<int, string> sizes = new Dictionary<int, string>
            {
                { 0, "Small" },
                { 1, "Medium" },
                { 2, "Large" }
            };
            Dictionary<int, string> crustTypes = new Dictionary<int, string>
            {
                { 0, "Hand-Tossed" },
                { 1, "Deep-Dish" },
                { 2, "Thin Crust" }
            };
            Dictionary<int, string> toppings = new Dictionary<int, string>
            {
                { 0, "Pepperoni" },
                { 1, "Canadian Bacon" },
                { 2, "Sausage" },
                { 3, "Mushrooms" },
                { 4, "Black Olives" },
                { 5, "Green Peppers" },
                { 6, "Onions" }
            };
            LocationClass location = new LocationClass("Test Location", new List<OrderClass>());
            UserClass user = new UserClass(0, "Test", "User", location);
            OrderClass sut = new OrderClass(0, user, location); sut.AddPizza(new PizzaClass(sizes, crustTypes, toppings, size, crust, toppingChoices));

            decimal actual = sut.total;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0, 0, new bool[] { false, false, false, false, false, false, false }, 11.98)]
        [InlineData(0, 2, new bool[] { true, false, true, false, true, false, true }, 13.98)]
        [InlineData(1, 2, new bool[] { true, false, true, false, true, false, true }, 17.98)]
        [InlineData(2, 2, new bool[] { true, true, true, true, true, true, true }, 24.98)]
        public void OrderCalculatesPriceForMultiplePizzas(int size, int crust, bool[] toppingChoices, decimal expected)
        {
            Dictionary<int, string> sizes = new Dictionary<int, string>
            {
                { 0, "Small" },
                { 1, "Medium" },
                { 2, "Large" }
            };
            Dictionary<int, string> crustTypes = new Dictionary<int, string>
            {
                { 0, "Hand-Tossed" },
                { 1, "Deep-Dish" },
                { 2, "Thin Crust" }
            };
            Dictionary<int, string> toppings = new Dictionary<int, string>
            {
                { 0, "Pepperoni" },
                { 1, "Canadian Bacon" },
                { 2, "Sausage" },
                { 3, "Mushrooms" },
                { 4, "Black Olives" },
                { 5, "Green Peppers" },
                { 6, "Onions" }
            };

            LocationClass location = new LocationClass("Test Location", new List<OrderClass>());
            UserClass user = new UserClass(0, "Test", "User", location);
            OrderClass sut = new OrderClass(0, user, location); sut.AddPizza(new PizzaClass(sizes, crustTypes, toppings, size, crust, toppingChoices));
            sut.AddPizza(new PizzaClass(sizes, crustTypes, toppings, size, crust, toppingChoices));

            decimal actual = sut.total;

            Assert.Equal(expected, actual);
        }
    }
}
