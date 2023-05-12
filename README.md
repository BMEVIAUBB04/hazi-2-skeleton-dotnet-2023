# .NET projekt váz a Háttéralkalmazások tantárgy 2. házi feladatához

## Fejlesztőkörnyezet

Visual Studio használata javasolt, [Visual Studio Community](https://visualstudio.microsoft.com/vs/community/) változata elegendő.

Visual Studio használata esetén az alábbi [_workload_](https://docs.microsoft.com/en-us/visualstudio/install/install-visual-studio?view=vs-2019#step-4---choose-workloads) telepítésére van szükség:

- ASP.NET and web development

Ezt a _Visual Studio Installer_ (start menüben keresendő) segítségével telepíthetjük, a _Modify_ gomb megnyomásával.

## Webalkalmazás elindítása

1. A `Bme.Aut.Logistics` projekt legyen a startup projekt: _Solution Explorer_-ben jobb kattintás a projektre és _Set as Startup Project_.
1. A főmenüben _Build/Build Solution_-nel fordítás, majd _Debug/Start Debugging_-gal indítás.
1. A program konzol alkalmazásként indul, ez hosztolja a webalkalmazást. A logot a konzolban (és a Visual Studio output ablakában is) láthatjuk.
1. A webalkalmazás a `http://localhost:5000/` címen érhető el, ezen belül a relatív URL-ek a feladatkiírás szerint készítendőek el.

## Tesztek futtatása

A kiadott projekt váz tartalmaz teszteket is, amivel ellenőrizhető a megoldás. A tesztek maguk elindítják a webalkalmazást, így a tesztek futtatásához nem szükséges az alkalmazás előzetes futtatása. A tesztek futtatására a Visual Studio [_Test Explorer_](https://docs.microsoft.com/en-us/visualstudio/test/run-unit-tests-with-test-explorer?view=vs-2019)-t használhatjuk. Fontos, hogy a teszt kódot ne módosítsátok, ezt beadáskor ellenőrizzük.

## Segítség rendezéshez szöveges feltétel alapján

A szövegesen megadott rendezési feltétel Entity Framework lekérdezésben történő felhasználását megkönnyítendő, a webalkalmazás függősége a [Dynamic.LINQ modul](https://github.com/zzzprojects/System.Linq.Dynamic.Core) [NuGet csomagként](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.DynamicLinq/). Ennek használatához szükséges a `System.Linq.Dynamic.Core` névtér hivatkozása (a kiinduló projekt már ezt is tartalmazza!). Példa a használatra a modul [dokumentációjában található](https://dynamic-linq.net/basic-simple-query#ordering-results-combining-ascending-and-descending).
