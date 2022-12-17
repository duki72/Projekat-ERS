# Projekat-ERS


Potrebno je napraviti dizajn sistema, arhitekturu sistema, implementirati i istestirati rešenje koje simulira rad proxy servera. Sistem treba da sadrži klijentske aplikacije koje vrše slanje podataka serveru, centralnog servera i proxy-ja koji vrši dobavljanje podataka sa servera.

Device
•	Poseduje jedinstveni ID
•	Vrši upis podataka o merenjima 
•	Merenje sadrži informaciju o tome da li je vrednost analogna ili digitalna, vrednost merenja, svoj ID i Timestamp
•	Slanje merenja obavlja se na svakih 5 minuta 
•	Može biti podignuto više instanci komponente Device

Server
•	Sluša zahteve za upis podataka u bazu podataka
•	Sluša zahteve za dobavljanje podataka od proxy servera
•	Loguje sve događaje u txt fajl
•	Instanca servera je jedinstvena

Client
•	Dobavlja podatke sa servera preko proxy-ja
•	Nudi user interface(može consol app) za čitanje različitih podataka sa servera. Podaci se mogu dobavljati po sledećim kriterijumima: 
o	svi podaci odabranog ID-ja 
o	poslednja ažurirana vrednost odabranog ID-ja
o	poslednje ažurirane vrednosti svakog uređaja
o	svi podaci analognih merenja
o	svi podaci digitalnih merenja
•	Može biti podignuto više instanci komponente Client

Proxy
•	Prima zahteve od klijenata za dobavljanje podataka sa servera zatim vrši dobavljanje i u odgovoru šalje klijentu tražene podatke
•	Svake dobavljene podatke sa servera čuva lokalno kako ne bi morao sledeći put ponovo da dobavlja od servera, čuva informaciju o tome kada su poslednji put dobavljeni dati podaci odnosno ažurirani kao i informaciju o tome kada se podacima poslednji put pristupilo
•	Prilikom klijentskog zahteva proxy prvo proverava da li tražene podatke ima lokalno. Ukoliko ima, šalje serveru zahtev u kom traži informaciju o tome kada su dati podaci poslednji put menjani, odnosno kada su za dati kriterijum poslednji put sačuvani novi podaci. Na osnovu datog odgovora od servera, proxy zaključuje da li je potrebno da podatke ponovo povlači sa servera ili su njegove lokalne kopije up to date.
•	Proxy na svakih 5 minuta proverava poslednji pristup lokalnim kopijama podataka i ukoliko je poslednji pristup bio pre više od 24h, lokalnu kopiju briše
•	Proxy loguje sve događaje u txt fajl
•	Instanca proxy je jedinstvena

*Sva vremena u sistemu moraju biti konfigurabilna
