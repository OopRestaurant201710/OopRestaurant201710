//using OopRestaurant201710.Models; Ha k�v�lr�l hat�rozom meg, akkor kell a "gy�k�r n�vt�r"

namespace OopRestaurant201710.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models; //mivel a "gy�k�r" k�z�s, erre innen r�l�tunk, ez�rt csak a gy�k�rt�l megk�l�nb�ztet� �tvonal kell
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
            context.Categories.AddOrUpdate(x => x.Name, new Category(name: "Pizz�k"));
            context.Categories.AddOrUpdate(x => x.Name, new Category(name: "Italok"));
            context.Categories.AddOrUpdate(x => x.Name, new Category(name: "Desszertek"));
            context.SaveChanges();

            //LINQ
            var pizzaCategory = context.Categories
                                       .Single(x => x.Name == "Pizz�k"); //azt az egyetlent keress�k, amire ez igaz. Ha nincs: hiba, ha t�bb van: hiba

            context.MenuItems.AddOrUpdate(x => x.Name, new MenuItem(name: "Margarita",
                description: "Mozzarella sajt, paradicsomsz�sz", price: 100, category: pizzaCategory));

            context.MenuItems.AddOrUpdate(x => x.Name, new MenuItem(name: "Hawaii",
                description: "Sonka, anan�sz, mozzarella sajt, paradicsomsz�sz", price: 100, category: pizzaCategory));
            context.SaveChanges();


            //Helyis�gek felt�lt�se
            context.Locations.AddOrUpdate(x => x.Name, new Location() { Name = "Terasz", IsOutdoor = true }); //ha nem lenne megfelel� konstruktor, �gy mindig haszn�lhat� 
            context.Locations.AddOrUpdate(x => x.Name, new Location("Bels� terem", false)); //de az el�z� ugyanaz csin�lja mint ez, ha van ilyen konstruktor
            context.SaveChanges();

            var outdoorLocation = context.Locations
                                         .Where(x => x.Name == "Terasz") //Az �sszes sort visszaadja, amire ez igaz. Ha nincs: �res lista, ha t�bb van: hosszabb lista
                                                                         //.Single(); ha ezt �rn�nk, akkor ugyanazt �rn�nk mint a pizz�kn�l
                                                                         //.First(); ha �res a lista: hiba, ha van elem a list�n, akkor az els� elemt adja vissza.
                                         .FirstOrDefault(); //Ha �res a lista: null-t ad vissza, ha van elem a list�n, akkor az els� elemet adja

            if (outdoorLocation == null)
            { //ha nincs location p�ld�ny nem �rdemes tov�bbmenni
                throw new Exception("Nincs megfelel� Location az adatb�zisban (Terasz)");
            }

            //Asztalok felt�lt�se
            context.Tables.AddOrUpdate(x => x.Name, new Table() { Name = "Bal-1 (t)", Location = outdoorLocation });
            context.Tables.AddOrUpdate(x => x.Name, new Table() { Name = "Bal-2 (t)", Location = outdoorLocation });
            context.Tables.AddOrUpdate(x => x.Name, new Table() { Name = "Jobb-1 (t)", Location = outdoorLocation });

            var indoorLocation = context.Locations
                                        .Where(x => x.Name == "Bels� terem")
                                        .FirstOrDefault();

            if (indoorLocation == null)
            { //ha nincs location p�ld�ny nem �rdemes tov�bbmenni
                throw new Exception("Nincs megfelel� Location az adatb�zisban (Bels� terem)");
            }

            context.Tables.AddOrUpdate(x => x.Name, new Table() { Name = "Bal-1 (b)", Location = indoorLocation });
            context.Tables.AddOrUpdate(x => x.Name, new Table() { Name = "Jobb-1 (b)", Location = indoorLocation });

            context.SaveChanges();

            //Csoportok felvitele: Admin, Pincer, Fopincer
            AddRoleIfNotExists(context, "Admin");
            AddRoleIfNotExists(context, "Pincer");
            AddRoleIfNotExists(context, "Fopincer");

            //Felhaszn�l�k felvitele: admin@netacademia.hu, pincer@netacademia.hu, fopincer@netacademia.hu

            AddUserIfNotExists(context, "admin@netacademia.hu", "123456aA#", "Admin");
            AddUserIfNotExists(context, "pincer@netacademia.hu", "123456aA#", "Pincer");
            AddUserIfNotExists(context, "fopincer@netacademia.hu", "123456aA#", "Fopincer");

        }

        private static void AddUserIfNotExists(ApplicationDbContext context, string email, string pw, string role)
        {
            if (!context.Users.Any(x => x.Email == email))
            { //ha m�g nincs ilyen felhaszn�l�, akkor l�trehozzuk

                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);

                var user = new ApplicationUser()
                {
                    Email = email,
                    UserName = email //ez a kett� megegyezik, en�lk�l nem m�k�dik!!!
                };

                //elmentj�k a felhaszn�l�t az Identity adatb�zis�ba
                manager.Create(user, pw);

                //a felhaszn�l�t hozz�adjuk a megfelel� csoporthoz.
                manager.AddToRole(user.Id, role);

            }
        }

        private static void AddRoleIfNotExists(ApplicationDbContext context, string roleName)
        {
            if (!context.Roles.Any(x => x.Name == roleName))
            { //ha m�g nincs ilyen csoport, akkor l�trehozzuk
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);

                var role = new IdentityRole(roleName);
                manager.Create(role);
            }
        }
    }
}
