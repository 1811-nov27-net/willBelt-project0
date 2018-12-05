using PizzaShopUserInterface;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace PizzaShopTestProject
{
    public class UserClassTests
    {
        [Fact]
        public void UserConstructorCreatesValidUser()
        {
            User sut = new User("Will", "Belt", new Location("This Location", new List<Order>()));

            Assert.True(sut.FirstName == "Will");
        }
    }
}
