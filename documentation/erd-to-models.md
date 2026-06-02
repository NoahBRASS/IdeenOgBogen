# ERD til C# Models - Idéen og Bogen

## Formål

Dette dokument beskriver hvordan vores ERD omsættes til C# classes/entities i projektet.

Projektet er opdelt i lag:

| Projekt | Ansvar |
|---|---|
| IdeenOgBogen.Api | Controllers, routing og HTTP endpoints |
| IdeenOgBogen.Application | DTOs, interfaces og services |
| IdeenOgBogen.Domain | Entities/domæneklasser |
| IdeenOgBogen.Infrastructure | Entity Framework Core, DbContext, migrations og database |

---

## Overordnet flow

```text
Frontend / Postman / Swagger
        ↓
API Controller
        ↓
Application Service
        ↓
Infrastructure / DbContext
        ↓
MariaDB database
```

---

# Navngivning

I databasen bruger vi plural tabelnavne, men i C# bruger vi singular class names.

| Database tabel | C# class |
|---|---|
| Roles | Role |
| Users | User |
| Customers | Customer |
| Categories | Category |
| ProductStatus | ProductStatus |
| Products | Product |
| Inventory | Inventory |
| Orders | Order |
| OrderItems | OrderItem |
| Payments | Payment |
| Authors | Author |
| BookDetails | BookDetails |
| BookAuthors | BookAuthor |

---

# Brugere og roller

## Role

**Placering i kode:**

```text
backend/src/IdeenOgBogen.Domain/Entities/Role.cs
```

**Properties:**

| Property | Type | Beskrivelse |
|---|---|---|
| RoleId | int | Primary key |
| RoleName | string | Navn på rolle, fx Admin eller Customer |

**Relationer:**

- Én Role kan have mange Users.
- Én User har én Role.

---

## User

**Placering i kode:**

```text
backend/src/IdeenOgBogen.Domain/Entities/User.cs
```

**Properties:**

| Property | Type | Beskrivelse |
|---|---|---|
| UserId | int | Primary key |
| RoleId | int | Foreign key til Role |
| Username | string | Brugernavn, skal være unikt |
| Email | string | Email, skal være unik |
| PasswordHash | string | Hashet password |
| IsActive | bool | Om brugeren er aktiv |
| CreatedAt | DateTime | Hvornår brugeren blev oprettet |

**Relationer:**

- User har én Role.
- User kan have én Customer-profil.

**Bemærkning:**

Passwords må ikke gemmes som plain text.  
Vi gemmer kun `PasswordHash`.

---

## Customer

**Placering i kode:**

```text
backend/src/IdeenOgBogen.Domain/Entities/Customer.cs
```

**Properties:**

| Property | Type | Beskrivelse |
|---|---|---|
| CustomerId | int | Primary key |
| UserId | int | Foreign key til User |
| FullName | string | Kundens fulde navn |
| Phone | string | Telefonnummer |
| Address | string | Adresse |
| PostalCode | string | Postnummer |
| City | string | By |

**Relationer:**

- Customer hører til én User.
- Customer kan have mange Orders.

**Rettelse til ERD:**

I det gamle ERD stod `FullName` som `varchar(10)`.  
Det bør ændres til `varchar(100)`, fordi 10 tegn er alt for lidt til et fuldt navn.

---

# Produkter og kategorier

## Category

**Placering i kode:**

```text
backend/src/IdeenOgBogen.Domain/Entities/Category.cs
```

**Properties:**

| Property | Type | Beskrivelse |
|---|---|---|
| CategoryId | int | Primary key |
| ParentCategoryId | int? | Nullable foreign key til Category |
| Name | string | Kategorinavn |
| Description | string | Beskrivelse |

**Relationer:**

- Category kan have mange Products.
- Category kan have en parent category.
- Category kan have flere child categories.

**Eksempel:**

```text
Bøger
├── Programmering
├── Fantasy
└── Krimi
```

---

## ProductStatus

**Placering i kode:**

```text
backend/src/IdeenOgBogen.Domain/Entities/ProductStatus.cs
```

**Properties:**

| Property | Type | Beskrivelse |
|---|---|---|
| ProductStatusId | int | Primary key |
| StatusName | string | Statusnavn |
| IsSellable | bool | Om produktet må sælges |

**Eksempler:**

```text
Active
OutOfStock
Discontinued
```

**Forretningsregel:**

Hvis `IsSellable = false`, må produktet ikke sælges.

---

## Product

**Placering i kode:**

```text
backend/src/IdeenOgBogen.Domain/Entities/Product.cs
```

**Properties:**

| Property | Type | Beskrivelse |
|---|---|---|
| ProductId | int | Primary key |
| CategoryId | int | Foreign key til Category |
| ProductStatusId | int | Foreign key til ProductStatus |
| Name | string | Produktnavn |
| Description | string | Produktbeskrivelse |
| Price | decimal | Pris |
| SKU | string | Unikt varenummer |
| CreatedAt | DateTime | Oprettelsesdato |

**Relationer:**

- Product har én Category.
- Product har én ProductStatus.
- Product har én Inventory.
- Product kan have BookDetails.
- Product kan indgå i mange OrderItems.

**Bemærkning:**

Product er hovedtabellen for alle salgbare varer.

Eksempler:

- Bog
- Notesbog
- Kuglepen
- Brætspil

Bogspecifik information placeres i `BookDetails`.

---

## Inventory

**Placering i kode:**

```text
backend/src/IdeenOgBogen.Domain/Entities/Inventory.cs
```

**Properties:**

| Property | Type | Beskrivelse |
|---|---|---|
| ProductId | int | Primary key og foreign key til Product |
| Quantity | int | Antal på lager |
| LastUpdated | DateTime | Sidst opdateret |

**Relationer:**

- Inventory hører til ét Product.
- Product har én Inventory.

**Forretningsregel:**

`Quantity` må ikke være negativ.

---

# Ordrer og betalinger

## Order

**Placering i kode:**

```text
backend/src/IdeenOgBogen.Domain/Entities/Order.cs
```

**Properties:**

| Property | Type | Beskrivelse |
|---|---|---|
| OrderId | int | Primary key |
| CustomerId | int | Foreign key til Customer |
| OrderDate | DateTime | Ordredato |
| OrderStatus | string | Status på ordren |
| TotalAmount | decimal | Samlet beløb |

**Relationer:**

- Order hører til én Customer.
- Order har mange OrderItems.
- Order kan have én Payment.

**Bemærkning:**

`TotalAmount` gemmes på ordren, men skal beregnes ud fra:

```text
OrderItem.Quantity * OrderItem.UnitPrice
```

Det giver et historisk snapshot af ordren.

---

## OrderItem

**Placering i kode:**

```text
backend/src/IdeenOgBogen.Domain/Entities/OrderItem.cs
```

**Properties:**

| Property | Type | Beskrivelse |
|---|---|---|
| OrderItemId | int | Primary key |
| OrderId | int | Foreign key til Order |
| ProductId | int | Foreign key til Product |
| Quantity | int | Antal |
| UnitPrice | decimal | Pris på købstidspunktet |

**Relationer:**

- OrderItem hører til én Order.
- OrderItem hører til ét Product.

**Bemærkning:**

`UnitPrice` gemmes på ordrelinjen, fordi produktets pris kan ændre sig senere.

Eksempel:

Hvis kunden køber en bog til 199 kr., skal ordren stadig vise 199 kr., selv hvis produktets pris senere ændres til 249 kr.

---

## Payment

**Placering i kode:**

```text
backend/src/IdeenOgBogen.Domain/Entities/Payment.cs
```

**Properties:**

| Property | Type | Beskrivelse |
|---|---|---|
| PaymentId | int | Primary key |
| OrderId | int | Foreign key til Order |
| Amount | decimal | Betalt beløb |
| PaymentDate | DateTime | Betalingsdato |
| PaymentMethod | string | Betalingsmetode |
| PaymentStatus | string | Betalingsstatus |

**Relationer:**

- Payment hører til én Order.

**Anbefaling:**

Til vores projekt bør én Order kun have én Payment.  
Derfor kan `OrderId` i Payments gøres UNIQUE.

**Eksempler på PaymentStatus:**

```text
Pending
Paid
Failed
Refunded
```

**Eksempler på PaymentMethod:**

```text
Card
MobilePay
BankTransfer
Cash
```

---

# Bøger og forfattere

## BookDetails

**Placering i kode:**

```text
backend/src/IdeenOgBogen.Domain/Entities/BookDetails.cs
```

**Properties:**

| Property | Type | Beskrivelse |
|---|---|---|
| ProductId | int | Primary key og foreign key til Product |
| ISBN | string | Unikt ISBN |
| PublicationDate | DateOnly eller DateTime | Udgivelsesdato |
| Publisher | string | Forlag |
| Language | string | Sprog |
| PageCount | int | Antal sider |

**Relationer:**

- BookDetails hører til ét Product.
- BookDetails forbindes til Authors gennem BookAuthor.

**Bemærkning:**

Ikke alle Products er bøger.  
Derfor er BookDetails adskilt fra Product.

---

## Author

**Placering i kode:**

```text
backend/src/IdeenOgBogen.Domain/Entities/Author.cs
```

**Properties:**

| Property | Type | Beskrivelse |
|---|---|---|
| AuthorId | int | Primary key |
| Name | string | Forfatternavn |
| Biography | string | Kort biografi |

**Relationer:**

- Author kan have skrevet mange bøger.

---

## BookAuthor

**Placering i kode:**

```text
backend/src/IdeenOgBogen.Domain/Entities/BookAuthor.cs
```

**Properties:**

| Property | Type | Beskrivelse |
|---|---|---|
| ProductId | int | Del af composite primary key |
| AuthorId | int | Del af composite primary key |

**Relationer:**

- BookAuthor forbinder BookDetails og Author.
- En bog kan have flere forfattere.
- En forfatter kan have skrevet flere bøger.

---

# Constraints

Vigtige constraints fra ERD'et:

| Felt | Constraint |
|---|---|
| Users.Username | UNIQUE |
| Users.Email | UNIQUE |
| Customers.UserId | UNIQUE |
| Products.SKU | UNIQUE |
| BookDetails.ISBN | UNIQUE |
| Products.Price | >= 0 |
| Inventory.Quantity | >= 0 |
| OrderItems.Quantity | > 0 |
| Payments.Amount | > 0 |

---

# Første models vi bør lave

Vi starter med produktdelen:

```text
Product
Category
ProductStatus
Inventory
```

Grunden er, at produkter og lager er fundamentet for resten af systemet.

Derefter:

```text
Customer
User
Role
Order
OrderItem
Payment
BookDetails
Author
BookAuthor
```

---

# Første tekniske mål

Første store tekniske mål er:

```http
GET /api/products
```

Det viser at følgende virker:

```text
Routing
Controller
Service
DTO
DbContext
Database
```