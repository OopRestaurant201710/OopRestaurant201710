using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;

namespace OopRestaurant201710
{
    /// <summary>
    /// Az étlapon szereplő tételek közül egy tétel adatait tartalmazó osztály
    /// </summary>
    public class MenuItem
    {
        /// <summary>
        /// Az EntityFramework részére legyártjuk a
        /// paraméter nélküli konstruktort, amit
        /// a fordító már nem csinál meg nekünk
        /// </summary>
        public MenuItem() {}

        public MenuItem(string name, string description, int price, Category category)
        {
            Name = name;
            Description = description;
            Price = price;
            Category = category;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Price { get; set; }

        public Category Category { get; set; }
    }
}