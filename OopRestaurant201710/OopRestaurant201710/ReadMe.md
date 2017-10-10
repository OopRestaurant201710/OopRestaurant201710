
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
kell hozzá:

- EntityFramework nuget csomag
- a Code First Migration engedélyezése: a package Manager Console-ból: enable-migration
- ezzel létrejön egy Migrations könyvtár, ez alatt egy Configuration.cs állomány
- ha már létezik adatbázis, akkor automatikusan létrejön ide az első MigrationStep (XXX_InitialCreate névvel)
- ha nincs első migrációs lépés, akkor kézzel készítünk a Package Manager Console-ból: Add-Migration 'InitialCreate'
- végül létrehozzuk az adatbázist Package Manager Console-ból: Update-Database

az adatbázis pedig az SQL Server object Explorer ablakban látjuk a localdb csomópont alatt. Lehet, hogy frissítenünk kell.
