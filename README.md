# Technical Showcase: ASP.NET Core MVC Blog Platform

[![Framework](https://img.shields.io/badge/.NET%208.0%2B-512BD4?style=flat&logo=.net&logoColor=white)](#)
[![Architecture](https://img.shields.io/badge/Architecture-MVC%20%2F%20Code--First-blue?style=flat)](#)
[![Database](https://img.shields.io/badge/Database-SQL%20Server%20%2F%20EF%20Core-CC2927?style=flat&logo=microsoft-sql-server&logoColor=white)](#)

This repository provides an in-depth backend engineering analysis and implementation of a custom Blog Platform built with **ASP.NET Core MVC** and **Entity Framework Core**. Designed as a streamlined, single-author system, it showcases robust infrastructure, custom security layers, and optimized data patterns.

---

## 🛠️ Core Tech Stack

*   **Framework:** .NET 8 (ASP.NET Core MVC)
*   **Data Access:** Entity Framework Core (Code-First approach)
*   **Database:** Microsoft SQL Server
*   **UI Engine:** Razor Views

---

## 📦 Project Structure

The codebase strictly adheres to the architectural separation of concerns defined by the MVC pattern, utilizing an isolated area for administrative workflows:

```text
├── Areas/
│   └── Admin/               # Dedicated namespace for administrative logic
│       ├── Controllers/     # Primary controller handling full CRUD and Auth engine
│       └── Views/           # Admin-facing dashboard interfaces
├── Controllers/             # Public-facing routing and content retrieval
├── Models/                  # Database schema definitions (Tbl_Post, Tbl_Profile)
├── ViewModels/              # Isolated validation models (PostVM, ProfileVM, LoginVM)
├── Views/                   # Public Razor UI layouts
├── wwwroot/                 # Static assets and physical file storage
│   └── Images/              # Dynamic uploaded blog media
├── AppDbContext.cs          # EF Core Context configuration
├── Program.cs               # Minimal hosting model pipeline configuration
└── README.md
