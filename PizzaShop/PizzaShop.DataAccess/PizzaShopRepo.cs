using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using PizzaShop.Library;

namespace PizzaShop.DataAccess
{
    public class PizzaShopRepo : IPizzaShopRepo
    {
        public ProjectsContext db { get; }

        public PizzaShopRepo(ProjectsContext Db)
        {
            db = Db ?? throw new ArgumentNullException(nameof(Db));
        }

        public void AddNewLocation(LocationClass location)
        {
            Locations trackedLocation = new Locations();
            trackedLocation.LocationDescription = location.LocationDescription;
            trackedLocation.Menu = location.GetMenu();
            trackedLocation.Inventory = location.GetInventory();
            db.Add(trackedLocation);
        }

        public void AddNewUser(UserClass user)
        {
            Users trackedUser = new Users();
            trackedUser.FirstName = user.FirstName;
            trackedUser.LastName = user.LastName;
            trackedUser.DefaultLocation = db.Locations.First(l => l.LocationDescription == user.DefaultLocation.LocationDescription).LocationId;
            db.Add(trackedUser);
        }

        public void CreateOrder(OrderClass order)
        {
            Orders trackedOrder = new Orders();
            if (db.Users.Where(u => u.FirstName == order.customer.FirstName && u.LastName == order.customer.LastName).ToList().Count == 0)
            {
                AddNewUser(order.customer);
            }
            trackedOrder.UserId = db.Users.First(u => u.FirstName == order.customer.FirstName && u.LastName == order.customer.LastName).UserId;
            trackedOrder.LocationId = db.Locations.First(u => u.LocationDescription == order.location.LocationDescription).LocationId;
            trackedOrder.TotalCost = order.total;
            bool firstLoop = true;
            foreach(var pizza in order.pizzas)
            {
                if(!firstLoop)
                    trackedOrder.OrderDescription += "/";
                trackedOrder.OrderDescription += $"{pizza.size},{pizza.crustSelection},";
                foreach(var topping in pizza.toppingSelection)
                {
                    if (topping)
                        trackedOrder.OrderDescription += "1";
                    else
                        trackedOrder.OrderDescription += "0";
                }
                if (firstLoop)
                    firstLoop = false;
            }
            trackedOrder.Time = order.time;
            db.Orders.Add(trackedOrder);
        }

        public IList<LocationClass> GetAllLocations()
        {
            IList<LocationClass> locationList = new List<LocationClass>();
            foreach (var location in db.Locations.ToList())
            {
                var temporary = BuildLocationFromDBLocations(location);
                BuildLocationOrderHistory(temporary);
                locationList.Add(temporary);
            }
            return locationList;
        }

        public IList<OrderClass> GetAllOrders()
        {
            IList<OrderClass> orderList = new List<OrderClass>();
            foreach (var order in db.Orders.ToList())
            {
                orderList.Add(BuildOrderFromDBOrder(order));
            }
            return orderList;
        }

        public IList<UserClass> GetAllUsers()
        {
            IList<UserClass> userList = new List<UserClass>();
            foreach(var user in db.Users.ToList())
            {
                userList.Add(BuildUserFromDBUser(user));
            }
            return userList;
        }

        public IList<OrderClass> GetOrdersByUser(UserClass user)
        {
            Users DBUser = db.Users.First(u => u.FirstName == user.FirstName && u.LastName == user.LastName);
            IList<OrderClass> orderList = new List<OrderClass>();
            foreach (var order in db.Orders.Where(o => o.UserId == DBUser.UserId).ToList())
            {
                orderList.Add(BuildOrderFromDBOrder(order));
            }
            return orderList;
        }

        public void SaveChanges()
        {
            db.SaveChanges();
        }

        public bool UserIsInDB(string firstName, string lastName)
        {
            return db.Users.Where(u => u.FirstName == firstName && u.LastName == lastName).ToList().Count == 1;
        }

        public UserClass GetUserByName(string firstName, string lastName)
        {
            return BuildUserFromDBUser(db.Users.First(u => u.FirstName == firstName && u.LastName == lastName));
        }

        private LocationClass BuildLocationFromDBLocations(Locations location)
        {
            List<OrderClass> history = new List<OrderClass>();
            return new LocationClass(location.LocationId, location.LocationDescription, history, location.Menu, location.Inventory);
        }

        public void BuildLocationOrderHistory(LocationClass location)
        {
            foreach (var order in db.Orders.Where(o => o.LocationId == location.LocationID).ToList())
            {
                location.OrderHistory.Add(BuildOrderFromDBOrder(order));
            }
        }

        private OrderClass BuildOrderFromDBOrder(Orders order)
        {
            OrderClass newOrder = new OrderClass(order.OrderId, BuildUserFromDBUser(db.Users.Find(order.UserId)), BuildLocationFromDBLocations(db.Locations.Find(order.LocationId)));
            newOrder.time = order.Time;
            newOrder.total = order.TotalCost;
            foreach (var pizza in order.OrderDescription.Split('/'))
            {
                string[] pizzaSubStrings = pizza.Split(',');
                if (pizzaSubStrings.Length == 3)
                {
                    int size, crust, topping;
                    bool[] toppingChoices = new bool[newOrder.location.toppings.Count];
                    int.TryParse(pizzaSubStrings[0], out size);
                    int.TryParse(pizzaSubStrings[1], out crust);
                    for (int i = 0; i < pizzaSubStrings[2].Length; i++)
                    {
                        string temp = "";
                        temp += pizzaSubStrings[2][i];
                        int.TryParse(temp, out topping);
                        toppingChoices[i] = topping == 1;
                    }
                    newOrder.pizzas.Add(
                        new PizzaClass
                        (
                            newOrder.location.sizes,
                            newOrder.location.crustTypes,
                            newOrder.location.toppings,
                            size,
                            crust,
                            toppingChoices
                            ));
                }
            }
            return newOrder;
        }

        private UserClass BuildUserFromDBUser(Users user)
        {
            return new UserClass(
                user.UserId,
                user.FirstName,
                user.LastName,
                BuildLocationFromDBLocations(db.Locations.Find(user.DefaultLocation))
                );

        }

        public void UpdateLocation(LocationClass location)
        {
            var trackedLocation = db.Locations.Find(location.LocationID);
            trackedLocation.Inventory = location.GetInventory();
            trackedLocation.Menu = location.GetMenu();
            db.Locations.Update(trackedLocation);
        }
    }
}
