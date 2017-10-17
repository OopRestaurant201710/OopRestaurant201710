
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

## Kezdeti automatikus adatfeltöltés
\migrations\configuration.cs

Minden update-database esetén lefut


## Lenyílómező

```

+-----------------------+
|  HTML FORM (MenuItem) |
|-----------------------|     +------------------------+
|                       |     |  Category (table)      |
|  Category             |     |------------------------|
|                       |     |                        |
|                       |     |  1          Pizzák     |
|  Name                 |     |                        |
|                       |   +------------------------------+
|                       |   | |  2          Italok     |   | +--------------+
|  Description          |   +------------------------------+                |
|                       |     |                        |                    |
|                       |     |  3          Desszertek |                    |
|  Price                |     |                        |                    |
|                       |     +------------------------+                    |
+-----------------------+                                                   |
                                                                            |
                                                                            |
        +------------------------+                                          |
        |   Controller           |                                          |
        |------------------------|                                          |
        |   Category<+Category.Id| <-------+  POST      <----Category.Id----+
        |      +                 |
        |      v                 |
        |   MenuItem.Category    |
        |      +                 |
        |  DB <+                 |
        |                        |
        +------------------------+
```



## Lenyíló adatainak kezelése

```



 +------------------------+          +------------------+           +--------------------------+
 |  Model                 |          | Entity Framework |           | Adatbázis                |
 |------------------------|          |------------------|           |--------------------------|
 |                        |          |                  |           |                          |
 |    +------------+      |+-------->|                  |+--------->|  +--------+   +--------+ |
 |    |            |      |          |                  |           |  |        |   |        | |
 |    |Category    |      |<--------+|                  |<---------+|  |Category|   |MenuItem| |
 |    |            |      |          | +-----------+    |           |  |        |<-+|        | |
 |    +--^---------+      |          | | Category  |    |           |  |        |   |        | |
 |       |                |          | +-----------+    |           |  |        |   |        | |
 |       |                |          |     ^            |           |  +--------+   +--------+ |
 |    +--+----------+     |          |     |            |           |                 +        |
 |    |             |     |          | +---+-------+    |           |                 |        |
 |    |MenuItem     |     |          | |           |    |           |                 |        |
 |    |             |<----------------+| MenuItem  |<---------------------------------+        |
 |    +-------------+     |          | |           |    |           |                          |
 +------------------------+          +-+-----------+----+           +--------------------------+


 +---------------+            +--------------+
 |               |            |              |
 |               |            |  ENTRY       |
 |  FORM         |            |              |
 |               |            +--------------+
 |               |
 +---------------+
```


## A Felhasználók azonosítása és jogosultságkezelés

Bevezető rész: azonosítjuk a felhasználóinkat, és elválasztjuk a bejelentkezett és a be nem jelentkezett felhasználókat egymástól.

Az **ASP.NET Identity** a következőket végzi:

- lehetővé teszi a felhasználók regisztrációját az oldalon
- a regisztrált felhasználóknak végzi a session kezelését (session: bejelentkezéstől kijelentkezésig terjedő tevékenység)
- le tudjuk kérdezni egy kérés kiszolgálásakor, hogy a felhasználó be van-e jelentkezve, és ha igen, akkor mi az ő neve.
- a controllereket és az action-öket védeni tudjuk: megadhatjuk, hogy csak bejelentkezett felhasználók férjenek hozzá a teljes controllerhez (a controller összes action-jéhez) vagy az egyes action-ökhöz.

- Authentikáció: felhasználó azonosítás (bejelentkeztetés, sesion kezelés, kijelentkeztetés)
- Authorizáció: az adott felhasználónak van-e joga az adott tevékenységre (Controller/Action).

Az ASP.NET Identity alapértelmezetten Roles Based Authorizációval foglalkozik. Ez azt jelenti, hogy a felhasználókat csoportokba tudjuk rendezni, és csoport alapon tudunk jogosultságot osztani.

Például: fel tudok venni Admin, Pincér, Szakács, Főpincér csoportokat, és az egyes felhasználókat fel tudom venni ezekbe a csoportokba.

Majd, azt tudom mondani, hogy egy adott Controller/Action egy adott csoport számára használható.

### Csoport (Role) létrehozása
- Az AspNetRole táblába felvisszük a csoportok neveit (figyelem, az id mező GUID-ot vár, például [itt lehet](https://guidgenerator.com/online-guid-generator.aspx) ilyet generálni)
- Regisztrálunk felhasználókat (Ezek az AspNetUsers táblába kerülnek. Szintén GUID az id mezője)
- A felhasználókat hozzá tudjuk adni a csoporthoz úgy, hogy az AspNetUsers.ID értékéből és az AspNetRoles.Id értékéből létrejött párt felvesszük az AspNetUserRoles táblába

**AspNetUsers**

| AspNetUsers.Id | EMail |
| 72fae958-3f45-4563-871e-5a4762dbfae1 | gabor.plesz@gmail.com |
| **b8d3e6fc-84a6-4b38-8c0b-21590bd1bc90** | fopincer@netacademia.hu |
| cc2ac419-dbc1-4bd1-ade5-8a022eb96b7c | pincer@netacademia.hu |

**AspNetRoles**

AspNetRoles.Id|Name
d99da16c-0611-41a6-8e26-1793cc395d92|Admin
ae8e77d1-c3ff-4036-b8aa-0765c6ac101c|Pincer
**f5c4b79f-eaa7-4ce3-a759-4ce0c65b**|Fopincer

A végeredmény: **AspNetUserRoles**

UserId | RoleId
**b8d3e6fc-84a6-4b38-8c0b-21590bd1bc90** | **f5c4b79f-eaa7-4ce3-a759-4ce0c65b**

**Figyelem!**
A jogosultságokat cookie-ba írja, onnan olvassa az ASP.NET, és bejelentkezéskor írja, ezért ha változik valami, akkor ki kell jelentkezni és bejelentkezni, csak akkor lesz érvényes.

