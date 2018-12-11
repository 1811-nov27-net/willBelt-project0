using PizzaShop.Library;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace PizzaShopTestProject
{
    public class UserClassTests
    {
        /// <summary>
        /// Test User Constructor
        /// </summary>
        [Fact]
        public void UserConstructorCreatesValidUser()
        {
            //declare user and call constructor
            UserClass sut = new UserClass(0, "Will", "Belt", new LocationClass("This Location", new List<OrderClass>()));
            //test user for correct values
            Assert.True(sut.FirstName == "Will");
        }
    }
}
