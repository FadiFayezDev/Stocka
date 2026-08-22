# 📦 Stocka ERP - Multi-Tenant SaaS & Branch Isolation Platform

![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?style=for-the-badge&logo=dotnet)
![Docker](https://img.shields.io/badge/Docker-Containerized-2496ED?style=for-the-badge&logo=docker)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-17-4169E1?style=for-the-badge&logo=postgresql)
![MinIO](https://img.shields.io/badge/MinIO-S3_Compatible-C72C48?style=for-the-badge&logo=minio)
![Architecture](https://img.shields.io/badge/Architecture-Clean%20%2B%20CQRS-brightgreen?style=for-the-badge)

Stocka is a multi-brand, multi-branch ERP system engineered to simulate an enterprise business environment with production-grade backend architecture, strict data isolation, and scalable design decisions.

The core objective of this project is to go beyond simple CRUD operations by building a system capable of safely handling multiple brands, branches, and role-based access without risking cross-tenant data leaks.

> 🚧 **Status:** Actively under development

---

## 🎬 Architecture & Live Demo Walkthrough

[![Stocka ERP Demo Video](https://img.youtube.com/vi/gA8H65jbgj8/maxresdefault.jpg)](https://youtu.be/gA8H65jbgj8)

> 👈 **[اضغط هنا لمشاهدة الشرح العملي والمعمارية على YouTube](https://youtu.be/gA8H65jbgj8)**

---

## 🎯 Why This Project Exists

Most standard reference projects stop at basic authentication and simplistic CRUD operations. Stocka goes further by implementing:

* **Multi-tenant Architecture:** Data isolation across independent brands.
* **Branch-level Operational Scoping:** Dynamic data access based on active branch context.
* **Context-aware JWT Authentication:** Claims-driven access enforcement.
* **Backend-enforced Isolation:** Zero reliance on client-side filtering.
* **Granular Role-Based Access Control (RBAC):** Contextual permissions per brand/branch.

The primary focus is backend correctness, data safety, and clean software architecture.

---

## 📐 Core Architecture

The system strictly follows **Clean Architecture** principles and hosts two independent entry applications (**Web API** and **ASP.NET Core MVC**), sharing the core architectural layers:

* **Domain:** Core business rules, entities, and aggregate boundary logic.
* **Application:** Use cases, CQRS Command/Query handlers, and interface definitions.
* **Infrastructure:** Database contexts, Dapper repositories, external MinIO S3 object storage integration, and token generation.
* **API & MVC:** Standalone entry applications (RESTful Web API service & MVC web application).

```
                  ┌─────────────────────────────────────────┐
                  │        Standalone Applications          │
                  │   ┌──────────────┐   ┌──────────────┐   │
                  │   │   MVC App    │   │   Web API    │   │
                  │   └──────┬───────┘   └──────┬───────┘   │
                  └──────────┼──────────────────┼───────────┘
                             │                  │
            ┌────────────────┴──────────────────┴─────────────────┐
            ▼                                                     ▼
┌─────────────────────────┐                             ┌──────────────────┐
│   Commands (Write)      │                             │   Queries (Read) │
│ (EF Core / Aggregates)  │                             │     (Dapper)     │
└───────────┬─────────────┘                             └─────────┬────────┘
            │                                                     │
            └──────────────────────────┬──────────────────────────┘
                                       ▼
                     ┌──────────────────────────────────┐
                     │  PostgreSQL 17 / MinIO Object    │
                     └──────────────────────────────────┘
```

---

## 🔒 Multi-Brand & Multi-Branch Security Model

### Brand Context Isolation
* A user can belong to multiple brands.
* Upon switching brands, a new token/session context is generated containing `ActiveBrandId`, `BrandRole`, and `UserId`.
* All write/read operations enforce the `ActiveBrandId` server-side via `ICurrentUserContext`.
* `BrandId` supplied in routes or request bodies is never blindly trusted.

### Branch Context Isolation
* Operational data (Orders, Sales, Purchases, Expenses, Inventory Batches) is scoped to the active branch.
* **Owners & BrandAdmins:** Cross-branch access within their assigned brand.
* **Managers & Cashiers:** Restricted exclusively to their active branch.
* Switching branches requires context regeneration to preserve integrity.

---

## 🛠️ Tech Stack & Services

| Category | Technology |
|---|---|
| **Applications** | ASP.NET Core 9 (Standalone Web API & MVC App) |
| **Architecture** | Clean Architecture + CQRS Pattern |
| **ORM / Data Access** | Entity Framework Core (Writes) & Dapper (Reads) |
| **Database** | PostgreSQL 17 |
| **Object Storage** | MinIO (S3-Compatible Storage) |
| **Containerization** | Docker & Docker Compose |
| **Security** | Claims-driven JWT Tokens & Context Authentication |

---

## 📊 Implemented Modules

* **Products & Batch Management:** Multi-branch batch tracking and inventory quantities.
* **Orders & Point of Sale (POS):** Fast cashier checkout workflows.
* **Purchases & Inventory Restocking:** Supplier order execution.
* **Expenses & Employee Scoping:** Branch-filtered operational costs.
* **Brand Financial Reporting:** Aggregated Profit & Loss queries via optimized Dapper scripts.

---

## ⚡ Performance Strategy

* **Hybrid ORM Approach:** EF Core handles domain transactional consistency, while Dapper executes complex join queries and reporting projections.
* **Automated Scoping:** `BrandId` and `BranchId` parameters are automatically injected into Dapper SQL execution pipelines.
* **Asset Optimization:** Self-hosted MinIO object storage for media files with decoupled browser edge endpoints.

---

## 🐳 Quick Start (Docker Environment)

Ensure Docker Desktop is running on your machine, then execute:

```bash
# Clone the repository
git clone [https://github.com/FadiFayezDev/Stocka.git](https://github.com/FadiFayezDev/Stocka.git)
cd Stocka

# Spin up PostgreSQL, MinIO, pgAdmin, API, and MVC services
docker compose up -d --build
```

Access local instances:
* **Web MVC Application:** `http://localhost:5000`
* **Web API Service:** `http://localhost:6000`
* **MinIO Console:** `http://localhost:9001`
* **pgAdmin:** `http://localhost:5050`

---

## 🚀 Future Improvements

* Advanced multi-warehouse transfer management.
* Comprehensive unit & integration testing suite.
* Distributed caching with Redis.

---

## 👤 Author

**Fadi Fayez**  
*.NET Backend Developer & Software Architect*  

* **GitHub:** [@FadiFayezDev](https://github.com/FadiFayezDev)
