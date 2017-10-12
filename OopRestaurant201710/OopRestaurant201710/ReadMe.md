
# Ismétlés (http/html/MVC)

```
 +------------------------------+    +---------------------------+
 | Internet böngésző            |    | ASP.NET MVC webalkalmazás |
 |------------------------------|    |---------------------------|
 |                              |    |                           |
 | +-------------------------+  |http|  +------------+           |
 | |  HTML állomány a válasz |  |+-->|+->Útválasztás |+----+     |
 | |                         |  |    |  +------------+     v     |
 | |                         |<-+<--+| +--------+ +------------+ |
 | |                         |  |    | |Layout  | |            | |
 | |                         |  |    <-++       | | Controller
 | |                         |  |    | |View    <-+ + Action   | |
 | +-------------------------+  |    | |        | |            | |
 |                              |    | +--------+ +------------+ |
 +------------------------------+    +---------------------------+
```

# Az étterem projekt leírása (specifikáció)

## Képernyőképek
képernyőképeket (egyelőre) nem készítünk, mert az MVC template-ek elkészítik nekünk a képernyőket, azt nem tudjuk egyszerűen befolyásolni. Így amit kapunk azt fogjuk használni.

## Szereplők
## Érdeklődő
## Étlap
### Példa étlap

- Pizzák
------
    - Margarita pizza 200 Ft
      mozzarella sajt, pizzaszósz
    - Hawaii pizza    300 Ft
      sonka, ananász, mozzarella sajt, pizzaszósz

- Italok
------
    - Ásványvíz 100 Ft (3dl)
    - Cola      120 Ft (3dl)

## Forgatókönyvek
### Érdeklődő eldönti, hogy akar-e nálunk enni?
Érdeklődő elkéri az étlapot és megnézi, hogy mit lehet nálunk enni, mennyiért.


# Code First Migration
## kell hozzá:

- EntityFramework nuget csomag
- a Code First Migration engedélyezése: a *Package Manager Console*-ból: **enable-migration**
- ezzel létrejön egy Migrations könyvtár, ez alatt egy Configuration.cs állomány
- ha már létezik adatbázis, akkor automatikusan létrejön ide az első *MigrationStep* (XXX_InitialCreate névvel)
- ha nincs első migrációs lépés, akkor kézzel készítünk a Package Manager Console-ból: **Add-Migration 'InitialCreate'**
- végül létrehozzuk az adatbázist *Package Manager Console*-ból: **Update-Database**

Az utolsó három lépés az **ASP.NET Identity** (bejelentkeztető és jogosultságkezelő modul, Login és Register menüpont) miatt kell, különben nem kéne.

az adatbázist pedig az **SQL Server Object Explorer* ablakban látjuk a **localdb** csomópont alatt. Lehet, hogy frissítenünk kell.

```
                 +---------------+              +------------------+
                 | Add-Migration |              | Update-Database  |
                           +------------+       +------------------+
   +------------------+    | MigrStep 1 |                                   +--------------+    +
   | Model            |    |------------|                                   |              |    |
   |------------------|    |            |    +-------------------------->   | SQL script 1 |    |
   |                  |+-> |            |                                   |              |    |
   |  Category        |    |            |                                   +--------------+    |
   |                  |    +------------+                                                       |
   |                  |                                                                         |
   |                  |    +------------+                                                       |
   |                  |    | MigStep 2     |                                                       |
   |                  |    |------------|                                   +--------------+    |
   |                  |+-> |            |   +--------------------------->   |              |    |
   |                  |    |            |                                   | SQL script 2 |    |
   |                  |    |            |                                   |              |    |
   +------------------+    +------------+                                   +--------------+    |
                    +                                                                           |
                    |      +------------+                                                       |
                    |      | MigrStep 3 |                                   +---------------+   |
                    |      |------------|                                   |               |   |
                    |      |            |   +--------------------------->   | SQL script 3  |   |
                    +----->|            |                                   |               |   |
                           |            |                                   +---------------+   |
                           +------------+                                                       v
                                                                     +-----------------------------+
                                                                     |  SQL ADATBÁZIS              |
                                                                     |-----------------------------|
                                                                     |                             |
                                                                     +-----------------------------+
```

## Saját adat adatbázisba tétele
- Létre kell hozni egy osztályt, ami az adatokat tartalmazza (pl. *public class Category { ... }*)
- Az osztályt fel kell venni DbSet típusú property-ként az ApplicationDbContext osztályba (pl: *public DbSet<Category> Categories { get; set; }*).
- ki kell adni az **Add-Migration** parancsot (a *Package Manager Console*-ból)
- ki kell adni az **Update-Database** parancsot (a *Package Manager Console*-ból).

## Adatbázis helyének megválasztása
A web.config-ban a ConnectionStrings beállítást kell átírni, i9tt vannak a minták: https://www.connectionstrings.com/sql-server/
