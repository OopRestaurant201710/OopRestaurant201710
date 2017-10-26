using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OopRestaurant201710.Models
{
    /// <summary>
    /// Ez az osztály jelenti az éttermen belül az egyes helységeket.
    /// pl: kerthelyiség, belső terem, stb.
    /// </summary>
    public class Location
    {
        /// <summary>
        /// Alapértelmezett paraméter nélküli konstruktor az 
        /// EntityFramework-nek
        /// </summary>
        public Location() { }

        /// <summary>
        /// Az a konstruktor, ami a használhatóságot javítja
        /// </summary>
        /// <param name="name"></param>
        /// <param name="isOutdoor"></param>
        public Location(string name, bool isOutdoor)
        {
            Name = name;
            IsOutdoor = isOutdoor;
        }

        /// <summary>
        /// PK mező az adatbázishoz
        /// </summary>
        public int Id { get; set; }

        [Required]
        [Display(Name="Megnevezés")]
        public string Name { get; set; }

        /// <summary>
        /// Jelzi, hogy a terem/terület kültéri-e?
        /// true, ha kültéri, false, ha beltéri
        /// </summary>
        [Display(Name="A szabadban van?")]
        public bool IsOutdoor { get; set; }

        //felhasználva az Asztalból (Table.cs) ide irányuló kapcsolatot, visszafelé is
        //ide azokat az asztalokat várom, ami ehhez a teremhez lett rögzítve.
        //mivel kivettük a NotMapped-ed, ezért 
        //
        // 1. a Code First fennhatósága alá kerül.
        // 2. Mivel ide irányba az asztaltól mutat kapcsolat (minden asztal (Table) egy adott teremhez tartozik (Location)
        // 3. ezért a teremhez visszafelé ki lehet gyűjteni a hozzá tartozó asztalokat.
        //
        // Ezzel a property-vel ezt a létező kapcsolatot felhasználva elérhetővé tesszük az asztalokat
        // amik a teremhez tartoznak.
        public List<Table> Tables { get; set; }
    }
}