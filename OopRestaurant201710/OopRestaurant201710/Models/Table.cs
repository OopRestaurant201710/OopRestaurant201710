using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OopRestaurant201710.Models
{
    public class Table
    {
        /// <summary>
        /// Elsődleges kulcs mező (PK)
        /// az EF Code First ebből megcsinálja a db identity mezőt
        /// ami jó kulcsnak.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Az asztal azonosítása. pl: 23-as, jobb 12-es vagy A3-as
        /// </summary>
        /// 
        [Required] //két dolgot tesz: 1. az adatbázisban kötelező kitölteni 2. ViewModel-ként a felületen is kötelező kitölteni
        public string Name { get; set; }

        [Required] //lásd mint a Name property
        public Location Location { get; set; }

        /// <summary>
        /// ViewModel: a lenyílómező kiválasztott sora
        /// </summary>
        [NotMapped] //ezzel kikerül a Code First hatóköréből, csak mi használjuk az alkalmazásban, az adatbázisba nem kerül
        public int LocationId { get; set; }

        /// <summary>
        /// ViewModel: a lenyílómező tartalma
        /// </summary>
        [NotMapped] //ezzel kikerül a Code First hatóköréből, csak mi használjuk az alkalmazásban, az adatbázisba nem kerül
        public SelectList AssignableLocations { get; set; }


    }
}