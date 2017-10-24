
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
   |  MenuItem        |    +------------+                                                       |
   |                  |    | MigStep 2  |                                                       |
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

## Saját megjelenítő és szerkesztő HTML template

A cél: kiemelni azonos kódot egy külön állományba, majd különböző helyekről használni.

- A View\Shared\**EditorTemplates** mappában vannak a szerkesztésre használt temlate nézetek.
- Ha kiadjuk a @Html.EditorForModel() utasítást a View-n, akkor az ASP.NET ezek közül megkeresi azt, ami a model-hez tartozik.
- Ha nincs ilyen, akkor generál egy alapértelmezettet
- ha van (MenuItem osztálynak a MenuItem.cshtml lesz a template-je, név alapján egyeztet) akkor beemeli az aktuális helyre az utasítás helyére

## Saját modellek, modellek szerepe

- Minden ami adat az tulajdonképpen "modell", ez a szakzsargonban szinoním fogalom.
- Az objektumokból rengeteg van, több tízezer, mi is akármennyit létre tudunk hozni. Ezért egy listában ezeket az objektumokat nem lehet értelmesen tárolni.
- erre találták ki a névtereket (namespace)
- 

A .NET névterek felépítése, példa
```
                        +------------+    +----------+
                        |            |    |          |
                  +----->    Data    +--->|  Entity  |
     +------------|     |            |    |          |
     |            +     +------------+    +----------+
     |   System   |
     |            +     +------------+
     +------------|     |            |
        +     +   +---->|    Linq    |
        |     |         |            |
        |     |         +------------+
        |     |
        |     |
        |     |         +------------+
        |     |         |            |
        |     |         |    Net     |
        |     +-------->|            |
        |               +------------+
        |
        |               +------------+  +---------+
        |               |            |  |         |
        |               |    Web     |+>|   MVC   |
        +-------------->|            |  |         |
                        +------------+  +---------+
```

A saját projekt névtér hierarchiánk egy része:


```



                              +-----------------+     +-----------------------+
                              |                 |     |                       |
                  +---------->|   Controllers   |+--->|  MenuItemsController  |
                  |           |                 |     |                       |
                  |           +-----------------+     +-----------------------+
                  |
 +----------------+-----+
 |                      |
 | OopRestaurant201710  |
 |                      |
 +----------------+-----+
                  |          +--------------+
                  |          |              |
                  +--------->|    Models    |
                             |              |
                             +--------------+
```

Névtér definíció, létrehozás

```
namespace CsoportosításNév
{
       
}

namespace CsoportosításNév.Csoportosításnév2
{
   class OsztályNév {}
}

```

így hivatkozunk rá (1. változat, hosszú név):
```
var valami = CsoportosításNév.Csoportosításnév2.OsztályNév()
```


így hivatkozunk rá (2. változat, using):
```
using CsoportosításNév.Csoportosításnév2;

namespace akármi
{
var valami = OsztályNév()
}
```

Modell osztályok elnevezése:

- Ha ki szeretnénk hangsúlyozni, hogy ő egy modell, akkor mindenképpen ilyen nevet kell neki adni.
- A Model (DataModel) az az osztály, ami az adattárolásban részt vesz. Vagy ő az az osztály, amibe, az adattárolásból betöltjük az adatokat.
- A ViewModel az a model osztály, ami a megjelenítésben vesz csak részt, "nem megy el" az adatbázisig. Ő az, akit a Controller előállít és átad a View-nak.
- Mi un. hibrid modellt gyártunk, (Category és MenuItem), ami mindenben részt vesz.

Ha szét akarnánk szedni ezeket a szerepeket, akkor lenne:
- egy Category osztály, ami a Code First segítségével az adatbázist "jelentené"
- egy CategoryModel osztály, amibe az adatbézisból olvasott adatokat beírnánk.
- és egy CategoryViewModel, amit a Controller gyárt, és átad a View-nak, illetve, a Controller fogad a HTML Form-ról és gyárt belőle CategoryModel-t, amit aztán valahogy Category-vá alakítunk és az adatbázisba írjuk.


## Nézet generálása DisplayTemplate segítségével

```





                        +------------------+             +---------------------------+
       +----------+  +->|  Create/Edit     |     +------>|  EditorTemplates-MenuItem |
       |  Layout  |  |  |------------------|     |       |---------------------------|
       |----------|  |  | bbb              |     +       |                           |
       |aaa       |  |  | EditorForModel() |+---->       | ccc                       |
       |aaa       |  |  | bbb              |             +---------------------------+
       |          |  +  +------------------+
       |RenderBody|+->
       |          |  +
       |aaa       |  |
       |aaa       |  |  +------------------+             +---------------------------+
       +----------+  +->|  Details/Delete  |     +------>| DisplayTemplates-MenuItem |
                        |------------------|     |       |---------------------------|
                        |bbb               |     +       |                           |
                        |EditorForModel()  |+---->       | ccc                       |
                        |bbb               |             +---------------------------+
                        |                  |
                        +------------------+

                        +------------------+
                        |Eredmény          |
                        |------------------|
                        |aaaa              |
                        |aaaa              |
                        |bbb               |
                        |ccc               |
                        |bbb               |
                        |aaaa              |
                        |aaaa              |
                        +------------------+


```

## CRUD műveletek szervezése a Controlleren
- C=Create, 
- R=Read, 
- U=Update, 
- D=Delete
- N=New

### Megjelenítő művelek (Get action-ök)
- Details
- Create
- Edit
- Delete

Ezekben az a közös, hogy ezek megjelenítő oldalakat generálnak
Adat(Bázis)Műveletek:
- Read
- New

### Módosító műveletek (Post action-ök)
- Create
- Edit
- Delete

Ezekben a műveletekben az a közös, hogy a beérkező adatokat elmentik:
- Create
- Update
- Delete

## Gyakorlás és ismétlés
- Asztalok és csoportosításuk: 
  az asztalfoglalásnak és a rendelésfelvételnek/számlázásnak is alapja, hogy a vendégek asztaloknál 
  foglalnak helyet, így kihagyhatatlan, hogy legyen az asztalokról nyilvántartásunk.

Fontos, hogy az adatmodell készítésekor az alapvető érvényességi feltételekre is koncentráljunk, később mindig nehezebb beépíteni!

Fontos, hogy a szöveges mezők (ha nincs rajtuk egyéb megszorítás, akkor tetszőleges hosszű szöveget tartalmazhatnak, **nvarcha(max)** típusú mező lesz belőlük)







- Tegyünk fel olyan elemeket az étlapra, amikre nem készültünk fel. 
  - Például szeretnénk desszertet felvenni az étlapra, és legyen cukormentes is
  - Az italok közül vannak szénsavas és szénsavmentes italok is.
  - az ételek közül pedig egyebek mellett érdemes nyilvántartani, hogy tartalmaz-e húst.


## Házi feladat (2017.10.24)
- az adatfeltöltéssel játszani (Migrations/Configuration/Seed) több asztal, több helyszín
- az Asztal osztálynak konstruktor készítése
- a Location osztálynak generált vezérlő és nézetek átnézése
- A Location és a Categories nézetek továbbfejlesztése: DisplayTemplate és EditorTemplate készítése és használata

