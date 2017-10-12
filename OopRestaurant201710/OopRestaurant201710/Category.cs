using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;

namespace OopRestaurant201710
{
    /// <summary>
    /// Az ételek kategóriáját tartalmazó osztály
    /// </summary>
    public class Category
    {

        /// <summary>
        /// ez az ugy nevezett Konstruktor. Ez gyártja le a
        /// new kulcsszó után az osztályom egy-egy új példányát.
        /// 
        /// Figyelem: mivel implementáltam saját konstruktort, ezért 
        /// a fordító nem készít alapértelmezett paraméter nélküli konstruktort
        /// </summary>
        /// <param name="name">a name paraméter a kategória nevét tartalmazza</param>
        public Category(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Ez az un. alapértelmezett paraméter nélküli konstruktor
        /// Ezt a fordító legyártja mindig, kivéve, ha az osztálynak van
        /// implementálva konstruktor.
        /// mivel az előbb készítettem olyan konstruktort, aminek van paramétere,
        /// ezért a fordító már nem gyárt.
        /// 
        /// És azért kell ilyet csinálni, mert a Entity Framework 
        /// használatához minden adatelérési osztályhoz kell.
        /// </summary>
        public Category() { }

        /// <summary>
        /// Az adatbázisba írás miatt kell Primary Key (PK) a táblába
        /// ezt hozza létre ez a property. Mivel Id a neve a Code first 
        /// automatikusan tudja, hogy ő PK
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Az étel kategória neve
        /// </summary>
        public string Name { get; set; }
    }
}