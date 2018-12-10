using Microsoft.EntityFrameworkCore;
using PizzaShop.DataAccess;
using PizzaShop.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace PizzaShopTestProject
{
    public class PizzaShopRepoTests
    {
        [Fact]
        public void AddNewLocationWorks()
        {
            var options = new DbContextOptionsBuilder<ProjectsContext>()
                .UseInMemoryDatabase("add_new_location_test").Options;
            using (var db = new ProjectsContext(options))
            {
                IPizzaShopRepo sut = new PizzaShopRepo(db);
                LocationClass location = new LocationClass("Test Location", new List<OrderClass>());
                sut.AddNewLocation(location);
                sut.SaveChanges();

                bool actual = db.Locations.Where(l => l.LocationDescription == "Test Location").ToList().Count > 0;

                Assert.True(actual);
            }
        }

        [Fact]
        public void AddNewUserWorks()
        {
            var options = new DbContextOptionsBuilder<ProjectsContext>()
                .UseInMemoryDatabase("add_new_user_test").Options;
            using (var db = new ProjectsContext(options))
            {
                IPizzaShopRepo sut = new PizzaShopRepo(db);
                LocationClass location = new LocationClass("Test Location", new List<OrderClass>());
                UserClass user = new UserClass(0, "Test", "User", location);
                sut.AddNewLocation(location);
                sut.SaveChanges();
                sut.AddNewUser(user);
                sut.SaveChanges();

                bool actual = db.Users.Where(u => (u.FirstName == "Test" && u.LastName == "User")).ToList().Count > 0;

                Assert.True(actual);
            }
        }
        [Fact]
        public void CreateOrderWorks()
        {
            var options = new DbContextOptionsBuilder<ProjectsContext>()
                .UseInMemoryDatabase("create_order_test").Options;
            using (var db = new ProjectsContext(options))
            {
                IPizzaShopRepo sut = new PizzaShopRepo(db);
                LocationClass location = new LocationClass("Test Location", new List<OrderClass>());
                UserClass user = new UserClass(0, "Test", "User", location);
                OrderClass order = new OrderClass(0, user, location);
                PizzaClass pizza = new PizzaClass(
                    location.sizes,
                    location.crustTypes,
                    location.toppings,
                    0,
                    0,
                    new bool[] {
                        false,
                        false,
                        false,
                        false,
                        false,
                        false,
                        false});
                order.AddPizza(pizza);
                order.time = DateTime.Now;
                sut.AddNewLocation(location);
                sut.SaveChanges();
                sut.AddNewUser(user);
                sut.SaveChanges();
                sut.CreateOrder(order);
                sut.SaveChanges();

                bool actual = db.Orders.Where(
                    o => o.LocationId == db.Locations.First(
                        l => l.LocationDescription == location.LocationDescription).LocationId)
                    .ToList().Count > 0;

                Assert.True(actual);
            }
        }
        [Fact]
        public void GetAllLocationsWorks()
        {
            var options = new DbContextOptionsBuilder<ProjectsContext>()
                .UseInMemoryDatabase("get_all_locations_test").Options;
            using (var db = new ProjectsContext(options))
            {
                IPizzaShopRepo sut = new PizzaShopRepo(db);
                LocationClass location = new LocationClass("Test Location1", new List<OrderClass>());
                LocationClass location2 = new LocationClass("Test Location2", new List<OrderClass>());
                sut.AddNewLocation(location);
                sut.AddNewLocation(location2);
                sut.SaveChanges();

                IList<LocationClass> list = sut.GetAllLocations();

                bool actual = db.Locations.ToList().Count == list.Count;

                Assert.True(actual);
            }
        }

        [Fact]
        public void GetAllOrdersWorks()
        {
            var options = new DbContextOptionsBuilder<ProjectsContext>()
                .UseInMemoryDatabase("get_all_orders_test").Options;
            using (var db = new ProjectsContext(options))
            {
                IPizzaShopRepo sut = new PizzaShopRepo(db);
                LocationClass location = new LocationClass("Test Location", new List<OrderClass>());
                UserClass user = new UserClass(0, "Test", "User", location);
                OrderClass order = new OrderClass(0, user, location);
                PizzaClass pizza = new PizzaClass(
                    location.sizes,
                    location.crustTypes,
                    location.toppings,
                    0,
                    0,
                    new bool[] {
                        false,
                        false,
                        false,
                        false,
                        false,
                        false,
                        false});
                order.AddPizza(pizza);
                order.time = DateTime.Now;
                sut.AddNewLocation(location);
                sut.SaveChanges();
                sut.AddNewUser(user);
                sut.SaveChanges();
                sut.CreateOrder(order);
                sut.CreateOrder(order);
                sut.SaveChanges();

                IList<OrderClass> list = sut.GetAllOrders();

                bool actual = db.Orders.ToList().Count == list.Count;

                Assert.True(actual);
            }
        }

        [Fact]
        public void GetAllUsersWorks()
        {
            var options = new DbContextOptionsBuilder<ProjectsContext>()
                .UseInMemoryDatabase("get_all_users_test").Options;
            using (var db = new ProjectsContext(options))
            {
                IPizzaShopRepo sut = new PizzaShopRepo(db);
                LocationClass location = new LocationClass("Test Location", new List<OrderClass>());
                UserClass user = new UserClass(0, "Test", "User", location);
                sut.AddNewLocation(location);
                sut.SaveChanges();
                sut.AddNewUser(user);
                sut.SaveChanges();

                IList<UserClass> list = sut.GetAllUsers();

                bool actual = db.Users.ToList().Count == list.Count;

                Assert.True(actual);
            }
        }

        [Fact]
        public void GetOrdersByUserReturnsOnlyCorrectOrders()
        {
            var options = new DbContextOptionsBuilder<ProjectsContext>()
                .UseInMemoryDatabase("get_orders_by_user_test").Options;
            using (var db = new ProjectsContext(options))
            {
                IPizzaShopRepo sut = new PizzaShopRepo(db);
                LocationClass location = new LocationClass("Test Location", new List<OrderClass>());
                UserClass user = new UserClass(0, "Test", "User", location);
                UserClass user2 = new UserClass(0, "Test", "User2", location);
                OrderClass order = new OrderClass(0, user, location);
                OrderClass order2 = new OrderClass(0, user2, location);
                PizzaClass pizza = new PizzaClass(
                    location.sizes,
                    location.crustTypes,
                    location.toppings,
                    0,
                    0,
                    new bool[] {
                        false,
                        false,
                        false,
                        false,
                        false,
                        false,
                        false});
                order.AddPizza(pizza);
                order2.AddPizza(pizza);
                order.time = DateTime.Now;
                sut.AddNewLocation(location);
                sut.SaveChanges();
                sut.AddNewUser(user);
                sut.AddNewUser(user2);
                sut.SaveChanges();
                sut.CreateOrder(order);
                sut.CreateOrder(order2);
                sut.SaveChanges();

                IList<OrderClass> list = sut.GetOrdersByUser(user);

                bool actual = true;

                foreach (var item in list)
                {
                    if (item.customer.UserID != db.Users.First(u => u.FirstName == user.FirstName && u.LastName == user.LastName).UserId)
                        actual = false;
                }

                Assert.True(actual);
            }
        }

        [Fact]
        public void UserIsInDBWorks()
        {
            var options = new DbContextOptionsBuilder<ProjectsContext>()
                .UseInMemoryDatabase("user_is_in_db_test").Options;
            using (var db = new ProjectsContext(options))
            {
                IPizzaShopRepo sut = new PizzaShopRepo(db);
                LocationClass location = new LocationClass("Test Location", new List<OrderClass>());
                UserClass user = new UserClass(0, "Test", "User", location);
                sut.AddNewLocation(location);
                sut.SaveChanges();
                sut.AddNewUser(user);
                sut.SaveChanges();

                bool actual = sut.UserIsInDB("Test", "User");

                Assert.True(actual);

                actual = sut.UserIsInDB("Test", "User2");

                Assert.False(actual);
            }
        }

        [Fact]
        public void GetUserByNameWorks()
        {
            var options = new DbContextOptionsBuilder<ProjectsContext>()
                .UseInMemoryDatabase("get_user_by_name_test").Options;
            using (var db = new ProjectsContext(options))
            {
                IPizzaShopRepo sut = new PizzaShopRepo(db);
                LocationClass location = new LocationClass("Test Location", new List<OrderClass>());
                UserClass user = new UserClass(0, "Test", "User", location);
                sut.AddNewLocation(location);
                sut.SaveChanges();
                sut.AddNewUser(user);
                sut.SaveChanges();

                UserClass user2 = sut.GetUserByName("Test", "User");

                bool actual = user2.FirstName == user.FirstName && user2.LastName == user.LastName;

                Assert.True(actual);
            }
        }

        [Fact]
        public void GetLocationByDescriptionWorks()
        {
            var options = new DbContextOptionsBuilder<ProjectsContext>()
                .UseInMemoryDatabase("get_location_by_description_test").Options;
            using (var db = new ProjectsContext(options))
            {
                IPizzaShopRepo sut = new PizzaShopRepo(db);
                LocationClass location = new LocationClass("Test Location", new List<OrderClass>());
                sut.AddNewLocation(location);
                sut.SaveChanges();
                location = sut.GetLocationByDescription(location.LocationDescription);

                bool actual = location.LocationDescription == "Test Location" && location.LocationID != 0;

                Assert.True(actual);
            }
        }

        [Fact]
        public void UpdateLocationWorks()
        {
            var options = new DbContextOptionsBuilder<ProjectsContext>()
                .UseInMemoryDatabase("update_location_test").Options;
            using (var db = new ProjectsContext(options))
            {
                IPizzaShopRepo sut = new PizzaShopRepo(db);
                LocationClass location = new LocationClass("Test Location", new List<OrderClass>());
                sut.AddNewLocation(location);
                sut.SaveChanges();
                location = sut.GetLocationByDescription(location.LocationDescription);
                location.inventory[0] = 100;
                sut.UpdateLocation(location);
                sut.SaveChanges();
                IList<LocationClass> list = sut.GetAllLocations();

                bool actual = list.First(l => l.LocationDescription == location.LocationDescription).inventory[0] == 100;

                Assert.True(actual);
            }
        }
    }
}
