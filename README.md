# Stocka ERP

### Multi-Organization & Multi-Branch ERP System

[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-MVC-512BD4?logo=dotnet)](https://learn.microsoft.com/aspnet/core)
[![PostgreSQL](https://img.shields.io/badge/Database-PostgreSQL-4169E1?logo=postgresql)](https://www.postgresql.org/)
[![Docker](https://img.shields.io/badge/Docker-Compose-2496ED?logo=docker)](https://www.docker.com/)
[![Status](https://img.shields.io/badge/Status-In%20Progress-orange)]()

Stocka is an ERP system designed to help small and medium-sized retail businesses manage their daily operations from one place.

The system supports multiple organizations, each with its own branches, users, and business data.

This repository contains a rebuild of the original Stocka project. The current version uses a different architecture and a new user interface. It is still under development, and some features are not available yet.

---

## Why Stocka?

Managing a retail business often means working with different tools for products, inventory, sales, purchases, and expenses.

Stocka aims to bring these operations together in one system.

The main goals are:

- Manage multiple organizations from one account.
- Manage several branches within each organization.
- Keep business data separated between organizations.
- Manage products, inventory, sales, and purchases.
- Build a system that can grow as business needs change.

---

## Core Features

The system is being developed around the following areas:

| Module | Description |
|---|---|
| Identity | User accounts and authentication |
| Organization | Organizations, branches, and memberships |
| Catalog | Products and product categories |
| Inventory | Stock, warehouses, batches, and stock movements |
| Purchasing | Purchase operations and received items |
| Sales | Sales operations and POS |
| Accounting | Financial records and transactions |
| Income & Expenses | Business income and expense tracking |

> **Development status:** Stocka is currently being rebuilt. The list above describes the planned system structure, not a claim that every module is complete.

---

## Architecture & Design

Stocka follows a **Modular Monolith** architecture.

The system is divided into modules based on business responsibilities. Each module has its own Domain, Application, and Infrastructure layers.

This keeps business logic organized while allowing the application to run as one deployable system.

### Architecture Overview

```mermaid
flowchart TB
    User([User])
    MVC[ASP.NET Core MVC]

    subgraph Modules["Business Modules"]
        Identity[Identity]
        Organization[Organization]
        Catalog[Catalog]
        Inventory[Inventory]
        Purchasing[Purchasing]
        Sales[Sales]
        Accounting[Accounting]
    end

    Contracts[Module Contracts]
    Database[(PostgreSQL)]

    User --> MVC
    MVC --> Modules
    Modules --> Contracts
    Modules --> Database
```

*This diagram shows the current architectural direction. The module implementation is still in progress.*

### Main Design Approaches

| Approach | Purpose |
|---|---|
| Modular Monolith | Divide the system into separate business modules |
| Clean Architecture | Separate business logic from infrastructure |
| Domain-Driven Design | Organize the domain around business concepts |
| CQRS | Separate commands from queries |
| MediatR | Handle application commands and queries |

### Communication Between Modules

Modules communicate through their contracts instead of accessing the internal implementation of other modules.

This helps keep module dependencies clear and reduces direct coupling between business areas.

---

## Organization & Access Model

Stocka is designed to support multiple organizations and branches.

A user can belong to more than one organization, and each organization can have multiple branches.

The main concepts are:

- **Organization:** Represents a business using Stocka.
- **Branch:** Represents a branch that belongs to an organization.
- **Organization Membership:** Connects a user account to an organization and defines its role.
- **Party:** Holds shared information about a person or business contact.
- **Party Organization:** Connects a party to an organization for business-related purposes.

This model separates user account permissions from business relationships.

For example, a user may have an employee role in one organization while being a customer in another.

---

## Technology Stack

The current rebuild uses the following technologies:

| Area | Technologies |
|---|---|
| Language | C# |
| Framework | .NET 10 |
| Web Application | ASP.NET Core MVC |
| ORM | Entity Framework Core |
| Read Operations | Dapper |
| Database | PostgreSQL |
| Application Flow | MediatR |
| Validation | FluentValidation |
| Authentication | ASP.NET Core Identity, JWT |
| Authorization | Role-Based Access Control (RBAC) |
| Containers | Docker, Docker Compose |

### Data Access Strategy

Stocka uses different tools for read and write operations:

- **Entity Framework Core:** Used for write operations and working with domain entities.
- **Dapper:** Used for read operations where direct SQL queries are useful.

The goal is to keep write operations connected to the domain model while allowing more control over read queries.

---

## Running with Docker Compose

Stocka is intended to support running its development environment through Docker Compose.

This allows developers to start the application and its required services using a small number of commands.

### Requirements

Before running the project, install:

- [Git](https://git-scm.com/)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)

### 1. Clone the repository

```bash
git clone https://github.com/FadiFayezDev/Stocka.git
cd Stocka
```

### 2. Configure environment variables

Some services may require configuration values such as database connection strings and application settings.

Create the required environment file based on the project's example configuration, if provided.

Do not commit passwords, secret keys, or private connection strings to the repository.

### 3. Start the application

From the directory containing the Docker Compose file, run:

```bash
docker compose up --build
```

Docker Compose will build the configured services and start them.

### 4. Stop the application

```bash
docker compose down
```

To stop the containers and remove their networks:

```bash
docker compose down
```

> **Note:** The exact services, environment variables, and local URLs depend on the current Docker Compose configuration. This section will be updated as the rebuild's setup becomes ready.

---

## Future Development

The project is being rebuilt step by step.

Some of the planned improvements include:

- Complete the core business modules.
- Improve inventory and warehouse workflows.
- Add automated tests.
- Add an ASP.NET Core Web API as another application entry point.
- Build a separate frontend using Next.js and TypeScript.
- Improve deployment and development setup.

The future API and frontend are not part of the current MVC application yet.

---

## Project Structure

The project follows a modular structure. Each module is responsible for its own business logic and infrastructure.

A simplified view:

```text
Stocka/
│
├── Modules/
│   ├── Identity/
│   │   ├── Domain/
│   │   ├── Application/
│   │   └── Infrastructure/
│   │
│   ├── Organization/
│   ├── Catalog/
│   ├── Inventory/
│   ├── Purchasing/
│   ├── Sales/
│   └── Accounting/
│
├── Hosts/
│   └── Stocka.MVC/
│
└── README.md
```

*This is a simplified representation of the intended structure. The actual repository structure may change during development.*

---

## Author

**Fady Fayez**

Junior Full-Stack .NET Developer

- GitHub: [FadiFayezDev](https://github.com/FadiFayezDev)
- LinkedIn: [FadyFayezDev](https://www.linkedin.com/in/fadyfayezdev/)

---

*Stocka is an independent development project. The current version is a rebuild of an earlier implementation and is still in progress.*
