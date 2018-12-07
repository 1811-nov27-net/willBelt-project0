using PizzaShopUserInterface;
using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaShop.DataAccess
{
    public interface IPizzaShopRepo
    {
        IList<LocationClass> GetAllLocations();
        IList<UserClass> GetAllUsers();
        IList<OrderClass> GetAllOrders();
        void CreateOrder(UserClass user, LocationClass location);
        IList<OrderClass> GetOrdersByUser(UserClass user);
        void SaveChanges();
        void AddNewLocation(LocationClass location);
        void AddNewUser(UserClass user);
    }
}
