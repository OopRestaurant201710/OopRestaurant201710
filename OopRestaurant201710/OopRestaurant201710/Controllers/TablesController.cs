using System;
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
    public class TablesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private Table ReadOrNewTable(int? id, ReadOrNewOperation op)
        {
            Table table;

            switch (op)
            {
                case ReadOrNewOperation.Read:
                    table = db.Tables.Find(id);
                    if (table == null)
                    {
                        return null;
                    }

                    //szólni kell az Entity Frameworknek, hogy 
                    //az asztalhoz töltse be a termet is, ahova rögzítettük.
                    db.Entry(table)
                      .Reference(x => x.Location)
                      .Load();

                    break;
                case ReadOrNewOperation.New:
                    table = new Table();
                    break;
                default:
                    throw new Exception($"Erre a műveletre nem vagyunk felkészítve: {op}");
            }

            //A lenyílómező adatainak kitöltése
            // ?: feltételes null operátor, 
            //      ha eddig a kifejezés értéke null, akkor megáll és a végeredmény null
            //      ha pedig nem null, akkor folytatódik a kiértékelés, és megy tovább 

            //ugyanaz, mint ez:
            //int? eredmeny;
            //if (table.Location == null)
            //{
            //    eredmeny = null;
            //}
            //else
            //{
            //    eredmeny = table.Location.Id;
            //}

            // ??: null operátor, ha a bal oldalán szereplő érték null, akkor az eredmény a jobb oldalán szereplő érték
            //ugyanaz, mint ez
            //if (eredmeny == null)
            //{
            //    eredmeny = 0;
            //}

            table.LocationId = table.Location?.Id ?? 0;
            LoadAssignableLocations(table);

            return table;
        }

        private void LoadAssignableLocations(Table table)
        {
            table.AssignableLocations = new SelectList(db.Locations.ToList(), "Id", "Name");
        }

        private void CreateUpdateOrDeleteTable(Table table, CreateUpdateOrDeleteOperation op)
        {
            switch (op)
            {
                case CreateUpdateOrDeleteOperation.Create:
                    //a kiválasztott termet be kell állítani
                    table.Location = db.Locations.Find(table.LocationId);
                    db.Tables.Add(table);
                    break;
                case CreateUpdateOrDeleteOperation.Update:
                    //todo: a teremhez be kell állítani a location-t

                    //1. be kell mutatni a model-t az adatbázisnak
                    db.Tables.Attach(table);

                    //2. be kell tölteni a hozzátartozó eredeti teremadatokat
                    db.Entry(table)                //kérem az EF adatbázist elérő részét
                      .Reference(x => x.Location)  //kérem a csatlakozó táblák közül a Location-t
                      .Load();                     //Onnan betöltöm az adatokat

                    //3. módosítani kell az új terem adatot
                    table.Location = db.Locations.Find(table.LocationId);

                    //4. Jelezni kell, hogy változott, így a többi érték (Name, stb.) változást is figyelmbe veszi az EF
                    db.Entry(table).State = EntityState.Modified;

                    //todo: a kiválasztott termet be kell állítani
                    break;
                case CreateUpdateOrDeleteOperation.Delete:
                    db.Tables.Remove(table);
                    break;
                default:
                    throw new Exception($"Erre a műveletre nem vagyunk felkészítve: {op}");
            }
        }

        // GET: Tables
        public ActionResult Index()
        {

            //Betöltöm a termek listáját, és megkérem az
            //Entity Framework-öt, hogy tegye hozzá a 
            //teremhez tartozó asztalok listáját
            var locations = db.Locations
                              .Include(x=>x.Tables)
                              .ToList();

            //majd ezt elküldjük a nézethez
            return View(locations);
        }

        // GET: Tables/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Table table = ReadOrNewTable(id, ReadOrNewOperation.Read);
            if (table == null)
            {
                return HttpNotFound();
            }
            return View(table);
        }


        // GET: Tables/Create
        public ActionResult Create()
        {
            var table = ReadOrNewTable(null, ReadOrNewOperation.New);
            return View(table);
        }

        // POST: Tables/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,LocationId")] Table table)
        {
            CreateUpdateOrDeleteTable(table, CreateUpdateOrDeleteOperation.Create);

            ModelState.Clear();
            TryValidateModel(table);

            if (ModelState.IsValid)
            {
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            LoadAssignableLocations(table);
            return View(table);
        }

        // GET: Tables/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Table table = ReadOrNewTable(id, ReadOrNewOperation.Read);
            if (table == null)
            {
                return HttpNotFound();
            }
            return View(table);
        }

        // POST: Tables/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,LocationId")] Table table)
        {
            CreateUpdateOrDeleteTable(table, CreateUpdateOrDeleteOperation.Update);

            ModelState.Clear();
            TryValidateModel(table);

            if (ModelState.IsValid)
            {
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            LoadAssignableLocations(table);
            return View(table);
        }

        // GET: Tables/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Table table = ReadOrNewTable(id, ReadOrNewOperation.Read);
            if (table == null)
            {
                return HttpNotFound();
            }
            return View(table);
        }

        // POST: Tables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Table table = db.Tables.Find(id);
            CreateUpdateOrDeleteTable(table, CreateUpdateOrDeleteOperation.Delete);
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
