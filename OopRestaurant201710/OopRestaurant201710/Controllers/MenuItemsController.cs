using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OopRestaurant201710;
using OopRestaurant201710.Models;

namespace OopRestaurant201710.Controllers
{
    public class MenuItemsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: MenuItems
        public ActionResult Index()
        {
            return View(db.MenuItems.Include(x=>x.Category).ToList());
        }

        // GET: MenuItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MenuItem menuItem = db.MenuItems.Find(id);
            if (menuItem == null)
            {
                return HttpNotFound();
            }
            return View(menuItem);
        }

        // GET: MenuItems/Create
        [Authorize(Roles = "Fopincer,Admin")] //Csak a Főpincér és az Admin csoport tagjai használhatják ezt az Action-t
        public ActionResult Create()
        {
            var menuItem = new MenuItem();
            //todo: ezt az adatbetöltést csináljuk meg jól!
            LoadAssignableCategories(menuItem);
            return View(menuItem);
        }

        // POST: MenuItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Fopincer,Admin")] //Csak a Főpincér és az Admin csoport tagjai használhatják ezt az Action-t
        public ActionResult Create([Bind(Include = "Id,Name,Description,Price,CategoryId")] MenuItem menuItem)
        {

            var category = db.Categories.Find(menuItem.CategoryId);
            if (category == null)
            { //Ha nincs ilyen kategóriám, akkor nem tudok mit tenni, 
              //visszaküldöm az adatokat módosításra
                LoadAssignableCategories(menuItem);
                return View(menuItem);
            }

            db.MenuItems.Attach(menuItem);
            //mivel ez egy vadonatúj elem, ami még nem volt adatbázisban,
            //ezért nem tudunk property-t tölteni, mert nincs honnan.
            //ezért az Edit-tel ellentétben ez a sor nem kell.
            //db.Entry(menuItem).Reference(x => x.Category).Load();
            menuItem.Category = category;
            
            //Újra kell az adatok ellenőrzését végezni, hiszen 
            //megmódosítottam az egyes property-ket
            ModelState.Clear(); //előző törlése
            TryValidateModel(menuItem); //újra validálás

            if (ModelState.IsValid)
            {
                db.MenuItems.Add(menuItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            LoadAssignableCategories(menuItem);
            return View(menuItem);
        }

        private void LoadAssignableCategories(MenuItem menuItem)
        {
            menuItem.AssignableCategories = new SelectList(db.Categories.OrderBy(x => x.Name).ToList(), "Id", "Name");
        }

        // GET: MenuItems/Edit/5
        //Ezt a megoldást nem használjuk, mert minden új felhasználónál 
        //hozzá kell nyúlni a kódhoz és újrafordítani/telepíteni
        //[Authorize(Users = "gabor.plesz@gmail.com")] 
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MenuItem menuItem = db.MenuItems.Find(id);
            if (menuItem == null)
            {
                return HttpNotFound();
            }

            //Ahhoz, hogy legyen, be kell töltenünk a menuItem Category property-jét,
            //amit magától az EF (EntityFramework) nem tölt be
            db.Entry(menuItem)
              .Reference(x => x.Category)
              .Load();

            //hogy be tudjuk állítani a lenyílót, megadjuk az aktuális Category azonosítóját
            menuItem.CategoryId = menuItem.Category.Id;
            //leküldjük a Categories tábla tartalmát (db.Categories.ToList())
            //megadjuk, hogy melyik mező azonosítja a sort, és adja azt az értéket, ami a végeredmény (Id),
            //megadjuk, hogy a lenyíló egyes soraiba, melyik property értékei kerüljenek (Name)
            LoadAssignableCategories(menuItem);

            return View(menuItem);
        }

        // POST: MenuItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken] //Be kell engedni a lenyíló által kiválasztott azonosítót
        [Authorize]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,Price,CategoryId")] MenuItem menuItem) 
        {
            var category = db.Categories.Find(menuItem.CategoryId);

            if (category == null)
            { //Ha nincs ilyen kategóriám, akkor nem tudok mit tenni, 
              //visszaküldöm az adatokat módosításra
                LoadAssignableCategories(menuItem);
                return View(menuItem);
            }

            //a html form-ról jövő adatokat "bemutatjuk" az adatbázisnak
            db.MenuItems.Attach(menuItem);

            //az adatbázissal kapcsolatos dolgok eléréséhez kell az Entry
            var entry = db.Entry(menuItem);

            //ennek segítségével betöltjük a Category tábla adatait a menuItem.Category property-be
            entry.Reference(x => x.Category).Load();

            //majd felülírjuk azzal, ami bejött a HTML form-on
            menuItem.Category = category;

            //Módosítás után az adatellenőrzést el kell végezni
            ModelState.Clear();
            TryValidateModel(menuItem);

            if (ModelState.IsValid)
            {
                entry.State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            LoadAssignableCategories(menuItem);
            return View(menuItem);
        }

        // GET: MenuItems/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MenuItem menuItem = db.MenuItems.Find(id);
            if (menuItem == null)
            {
                return HttpNotFound();
            }
            return View(menuItem);
        }

        // POST: MenuItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            MenuItem menuItem = db.MenuItems.Find(id);
            db.MenuItems.Remove(menuItem);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
