﻿using System.ComponentModel.DataAnnotations;

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
        public string Name { get; set; }

        /// <summary>
        /// Jelzi, hogy a terem/terület kültéri-e?
        /// true, ha kültéri, false, ha beltéri
        /// </summary>
        public bool IsOutdoor { get; set; }

    }
}