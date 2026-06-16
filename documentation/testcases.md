# Testscenarier og testcases – IdéenOgBogen

## Formål

Formålet med dette dokument er at opstille konkrete testscenarier og testcases ud fra user stories i mini-testplanen for **IdéenOgBogen**.

Testene fokuserer på MVP’ens produktflow:

* Hent alle produkter/bøger
* Hent ét produkt/en bog via id
* Opret et nyt produkt/en ny bog
* Vis produkter/bøger i frontend

Felterne **Actual result** og **Status** udfyldes, når testene bliver udført.

---

## User stories

| ID    | User story                                                                                                      |
| ----- | --------------------------------------------------------------------------------------------------------------- |
| US-01 | Som bruger vil jeg kunne se en liste over produkter/bøger, så jeg kan få et overblik over udvalget.             |
| US-02 | Som bruger vil jeg kunne se detaljer for ét bestemt produkt/en bestemt bog, så jeg kan læse mere om varen.      |
| US-03 | Som systembruger vil jeg kunne oprette et nyt produkt/en ny bog, så den kan gemmes og vises i systemet.         |
| US-04 | Som bruger vil jeg kunne se produkter/bøger i frontend, så jeg kan bruge systemet gennem en simpel brugerflade. |

---

# Testcases

## US-01 – Se liste over produkter/bøger

### TC-01 – Hent alle produkter via API

| Felt            | Beskrivelse                                                                            |
| --------------- | -------------------------------------------------------------------------------------- |
| Testcase ID     | TC-01                                                                                  |
| Testscenarie    | Hent produktliste via API                                                              |
| Preconditions   | Backend kører, og databasen indeholder mindst ét produkt.                              |
| Test steps      | 1. Åbn Swagger eller en API-client. <br> 2. Send en `GET` request til `/api/products`. |
| Expected result | API’et returnerer statuskode `200 OK` og en liste med produkter.                       |
| Actual result   |                                                                                        |
| Status          | Ikke kørt                                                                              |

### TC-02 – Hent produktliste uden produkter

| Felt            | Beskrivelse                                                                            |
| --------------- | -------------------------------------------------------------------------------------- |
| Testcase ID     | TC-02                                                                                  |
| Testscenarie    | Hent tom produktliste via API                                                          |
| Preconditions   | Backend kører, og databasen indeholder ingen produkter.                                |
| Test steps      | 1. Åbn Swagger eller en API-client. <br> 2. Send en `GET` request til `/api/products`. |
| Expected result | API’et returnerer statuskode `200 OK` og en tom liste. Systemet må ikke crashe.        |
| Actual result   |                                                                                        |
| Status          | Ikke kørt                                                                              |

---

## US-02 – Se detaljer for ét produkt/en bog

### TC-03 – Hent produkt med gyldigt id

| Felt            | Beskrivelse                                                                                                   |
| --------------- | ------------------------------------------------------------------------------------------------------------- |
| Testcase ID     | TC-03                                                                                                         |
| Testscenarie    | Hent produktdetaljer via id                                                                                   |
| Preconditions   | Backend kører, og databasen indeholder et produkt med det valgte id.                                          |
| Test steps      | 1. Åbn Swagger eller en API-client. <br> 2. Send en `GET` request til `/api/products/{id}` med et gyldigt id. |
| Expected result | API’et returnerer statuskode `200 OK` og data for det korrekte produkt.                                       |
| Actual result   |                                                                                                               |
| Status          | Ikke kørt                                                                                                     |

### TC-04 – Hent produkt med ugyldigt id

| Felt            | Beskrivelse                                                                                                            |
| --------------- | ---------------------------------------------------------------------------------------------------------------------- |
| Testcase ID     | TC-04                                                                                                                  |
| Testscenarie    | Hent produktdetaljer med ugyldigt id                                                                                   |
| Preconditions   | Backend kører, og der findes ikke et produkt med det valgte id.                                                        |
| Test steps      | 1. Åbn Swagger eller en API-client. <br> 2. Send en `GET` request til `/api/products/{id}` med et id, der ikke findes. |
| Expected result | API’et returnerer statuskode `404 Not Found` eller en passende fejlbesked. Systemet må ikke crashe.                    |
| Actual result   |                                                                                                                        |
| Status          | Ikke kørt                                                                                                              |

---

## US-03 – Opret nyt produkt/en ny bog

### TC-05 – Opret produkt med gyldige data

| Felt            | Beskrivelse                                                                                                     |
| --------------- | --------------------------------------------------------------------------------------------------------------- |
| Testcase ID     | TC-05                                                                                                           |
| Testscenarie    | Opret nyt produkt via API                                                                                       |
| Preconditions   | Backend kører, og API’et har et endpoint til oprettelse af produkter.                                           |
| Test steps      | 1. Åbn Swagger eller en API-client. <br> 2. Send en `POST` request til `/api/products` med gyldige produktdata. |
| Expected result | API’et returnerer statuskode `201 Created` eller `200 OK`, og produktet bliver oprettet i systemet.             |
| Actual result   |                                                                                                                 |
| Status          | Ikke kørt                                                                                                       |

### TC-06 – Opret produkt med manglende eller ugyldige data

| Felt            | Beskrivelse                                                                                                                                   |
| --------------- | --------------------------------------------------------------------------------------------------------------------------------------------- |
| Testcase ID     | TC-06                                                                                                                                         |
| Testscenarie    | Opret produkt med ugyldige data                                                                                                               |
| Preconditions   | Backend kører, og API’et har validering af produktdata.                                                                                       |
| Test steps      | 1. Åbn Swagger eller en API-client. <br> 2. Send en `POST` request til `/api/products`, hvor vigtige felter mangler, f.eks. titel eller pris. |
| Expected result | API’et returnerer statuskode `400 Bad Request` eller en passende valideringsfejl. Produktet må ikke blive oprettet.                           |
| Actual result   |                                                                                                                                               |
| Status          | Ikke kørt                                                                                                                                     |

---

## US-04 – Se produkter/bøger i frontend

### TC-07 – Frontend viser produkter fra backend

| Felt            | Beskrivelse                                                                                                            |
| --------------- | ---------------------------------------------------------------------------------------------------------------------- |
| Testcase ID     | TC-07                                                                                                                  |
| Testscenarie    | Vis produktliste i frontend                                                                                            |
| Preconditions   | Backend og frontend kører, og databasen indeholder mindst ét produkt.                                                  |
| Test steps      | 1. Start backend. <br> 2. Start frontend. <br> 3. Åbn frontend i browseren. <br> 4. Gå til siden hvor produkter vises. |
| Expected result | Frontend henter produkter fra backend og viser dem korrekt på siden.                                                   |
| Actual result   |                                                                                                                        |
| Status          | Ikke kørt                                                                                                              |

### TC-08 – Frontend håndterer fejl ved manglende backend

| Felt            | Beskrivelse                                                                         |
| --------------- | ----------------------------------------------------------------------------------- |
| Testcase ID     | TC-08                                                                               |
| Testscenarie    | Frontend håndterer API-fejl                                                         |
| Preconditions   | Frontend kører, men backend er stoppet eller utilgængelig.                          |
| Test steps      | 1. Stop backend. <br> 2. Start eller refresh frontend. <br> 3. Gå til produktsiden. |
| Expected result | Frontend viser en passende fejlbesked eller tom tilstand. Siden må ikke crashe.     |
| Actual result   |                                                                                     |
| Status          | Ikke kørt                                                                           |

---

## Kort refleksion

De vigtigste testcases er **TC-01, TC-03, TC-05 og TC-07**, fordi de tester det primære produktflow i MVP’en:

1. Produkter kan hentes fra backend.
2. Ét specifikt produkt kan hentes.
3. Nye produkter kan oprettes.
4. Frontend kan vise produkter fra backend.

Fejltestene **TC-04, TC-06 og TC-08** er også vigtige, fordi systemet ikke kun skal virke, når brugeren gør alt korrekt. Det skal også kunne håndtere fejl uden at crashe.
