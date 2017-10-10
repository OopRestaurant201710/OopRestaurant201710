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