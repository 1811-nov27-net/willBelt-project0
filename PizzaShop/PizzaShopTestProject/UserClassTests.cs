﻿using PizzaShopUserInterface;
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
            UserClass sut = new UserClass("Will", "Belt", new LocationClass("This Location", new List<OrderClass>()));

            Assert.True(sut.FirstName == "Will");
        }
    }
}
