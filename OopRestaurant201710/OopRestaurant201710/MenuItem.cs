using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

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

        [Required(ErrorMessage = "Ezt a mezőt kötelező kitölteni")] //Ezt a mezőt kötelező kitölteni
        [Display(Name="Név")]
        public string Name { get; set; }

        [Required] //Ezt a mezőt kötelező kitölteni
        [Display(Name = "Leírás")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Range(1, 100000)]
        [Display(Name = "Ár")]
        public int Price { get; set; }

        [Required] //Ezt a mezőt kötelező kitölteni
        [Display(Name = "Kategória")]
        public Category Category { get; set; }

        /// <summary>
        /// A lenyíló lista kiválasztott elemének az azonosítója részére
        /// </summary>
        [NotMapped]
        public int CategoryId { get; set; }

        /// <summary>
        /// A lenyíló lista tartalma: azonosító és megjelenítendő szöveg párok.
        /// </summary>
        [NotMapped]
        public SelectList AssignableCategories { get; set; }
    }
}