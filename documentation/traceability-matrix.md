Traceability Matrix – IdéenOgBogen
Formål

Formålet med denne traceability matrix er at sikre, at kravene/user stories fra mini-testplanen bliver dækket af konkrete testcases.

Matrixen viser sammenhængen mellem:

Krav/user stories
Testcases
Testniveau/testtype
Prioritet
Status

# Traceability Matrix

| Krav/User story                                                                                                        | Testcase                                               | Testniveau/Testtype         | Prioritet | Status    |
| ---------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------ | --------------------------- | --------- | --------- |
| US-01: Som bruger vil jeg kunne se en liste over produkter/bøger, så jeg kan få et overblik over udvalget.             | TC-01: Hent alle produkter via API                     | Integrationstest/Systemtest | Høj       | Ikke kørt |
| US-01: Som bruger vil jeg kunne se en liste over produkter/bøger, så jeg kan få et overblik over udvalget.             | TC-02: Hent produktliste uden produkter                | Integrationstest/Systemtest | Middel    | Ikke kørt |
| US-02: Som bruger vil jeg kunne se detaljer for ét bestemt produkt/en bestemt bog, så jeg kan læse mere om varen.      | TC-03: Hent produkt med gyldigt id                     | Integrationstest/Systemtest | Høj       | Ikke kørt |
| US-02: Som bruger vil jeg kunne se detaljer for ét bestemt produkt/en bestemt bog, så jeg kan læse mere om varen.      | TC-04: Hent produkt med ugyldigt id                    | Integrationstest/Systemtest | Høj       | Ikke kørt |
| US-03: Som systembruger vil jeg kunne oprette et nyt produkt/en ny bog, så den kan gemmes og vises i systemet.         | TC-05: Opret produkt med gyldige data                  | Integrationstest            | Høj       | Ikke kørt |
| US-03: Som systembruger vil jeg kunne oprette et nyt produkt/en ny bog, så den kan gemmes og vises i systemet.         | TC-06: Opret produkt med manglende eller ugyldige data | Integrationstest            | Høj       | Ikke kørt |
| US-04: Som bruger vil jeg kunne se produkter/bøger i frontend, så jeg kan bruge systemet gennem en simpel brugerflade. | TC-07: Frontend viser produkter fra backend            | Systemtest                  | Høj       | Ikke kørt |
| US-04: Som bruger vil jeg kunne se produkter/bøger i frontend, så jeg kan bruge systemet gennem en simpel brugerflade. | TC-08: Frontend håndterer fejl ved manglende backend   | Systemtest                  | Middel    | Ikke kørt |

## Krav uden testcases

Der er ingen af de valgte MVP-krav/user stories, som mangler testcases.

Alle fire user stories fra mini-testplanen er dækket af mindst to testcases.

## Kort refleksion

Traceability matrixen viser, at de vigtigste MVP-krav i produktflowet er dækket af testcases.

De højest prioriterede testcases er dem, der tester om produkter kan hentes, oprettes og vises korrekt. Det er vigtigt, fordi produktflowet er en central del af systemet.

Fejltestene er også vigtige, fordi systemet skal kunne håndtere ugyldige data og manglende backend uden at crashe.