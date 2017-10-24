using System.ComponentModel.DataAnnotations;

namespace OopRestaurant201710.Models
{
    /// <summary>
    /// Ez az osztály jelenti az éttermen belül az egyes helységeket.
    /// pl: kerthelyiség, belső terem, stb.
    /// </summary>
    public class Location
    {
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