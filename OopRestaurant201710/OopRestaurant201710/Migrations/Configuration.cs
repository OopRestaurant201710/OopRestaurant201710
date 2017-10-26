//using OopRestaurant201710.Models; Ha kívülrõl határozom meg, akkor kell a "gyökér névtér"

namespace OopRestaurant201710.Migrations
{
    using Models; //mivel a "gyökér" közös, erre innen rálátunk, ezért csak a gyökértõl megkülönböztetõ útvonal kell
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<OopRestaurant201710.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "OopRestaurant201710.Models.ApplicationDbContext";
        }

        protected override void Seed(OopRestaurant201710.Models.ApplicationDbContext context)
        {
            context.Categories.AddOrUpdate(x => x.Name, new Category(name: "Pizzák"));
            context.Categories.AddOrUpdate(x => x.Name, new Category(name: "Italok"));
            context.Categories.AddOrUpdate(x => x.Name, new Category(name: "Desszertek"));
            context.SaveChanges();

            //LINQ
            var pizzaCategory = context.Categories
                                       .Single(x => x.Name == "Pizzák"); //azt az egyetlent keressük, amire ez igaz. Ha nincs: hiba, ha több van: hiba

            context.MenuItems.AddOrUpdate(x => x.Name, new MenuItem(name: "Margarita",
                description: "Mozzarella sajt, paradicsomszósz", price: 100, category: pizzaCategory));

            context.MenuItems.AddOrUpdate(x => x.Name, new MenuItem(name: "Hawaii",
                description: "Sonka, ananász, mozzarella sajt, paradicsomszósz", price: 100, category: pizzaCategory));
            context.SaveChanges();


            //Helyiségek feltöltése
            context.Locations.AddOrUpdate(x => x.Name, new Location() { Name = "Terasz", IsOutdoor = true }); //ha nem lenne megfelelõ konstruktor, így mindig használható 
            context.Locations.AddOrUpdate(x => x.Name, new Location("Belsõ terem", false)); //de az elõzõ ugyanaz csinálja mint ez, ha van ilyen konstruktor
            context.SaveChanges();

            var outdoorLocation = context.Locations
                                         .Where(x => x.Name == "Terasz") //Az összes sort visszaadja, amire ez igaz. Ha nincs: üres lista, ha több van: hosszabb lista
                                         //.Single(); ha ezt írnánk, akkor ugyanazt írnánk mint a pizzáknál
                                         //.First(); ha üres a lista: hiba, ha van elem a listán, akkor az elsõ elemt adja vissza.
                                         .FirstOrDefault(); //Ha üres a lista: null-t ad vissza, ha van elem a listán, akkor az elsõ elemet adja

            if (outdoorLocation == null)
            { //ha nincs location példány nem érdemes továbbmenni
                throw new Exception("Nincs megfelelõ Location az adatbázisban (Terasz)");
            }

            //Asztalok feltöltése
            context.Tables.AddOrUpdate(x => x.Name, new Table() { Name = "Bal-1 (t)", Location = outdoorLocation });
            context.Tables.AddOrUpdate(x => x.Name, new Table() { Name = "Bal-2 (t)", Location = outdoorLocation });
            context.Tables.AddOrUpdate(x => x.Name, new Table() { Name = "Jobb-1 (t)", Location = outdoorLocation });

            var indoorLocation = context.Locations
                                        .Where(x => x.Name == "Belsõ terem")
                                        .FirstOrDefault();

            if (indoorLocation == null)
            { //ha nincs location példány nem érdemes továbbmenni
                throw new Exception("Nincs megfelelõ Location az adatbázisban (Belsõ terem)");
            }

            context.Tables.AddOrUpdate(x => x.Name, new Table() { Name = "Bal-1 (b)", Location = indoorLocation });
            context.Tables.AddOrUpdate(x => x.Name, new Table() { Name = "Jobb-1 (b)", Location = indoorLocation });

            context.SaveChanges();
        }
    }
}
