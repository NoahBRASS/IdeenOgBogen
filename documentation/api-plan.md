# API-plan - Idéen og Bogen

## Formål

API'et skal fungere som backend for boghandelssystemet.  
Det skal kunne håndtere produkter, kategorier, kunder, ordrer, betalinger og lager.

API'et skal kunne bruges af:
- Swagger
- Postman
- Simpel HTML/CSS/JavaScript frontend

---

## Arkitektur

Projektet er opdelt i flere lag:

- **Api**: Controllers og HTTP endpoints
- **Application**: Services, interfaces og DTOs
- **Domain**: Entities/domæneklasser
- **Infrastructure**: Database, DbContext, EF Core og MariaDB

Flow:

```text
Frontend/Postman/Swagger
        ↓
Controller
        ↓
Service
        ↓
DbContext / EF Core
        ↓
MariaDB database