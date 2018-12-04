using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaShopUserInterface
{
    public class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        Location DefaultLocation = new Location();
    }
}
