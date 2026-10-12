\# CampManager



ASP.NET Core MVC aplikacija za upravljanje smještajem i rezervacijama

kampa Miris Ljeta. Lični portfolio projekat za praktično učenje

C# web razvoja i rada s relacionom bazom podataka.



\## Funkcionalnosti



\- Prikaz, dodavanje, uređivanje i brisanje smještaja.

\- Trajno čuvanje podataka u SQL Server bazi.

\- Unos, pregled i brisanje rezervacija.

\- Automatski izračun ukupne cijene prema broju noćenja.

\- Provjera datuma dolaska i odlaska.

\- Provjera broja gostiju prema kapacitetu smještaja.

\- Sprečavanje preklapanja rezervacija istog smještaja.

\- Sprečavanje brisanja smještaja koji ima rezervacije.



\## Tehnologije



\- C# i .NET 10

\- ASP.NET Core MVC

\- Razor Views

\- Entity Framework Core 10

\- SQL Server LocalDB

\- HTML, CSS i Bootstrap



\## Pokretanje



Potrebni su Windows, .NET 10 SDK, SQL Server LocalDB i Visual Studio

s instaliranim paketom ASP.NET and web development.



1\. Preuzmite ili klonirajte repozitorij.

2\. Otvorite `CampManager.slnx` u Visual Studiju.

3\. Sačekajte obnovu NuGet paketa.

4\. Provjerite connection string `CampDatabase` u

&#x20;  `CampManager/appsettings.json`. Zadana instanca je

&#x20;  `(localdb)\\MSSQLLocalDB`.

5\. Otvorite Tools → NuGet Package Manager → Package Manager Console.

6\. Postavite Default project na `CampManager` i izvršite:



```powershell

Update-Database

```



7\. Pokrenite aplikaciju preko Debug → Start Without Debugging.

8\. Prvo dodajte smještaj, a zatim rezervacije kroz navigaciju.



Baza se kreira iz uključenih migracija. Početni podaci nisu uključeni.



\## Ručne provjere



\- Dodavanje i brisanje smještaja.

\- Uređivanje cijene smještaja.

\- Čuvanje podataka nakon ponovnog pokretanja.

\- Unos rezervacije i izračun ukupne cijene.

\- Odbijanje rezervacije s preklapanjem termina.

\- Odbijanje neispravnog redoslijeda datuma.

\- Odbijanje broja gostiju koji prelazi kapacitet.

\- Zabrana brisanja smještaja koji ima rezervacije.



\## Trenutna ograničenja



Projekat je lokalna portfolio demonstracija.

Autentikacija, korisničke uloge i automatizovani testovi

još nisu implementirani. Za demonstraciju koristite izmišljene

podatke gostiju.



\## Autorica



Ilhana Gabela

