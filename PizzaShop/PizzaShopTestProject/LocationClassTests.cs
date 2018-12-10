using PizzaShop.Library;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace PizzaShopTestProject
{
    public class LocationClassTests
    {
        [Fact]
        public void TimeCheckLogicWorks()
        {
            LocationClass sut = new LocationClass("Test", new List<OrderClass>());
            UserClass user = new UserClass(0, "Test", "User", sut);
            OrderClass order = new OrderClass(0, user, sut);
            order.time = DateTime.Now;

            bool actual = sut.TimeCheck(order.time);

            Assert.False(actual);
        }
    }
}
