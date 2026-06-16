# Mini-testplan – IdéenOgBogen

## Formål

Formålet med denne mini-testplan er at planlægge og afgrænse testarbejdet for MVP-versionen af projektet **IdéenOgBogen**.

IdéenOgBogen er et boghandelssystem, hvor brugere skal kunne se produkter/bøger, og hvor systemet skal kunne hente og oprette produkter via backend API’et.

Denne testplan fokuserer på produktdelen af systemet, da det er en central del af MVP’en.

---

## User stories dækket af testplanen

| ID    | User story                                                                                                      |
| ----- | --------------------------------------------------------------------------------------------------------------- |
| US-01 | Som bruger vil jeg kunne se en liste over produkter/bøger, så jeg kan få et overblik over udvalget.             |
| US-02 | Som bruger vil jeg kunne se detaljer for ét bestemt produkt/en bestemt bog, så jeg kan læse mere om varen.      |
| US-03 | Som systembruger vil jeg kunne oprette et nyt produkt/en ny bog, så den kan gemmes og vises i systemet.         |
| US-04 | Som bruger vil jeg kunne se produkter/bøger i frontend, så jeg kan bruge systemet gennem en simpel brugerflade. |

---

## Scope

Denne testplan dækker kun produktflowet i MVP-versionen.

Det betyder, at testen fokuserer på:

* Hentning af alle produkter via API
* Hentning af ét produkt via id
* Oprettelse af et nyt produkt
* Samspil mellem controller, service/repository og database
* Frontend-visning af produkter fra backend

---

## Hvad skal testes?

| Område                | Hvad testes?                                                                         |
| --------------------- | ------------------------------------------------------------------------------------ |
| Produktliste          | At `GET /api/products` returnerer en liste af produkter.                             |
| Produktdetaljer       | At `GET /api/products/{id}` returnerer det rigtige produkt, hvis id findes.          |
| Fejlhåndtering        | At `GET /api/products/{id}` returnerer en passende fejl, hvis produktet ikke findes. |
| Oprettelse af produkt | At `POST /api/products` kan oprette et nyt produkt med gyldige data.                 |
| Databaselag           | At produkter kan gemmes og hentes korrekt via repository/database.                   |
| Frontend              | At frontend kan hente og vise produkter fra backend API’et.                          |

---

## Hvad skal ikke testes?

Følgende er ikke en del af denne mini-testplan:

* Login
* JWT/authentication
* Brugerroller
* Betaling
* Checkout/order flow
* Avanceret sikkerhedstest
* Performance/load testing
* Mobilversion/responsivt design
* Farver, layout og visuel finpudsning
* Avancerede administratorfunktioner

Disse områder er fravalgt, fordi projektets MVP først fokuserer på at få produktflowet til at virke stabilt fra backend til frontend.

---

## Testniveauer

| Testniveau       | Hvordan bruges det i projektet?                                                                                                                        |
| ---------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------ |
| Unit test        | Bruges til at teste mindre dele af systemet isoleret, f.eks. ProductService eller metoder der håndterer produkter.                                     |
| Integrationstest | Bruges til at teste samspillet mellem API, service/repository og database. Eksempel: opret et produkt og hent det bagefter.                            |
| Systemtest       | Bruges til at teste hele produktflowet gennem systemet, f.eks. via Swagger eller frontend.                                                             |
| UAT              | Bruges til at vurdere, om funktionerne opfylder brugerens behov. En anden gruppe, læreren eller en tester kan prøve systemet og vurdere produktflowet. |

---

## Ansvar

| Rolle                    | Ansvar                                        |
| ------------------------ | --------------------------------------------- |
| Udvikler                 | Implementerer funktionerne og retter fejl.    |
| Tester/gruppemedlem      | Udfører testcases og dokumenterer resultater. |
| Kunde/lærer/anden gruppe | Udfører eller vurderer UAT og giver feedback. |

---

## Godkendelseskriterier

Systemet vurderes som godkendt, når:

* `GET /api/products` returnerer en produktliste korrekt
* `GET /api/products/{id}` returnerer det rigtige produkt ved gyldigt id
* `GET /api/products/{id}` håndterer ugyldigt id korrekt
* `POST /api/products` kan oprette et nyt produkt med gyldige data
* Produktdata gemmes og hentes korrekt
* Frontend kan hente og vise produkter fra backend
* Alle kritiske testcases er gennemført
* Der ikke er åbne kritiske fejl i produktflowet
* Testeren eller kunden vurderer, at de dækkede user stories er opfyldt

---

## Kort refleksion over afgrænsning

Vi har valgt at afgrænse testplanen til produktdelen, fordi det er den vigtigste funktion i MVP-versionen af IdéenOgBogen.

Login, JWT, betaling og checkout er relevante funktioner for et færdigt boghandelssystem, men de er ikke nødvendige for at demonstrere det første fungerende produktflow. Derfor prioriterer vi først at sikre, at produkter kan oprettes, hentes og vises korrekt.
