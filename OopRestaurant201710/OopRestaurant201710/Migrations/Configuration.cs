namespace OopRestaurant201710.Migrations
{
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

            var pizzaCategory = context.Categories.Single(x => x.Name == "Pizz�k");

            context.MenuItems.AddOrUpdate(x => x.Name, new MenuItem(name: "Margarita",
                description: "Mozzarella sajt, paradicsomsz�sz", price: 100, category: pizzaCategory));

            context.MenuItems.AddOrUpdate(x => x.Name, new MenuItem(name: "Hawaii",
                description: "Sonka, anan�sz, mozzarella sajt, paradicsomsz�sz", price: 100, category: pizzaCategory));
            context.SaveChanges();
        }
    }
}
