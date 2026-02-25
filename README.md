# Stocka ERP

Stocka is a multi-brand, multi-branch ERP system I’m building to simulate a real-world business environment with proper backend architecture, strict data isolation, and scalable design decisions.

The main goal of this project is not just to build CRUD APIs, but to design a backend that can safely handle multiple brands, multiple branches, and role-based access without risking cross-tenant data leaks.

> 🚧 Status: Actively under development

---

## Why This Project Exists

Most beginner projects stop at basic authentication and simple CRUD operations.

Stocka goes further by introducing:

- Multi-tenant architecture (multiple brands)
- Branch-level operational scoping
- Context-aware JWT authentication
- Backend-enforced data isolation
- Role-based access control across brands and branches

The focus is on backend architecture and correctness rather than UI complexity.

---

## Core Architecture

The backend is built using:

- ASP.NET Core Web API
- Clean Architecture structure
- EF Core for relational data management
- Dapper for performance-critical queries
- SQL Server
- JWT Authentication

The solution is separated into:

- Domain
- Application
- Infrastructure
- API

This separation keeps business rules independent from infrastructure concerns.

---

## Multi-Brand & Multi-Branch Design

### Brand Context

A user can belong to multiple brands.

When switching brands, the system issues a new JWT containing:

- ActiveBrandId
- BrandRole
- UserId

All queries are automatically scoped to the active brand at the backend level.

BrandId is never trusted from request parameters.

---

### Branch Context

Each brand contains multiple branches.

Operational data such as:

- Orders
- Purchases
- Expenses
- Employees

is always scoped to the active branch.

Owners and BrandAdmins can access all branches within their brand.  
Managers and Cashiers are restricted to their assigned branch.

Branch switching regenerates a scoped token to ensure integrity.

---

## Security Model

Instead of relying on frontend filtering, the backend enforces isolation through:

- Context-aware JWT claims
- Centralized `ICurrentUserContext`
- Automatic Brand/Branch filtering
- Role-based authorization policies
- Controlled Dapper queries with enforced parameters

This prevents:

- Cross-brand data access
- Cross-branch manipulation
- Manual tampering with brandId in routes

---

## Implemented Modules

- Products Management
- Orders & Sales
- Purchases
- Employees
- Expenses
- Brand-level Profit & Loss reporting

Each module respects brand and branch boundaries.

---

## Performance Approach

- EF Core is used for transactional consistency.
- Dapper is used for high-performance read queries.
- Branch and brand filters are enforced server-side.
- Write operations automatically inject BrandId and BranchId where required.

---

## What I Focused On

- Designing for scale early
- Preventing data leakage by design
- Keeping business logic isolated
- Writing maintainable, structured backend code
- Avoiding shortcuts in multi-tenant logic

---

## Future Improvements

- Advanced inventory tracking
- More detailed financial reporting
- Automated testing coverage
- Performance profiling and tuning

---

## Author

Fadi Fayez  
.NET Backend Developer  

GitHub: https://github.com/FadiFayezDev
