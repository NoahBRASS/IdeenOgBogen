# IdeenOgBogen

## Purpose

IdeenOgBogen is a web-based bookstore developed as part of the Server-Side Programming course.

The project demonstrates the implementation of a layered ASP.NET Core Web API solution using Onion Architecture, Entity Framework Core, MariaDB and a custom frontend.

The purpose of the project is to create a Minimum Viable Product (MVP) that demonstrates a complete flow from database to frontend while following modern software development principles.

---

## Functional Scope

The bookstore supports management of:

* Products
* Categories
* Product Statuses
* Inventory
* Authors
* Book Details
* Customers
* Users
* Roles
* Orders
* Payments

The MVP focuses on the product domain and provides functionality for retrieving and displaying products through a REST API.

---

## Module Layout

The solution is divided into multiple projects:

### IdeenOgBogen.Api

Responsible for:

* Controllers
* HTTP Endpoints
* Swagger Configuration
* Dependency Injection

### IdeenOgBogen.Application

Responsible for:

* Services
* Use Cases
* DTOs
* Repository Contracts

### IdeenOgBogen.Domain

Responsible for:

* Domain Models
* Business Rules
* Core Entities

### IdeenOgBogen.Infrastructure

Responsible for:

* Entity Framework Core
* Database Access
* Repository Implementations
* MariaDB Integration

### IdeenOgBogen.Tests

Responsible for:

* Unit Tests
* Service Tests
* Validation Tests

---

## Architecture Style

The project follows Onion Architecture.

Benefits:

* Separation of Concerns
* Testability
* Maintainability
* Scalability

Dependency Flow:

API → Application → Domain

Infrastructure depends on Domain and Application but never the opposite.

---

## Technical Boundaries

### Database

* MariaDB
* Docker Container
* Entity Framework Core
* Pomelo MySQL Provider

### API

* ASP.NET Core Web API
* Swagger

### Frontend

* HTML
* CSS
* JavaScript

### Testing

* xUnit
* Moq

---

## Use Cases

### UC-0001 Retrieve Products

Description:

Retrieves all available products from the system.

Actor:

Customer

Priority:

Critical

---

### UC-0002 Retrieve Product By Id

Description:

Retrieves a specific product.

Actor:

Customer

Priority:

Critical

---

### UC-0003 Create Product

Description:

Creates a new product.

Actor:

Administrator

Priority:

High

---

### UC-0004 Update Product

Description:

Updates an existing product.

Actor:

Administrator

Priority:

Medium

---

### UC-0005 Manage Inventory

Description:

Updates stock information for products.

Actor:

Administrator

Priority:

Medium

---

## API Endpoints

### GET

* /api/products
* /api/products/{id}

### POST

* /api/products

### PUT

* /api/products/{id}

### DELETE

* /api/products/{id}

---

## Dependencies

### Internal Dependencies

* IdeenOgBogen.Domain
* IdeenOgBogen.Application
* IdeenOgBogen.Infrastructure

### External Dependencies

* ASP.NET Core
* Entity Framework Core
* MariaDB
* Docker
* Swagger
* xUnit
* Moq

---

## Git Workflow

Branches:

* main
* dev
* feature/*

Development process:

1. Create feature branch
2. Implement feature
3. Build and test
4. Create Pull Request
5. Merge into dev
6. Release through main

---

## Configuration

### Environment Variables

Managed through configuration files and Docker environment variables.

### Database

MariaDB is hosted inside Docker and accessed through Entity Framework Core.

---

## Version

Current Version:

1.0.0-MVP

Status:

In Development

---

Last Updated: 11-06-2026
