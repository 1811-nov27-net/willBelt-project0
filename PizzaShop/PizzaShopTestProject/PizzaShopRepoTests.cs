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
        /// <summary>
        /// Test AddNewLocation method
        /// </summary>
        [Fact]
        public void AddNewLocationWorks()
        {
            //setup inmemory database
            var options = new DbContextOptionsBuilder<ProjectsContext>()
                .UseInMemoryDatabase("add_new_location_test").Options;
            using (var db = new ProjectsContext(options))
            {
                //declare and instantiate IPizzaShopRepo and LocationClass
                IPizzaShopRepo sut = new PizzaShopRepo(db);
                LocationClass location = new LocationClass("Test Location", new List<OrderClass>());
                //call AddNewLocationMethod and SaveChanges method
                sut.AddNewLocation(location);
                sut.SaveChanges();
                //check whether a location has been added to the previously empty database
                bool actual = db.Locations.Where(l => l.LocationDescription == "Test Location").ToList().Count > 0;

                Assert.True(actual);
            }
        }
        /// <summary>
        /// Test AddNewUser method
        /// </summary>
        [Fact]
        public void AddNewUserWorks()
        {
            //setup inmemory database
            var options = new DbContextOptionsBuilder<ProjectsContext>()
                .UseInMemoryDatabase("add_new_user_test").Options;
            using (var db = new ProjectsContext(options))
            {
                //declare and instantiate IPizzaShopRepo, LocationClass, and UserClass 
                IPizzaShopRepo sut = new PizzaShopRepo(db);
                LocationClass location = new LocationClass("Test Location", new List<OrderClass>());
                UserClass user = new UserClass(0, "Test", "User", location);
                //Add location to database so that Users ForeignKey can reference it and save changes
                sut.AddNewLocation(location);
                sut.SaveChanges();
                //add user to database and save changes
                sut.AddNewUser(user);
                sut.SaveChanges();
                //check whether user was added to the previously empty database
                bool actual = db.Users.Where(u => (u.FirstName == "Test" && u.LastName == "User")).ToList().Count > 0;

                Assert.True(actual);
            }
        }
        /// <summary>
        /// test CreateOrder method
        /// </summary>
        [Fact]
        public void CreateOrderWorks()
        {
            //setup inmemory database
            var options = new DbContextOptionsBuilder<ProjectsContext>()
                .UseInMemoryDatabase("create_order_test").Options;
            using (var db = new ProjectsContext(options))
            {
                //declare and instantiate IPizzaShopRepo, LocationClass, UserCLass, OrderClass, and Pizza Class
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
                //add pizza to order and set time to now
                order.AddPizza(pizza);
                order.time = DateTime.Now;
                //add location, user, and order to database
                sut.AddNewLocation(location);
                sut.SaveChanges();
                sut.AddNewUser(user);
                sut.SaveChanges();
                sut.CreateOrder(order);
                sut.SaveChanges();
                //check if order was added to previously empty database
                bool actual = db.Orders.Where(
                    o => o.LocationId == db.Locations.First(
                        l => l.LocationDescription == location.LocationDescription).LocationId)
                    .ToList().Count > 0;

                Assert.True(actual);
            }
        }
        /// <summary>
        /// Test GetAllLocations method
        /// </summary>
        [Fact]
        public void GetAllLocationsWorks()
        {
            //setup inmemory database
            var options = new DbContextOptionsBuilder<ProjectsContext>()
                .UseInMemoryDatabase("get_all_locations_test").Options;
            using (var db = new ProjectsContext(options))
            {
                //declare and instantiate IPizzaShopRepo and multiple locations
                IPizzaShopRepo sut = new PizzaShopRepo(db);
                LocationClass location = new LocationClass("Test Location1", new List<OrderClass>());
                LocationClass location2 = new LocationClass("Test Location2", new List<OrderClass>());
                //add locations to database and save changes
                sut.AddNewLocation(location);
                sut.AddNewLocation(location2);
                sut.SaveChanges();
                //call GetAllLocations
                IList<LocationClass> list = sut.GetAllLocations();
                //test if all locations in database are now in list
                bool actual = db.Locations.ToList().Count == list.Count;

                Assert.True(actual);
            }
        }
        /// <summary>
        /// Test GetAllOrders method
        /// </summary>
        [Fact]
        public void GetAllOrdersWorks()
        {
            //setup inmemory database
            var options = new DbContextOptionsBuilder<ProjectsContext>()
                .UseInMemoryDatabase("get_all_orders_test").Options;
            using (var db = new ProjectsContext(options))
            {
                //declare and instantiate IPizzaShopRepo, LocationClass, UserClass, OrderClass, and PizzaClass
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
                //add pizza to order and set order.time
                order.AddPizza(pizza);
                order.time = DateTime.Now;
                //add location, and user to database, then add order twice, creating two entries of the same order
                sut.AddNewLocation(location);
                sut.SaveChanges();
                sut.AddNewUser(user);
                sut.SaveChanges();
                sut.CreateOrder(order);
                sut.CreateOrder(order);
                sut.SaveChanges();
                //call GetAllOrders method
                IList<OrderClass> list = sut.GetAllOrders();
                //test whether all orders in database are now in list
                bool actual = db.Orders.ToList().Count == list.Count;

                Assert.True(actual);
            }
        }
        /// <summary>
        /// Test GetAllUsers method
        /// </summary>
        [Fact]
        public void GetAllUsersWorks()
        {
            //setup inmemory database
            var options = new DbContextOptionsBuilder<ProjectsContext>()
                .UseInMemoryDatabase("get_all_users_test").Options;
            using (var db = new ProjectsContext(options))
            {
                //declare and instantiate IPizzaShopRepo, LocationClass, and UserClass
                IPizzaShopRepo sut = new PizzaShopRepo(db);
                LocationClass location = new LocationClass("Test Location", new List<OrderClass>());
                UserClass user = new UserClass(0, "Test", "User", location);
                //add location and user to database
                sut.AddNewLocation(location);
                sut.SaveChanges();
                sut.AddNewUser(user);
                sut.SaveChanges();
                //call GetAllUsers method
                IList<UserClass> list = sut.GetAllUsers();
                //test whether all users in database are now in list
                bool actual = db.Users.ToList().Count == list.Count;

                Assert.True(actual);
            }
        }
        /// <summary>
        /// Test GetOrderByUser to ensure it returns only order from the correct user
        /// </summary>
        [Fact]
        public void GetOrdersByUserReturnsOnlyCorrectOrders()
        {
            //setup inmemory database
            var options = new DbContextOptionsBuilder<ProjectsContext>()
                .UseInMemoryDatabase("get_orders_by_user_test").Options;
            using (var db = new ProjectsContext(options))
            {
                //declare and instantiate repo, location, two users, two orders and a pizza
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
                //add pizza to both orders, and set both orders time to now
                order.AddPizza(pizza);
                order2.AddPizza(pizza);
                order.time = DateTime.Now;
                order2.time = DateTime.Now;
                //add location, both users, and both orders to the database
                sut.AddNewLocation(location);
                sut.SaveChanges();
                sut.AddNewUser(user);
                sut.AddNewUser(user2);
                sut.SaveChanges();
                sut.CreateOrder(order);
                sut.CreateOrder(order2);
                sut.SaveChanges();
                //call GetOrdersByUser
                IList<OrderClass> list = sut.GetOrdersByUser(user);

                bool actual = true;
                //iterate through list to make sure all orders it contains belong to the correct user
                foreach (var item in list)
                {
                    if (item.customer.UserID != db.Users.First(u => u.FirstName == user.FirstName && u.LastName == user.LastName).UserId)
                        actual = false;
                }

                Assert.True(actual);
            }
        }
        /// <summary>
        /// test UserIsInDB method
        /// </summary>
        [Fact]
        public void UserIsInDBWorks()
        {
            //setup inmemory database
            var options = new DbContextOptionsBuilder<ProjectsContext>()
                .UseInMemoryDatabase("user_is_in_db_test").Options;
            using (var db = new ProjectsContext(options))
            {
                //declare and instantiate repo, location, and user
                IPizzaShopRepo sut = new PizzaShopRepo(db);
                LocationClass location = new LocationClass("Test Location", new List<OrderClass>());
                UserClass user = new UserClass(0, "Test", "User", location);
                //add location and user to database
                sut.AddNewLocation(location);
                sut.SaveChanges();
                sut.AddNewUser(user);
                sut.SaveChanges();
                //call UserIsInDB
                bool actual = sut.UserIsInDB("Test", "User");
                //Assert that true was returned for user added to database
                Assert.True(actual);
                //call UserIsInDB
                actual = sut.UserIsInDB("Test", "User2");
                //Assert that false was returned for user not added to database
                Assert.False(actual);
            }
        }
        /// <summary>
        /// Test GetUserByName method
        /// </summary>
        [Fact]
        public void GetUserByNameWorks()
        {
            //setup inmemory database
            var options = new DbContextOptionsBuilder<ProjectsContext>()
                .UseInMemoryDatabase("get_user_by_name_test").Options;
            using (var db = new ProjectsContext(options))
            {
                //declare and instantiate repo, location, and user
                IPizzaShopRepo sut = new PizzaShopRepo(db);
                LocationClass location = new LocationClass("Test Location", new List<OrderClass>());
                UserClass user = new UserClass(0, "Test", "User", location);
                //add location and user to database
                sut.AddNewLocation(location);
                sut.SaveChanges();
                sut.AddNewUser(user);
                sut.SaveChanges();
                //callGetUserByName
                UserClass user2 = sut.GetUserByName("Test", "User");
                //check if returned user is the correct one
                bool actual = user2.FirstName == user.FirstName && user2.LastName == user.LastName;

                Assert.True(actual);
            }
        }
        /// <summary>
        /// Test GetLocationByDescription method
        /// </summary>
        [Fact]
        public void GetLocationByDescriptionWorks()
        {
            //setup inmemory database
            var options = new DbContextOptionsBuilder<ProjectsContext>()
                .UseInMemoryDatabase("get_location_by_description_test").Options;
            using (var db = new ProjectsContext(options))
            {
                //declare and instantiate repo and location
                IPizzaShopRepo sut = new PizzaShopRepo(db);
                LocationClass location = new LocationClass("Test Location", new List<OrderClass>());
                //add location to database
                sut.AddNewLocation(location);
                sut.SaveChanges();
                //call GetLocationByDescription
                location = sut.GetLocationByDescription(location.LocationDescription);
                //check if returned location is correct location
                bool actual = location.LocationDescription == "Test Location" && location.LocationID != 0;

                Assert.True(actual);
            }
        }
        /// <summary>
        /// Test UpdateLocation method
        /// </summary>
        [Fact]
        public void UpdateLocationWorks()
        {
            //setup inmemory database
            var options = new DbContextOptionsBuilder<ProjectsContext>()
                .UseInMemoryDatabase("update_location_test").Options;
            using (var db = new ProjectsContext(options))
            {
                //declare and instantiate repo and location
                IPizzaShopRepo sut = new PizzaShopRepo(db);
                LocationClass location = new LocationClass("Test Location", new List<OrderClass>());
                //add location to database
                sut.AddNewLocation(location);
                sut.SaveChanges();
                //Get Location from database to get LocationID
                location = sut.GetLocationByDescription(location.LocationDescription);
                //change one of the locations inventory values
                location.inventory[0] = 100;
                //call UpdateLocation
                sut.UpdateLocation(location);
                sut.SaveChanges();
                //get location from database to check for updated value
                IList<LocationClass> list = sut.GetAllLocations();
                //check if location in database reflects updated value
                bool actual = list.First(l => l.LocationDescription == location.LocationDescription).inventory[0] == 100;

                Assert.True(actual);
            }
        }
    }
}
