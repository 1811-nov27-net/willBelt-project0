using PizzaShopUserInterface;
using System;
using System.Collections.Generic;
using Xunit;

namespace PizzaShopTestProject
{
    public class PizzaClassTests
    {
        [Theory]
        [InlineData(0,0,new bool[] { false, false, false, false, false, false, false}, 5.99)]
        [InlineData(0,2,new bool[] { true, false, true, false, true, false, true}, 6.99)]
        [InlineData(1, 2, new bool[] { true, false, true, false, true, false, true }, 8.99)]
        [InlineData(2, 2, new bool[] { true, true, true, true, true, true, true }, 12.49)]
        public void PizzasCalculatePrice(int size, int crust, bool[] toppingsSelected, double expected)
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
            Pizza sut = new Pizza(sizes, crustTypes, toppings, size, crust, toppingsSelected);

            Assert.Equal(expected, sut.price);
        }
        [Theory]
        [InlineData(3,2,new bool[] { false, false, false, false, false, false, false}, 0.00)]
        [InlineData(1, 3, new bool[] { false, false, false, false, false, false, false }, 0.00)]
        [InlineData(2, 2, new bool[] { false, false, false, false, false, false, false, true }, 0.00)]
        public void InvalidPizzasHaveZeroPrice(int size, int crust, bool[] toppingsSelected, double expected)
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

            Pizza sut = new Pizza(sizes, crustTypes, toppings, size, crust, toppingsSelected);

            Assert.Equal(expected, sut.price);
        }
        [Theory]
        [InlineData(0, 0, new bool[] { false, false, false, false, false, false, false }, "Small Hand-Tossed pizza with no toppings")]
        [InlineData(0, 1, new bool[] { true, false, true, false, true, false, true }, "Small Deep-Dish pizza with Pepperoni, Sausage, Black Olives, and Onions")]
        [InlineData(1, 2, new bool[] { true, false, true, false, true, false, true }, "Medium Thin Crust pizza with Pepperoni, Sausage, Black Olives, and Onions")]
        [InlineData(2, 2, new bool[] { true, true, true, true, true, true, true }, "Large Thin Crust pizza with Pepperoni, Canadian Bacon, Sausage, Mushrooms, Black Olives, Green Peppers, and Onions")]
        public void ToStringReturnsCorrectString(int size, int crust, bool[] toppingsSelected, string expected)
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
            Pizza sut = new Pizza(sizes, crustTypes, toppings, size, crust, toppingsSelected);

            string actual = sut.ToString();

            Assert.Equal(expected, actual);
        }
    }
}
