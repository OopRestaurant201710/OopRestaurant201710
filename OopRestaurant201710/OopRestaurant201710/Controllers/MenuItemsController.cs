﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using OopRestaurant201710.Models;

namespace OopRestaurant201710.Controllers
{
    public class MenuItemsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Ez a függvény felelős a model betöltéséért minden megjelenítő (get) action esetén
        /// </summary>
        /// <param name="id">MenuItem azonosító, lehet null is.</param>
        /// <param name="op">Művelet: Read vagy New</param>
        /// <returns></returns>
        private MenuItem ReadOrNewMenuItem(int? id, ReadOrNewOperation op)
        {

            MenuItem menuItem;

            switch (op) //Az op változó értékétől függ, hogy merre megyünk tovább
            {
                case ReadOrNewOperation.Read: //ha Read az érték akkor erre
                    //1. Adatok betöltése az Adatbázisból (Model)
                    /////////////////////////////////////////////
                    menuItem = db.MenuItems.Find(id);

                    if (menuItem == null)
                    {
                        //return menuItem; //ez ugyanaz:
                        return null;
                    }

                    //Ahhoz, hogy legyen, be kell töltenünk a menuItem Category property-jét,
                    //amit magától az EF (EntityFramework) nem tölt be
                    db.Entry(menuItem)
                      .Reference(x => x.Category)
                      .Load();
                    break;

                case ReadOrNewOperation.New: //ha new az érték akkor erre
                    //1. Adat (model) példányosítása "röptében"
                    menuItem = new MenuItem();
                    break;

                default: //Ha egyik sem igaz a fentiek közül akkor van ez
                    throw new Exception($"Erre nem készültünk fel: {op}");
            }


            //2. Megjelenítési adatok feltöltése (ViewModel)
            //hogy be tudjuk állítani a lenyílót, megadjuk az aktuális Category azonosítóját

            if (menuItem.Category!=null)
            {
                menuItem.CategoryId = menuItem.Category.Id;
            }
            //leküldjük a Categories tábla tartalmát (db.Categories.ToList())
            //megadjuk, hogy melyik mező azonosítja a sort, és adja azt az értéket, ami a végeredmény (Id),
            //megadjuk, hogy a lenyíló egyes soraiba, melyik property értékei kerüljenek (Name)
            LoadAssignableCategories(menuItem);

            return menuItem;
        }

        private void CreateUpdateOrDeleteMenuItem(MenuItem menuItem, CreateUpdateOrDeleteOperation op)
        {
            switch (op)
            {
                case CreateUpdateOrDeleteOperation.Create:
                    var categoryCreate = db.Categories.Find(menuItem.CategoryId);
                    //todo: ezt a részt vissza integrálni
                    //if (category == null)
                    //{ //Ha nincs ilyen kategóriám, akkor nem tudok mit tenni, 
                    //  //visszaküldöm az adatokat módosításra
                    //    LoadAssignableCategories(menuItem);
                    //    return View(menuItem);
                    //}

                    db.MenuItems.Attach(menuItem);
                    //mivel ez egy vadonatúj elem, ami még nem volt adatbázisban,
                    //ezért nem tudunk property-t tölteni, mert nincs honnan.
                    //ezért az Edit-tel ellentétben ez a sor nem kell.
                    //db.Entry(menuItem).Reference(x => x.Category).Load();
                    menuItem.Category = categoryCreate;
                    return;

                case CreateUpdateOrDeleteOperation.Update:
                    var categoryUpdate = db.Categories.Find(menuItem.CategoryId);

                    //todo: ezt a részt vissza kell integrálni
                    //if (category == null)
                    //{ //Ha nincs ilyen kategóriám, akkor nem tudok mit tenni, 
                    //  //visszaküldöm az adatokat módosításra
                    //    LoadAssignableCategories(menuItem);
                    //    return View(menuItem);
                    //}

                    //a html form-ról jövő adatokat "bemutatjuk" az adatbázisnak
                    db.MenuItems.Attach(menuItem);

                    //az adatbázissal kapcsolatos dolgok eléréséhez kell az Entry
                    var entry = db.Entry(menuItem);

                    //ennek segítségével betöltjük a Category tábla adatait a menuItem.Category property-be
                    entry.Reference(x => x.Category).Load();

                    //majd felülírjuk azzal, ami bejött a HTML form-on
                    menuItem.Category = categoryUpdate;
                    entry.State = EntityState.Modified;

                    return;

                case CreateUpdateOrDeleteOperation.Delete:
                    db.MenuItems.Remove(menuItem);
                    return;

                default:
                    throw new Exception($"Erre nem vagyunk felkészülve: {op}");
            }
        }

        private void LoadAssignableCategories(MenuItem menuItem)
        {
            menuItem.AssignableCategories = new SelectList(db.Categories.OrderBy(x => x.Name).ToList(), "Id", "Name");
        }

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
            MenuItem menuItem = ReadOrNewMenuItem(id, ReadOrNewOperation.Read);
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
            var menuItem = ReadOrNewMenuItem(null, ReadOrNewOperation.New);
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

            CreateUpdateOrDeleteMenuItem(menuItem, CreateUpdateOrDeleteOperation.Create);

            //Újra kell az adatok ellenőrzését végezni, hiszen 
            //megmódosítottam az egyes property-ket
            ModelState.Clear(); //előző törlése
            TryValidateModel(menuItem); //újra validálás

            if (ModelState.IsValid)
            {
                //todo: házi feladat
                db.MenuItems.Add(menuItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            LoadAssignableCategories(menuItem);
            return View(menuItem);
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

            MenuItem menuItem = ReadOrNewMenuItem(id, ReadOrNewOperation.Read);
            if (menuItem == null)
            {
                return HttpNotFound();
            }

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
            CreateUpdateOrDeleteMenuItem(menuItem, CreateUpdateOrDeleteOperation.Update);

            //Módosítás után az adatellenőrzést el kell végezni
            ModelState.Clear();
            TryValidateModel(menuItem);

            if (ModelState.IsValid)
            {
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
            MenuItem menuItem = ReadOrNewMenuItem(id, ReadOrNewOperation.Read);
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
            MenuItem menuItem = ReadOrNewMenuItem(id, ReadOrNewOperation.Read);
            CreateUpdateOrDeleteMenuItem(menuItem, CreateUpdateOrDeleteOperation.Delete);

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
