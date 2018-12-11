using PizzaShop.Library;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace PizzaShopTestProject
{
    public class LocationClassTests
    {
        /// <summary>
        /// test whether time check math works correctly
        /// </summary>
        [Fact]
        public void TimeCheckLogicWorks()
        {
            //daclare and instantiate a LocationClass, UserClass, and OrderClass
            LocationClass sut = new LocationClass("Test", new List<OrderClass>());
            UserClass user = new UserClass(0, "Test", "User", sut);
            OrderClass order = new OrderClass(0, user, sut);
            //set order's time field to DateTime.Now
            order.time = DateTime.Now;
            //send order's time filed to TimeCheck method
            bool actual = sut.TimeCheck(order.time);
            //Assert correct result
            Assert.False(actual);
        }
        ///// <summary>
        ///// Test AddToppingToMenu method with alternate signature
        ///// </summary>
        //[Fact]
        //public void AddToppingToLocationWorks()
        //{
        //    //declare a location
        //    LocationClass sut = new LocationClass("Test", new List<OrderClass>());
        //    //add a topping to locations menu
        //    sut.AddToppingToMenu("Feta Cheese");
        //    //see if topping was added to menu
        //    bool actual = sut.toppings[sut.toppings.Count - 1] == "Feta Cheese";

        //    Assert.True(actual);
        //}
        ///// <summary>
        ///// Test RemoveToppingFromMenu method with alternate signature
        ///// </summary>
        //[Fact]
        //public void RemoveToppingFromLocationWorks()
        //{
        //    //declare a location
        //    LocationClass sut = new LocationClass("Test", new List<OrderClass>());
        //    //remove a topping from the locations menu
        //    sut.RemoveToppingFromMenu("7");
        //    //check that topping's corresponding inventory value to confirm it is flagged as removed
        //    bool actual = sut.inventory[6] == -1;

        //    Assert.True(actual);
        //}
        ///// <summary>
        ///// Test whether AddToppingToMenu correctly identifies a topping that was previously on the menu and restores it rather than adding it again
        ///// </summary>
        //[Fact]
        //public void AddToppingsCorrectlyAddsPreExistingTopping()
        //{
        //    //declare and instantiate a location
        //    LocationClass sut = new LocationClass("Test", new List<OrderClass>());
        //    //remove a topping from locations menu
        //    sut.RemoveToppingFromMenu("7");
        //    //confirm topping removed from menu
        //    bool actual = sut.inventory[6] == -1;
            
        //    Assert.True(actual);
        //    //add topping back to menu
        //    sut.AddToppingToMenu("Onions");
        //    //confirm topping correctly restored to menu
        //    actual = sut.inventory[6] == 50;

        //    Assert.True(actual);
        //}
    }
}
