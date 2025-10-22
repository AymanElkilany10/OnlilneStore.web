# 🛒 OnlilneStore.web

## 📖 Project Overview
**OnlilneStore.web** is a complete backend solution for an online store, developed in **C#** using a **clean, layered architecture** (Core – Infrastructure – Shared).  
It provides a RESTful **API** (`OnlilneStore.API`) that enables all e-commerce operations such as product management, user authentication, order processing, and payment integration.

The project is designed to be scalable, maintainable, and easily extendable for future features or frontend integrations (Angular, React, or mobile apps).

---

## 🧱 Project Structure
| Folder | Description |
|---------|-------------|
| **Core/** | Contains business logic, domain entities, and core services. |
| **Infrastructure/** | Handles data access, persistence, and external integrations (e.g., database, payment). |
| **OnlilneStore.API/** | Main entry point for the application. Exposes all functionality through HTTP endpoints. |
| **Shared/** | Contains shared resources, utilities, and cross-cutting concerns used across layers. |
| `OnlilneStore.sln` | Visual Studio solution file that groups all projects together. |

---

## ⚙️ Technologies Used
- **.NET 6 / .NET 7**
- **Entity Framework Core**
- **SQL Server** (or any other RDBMS supported)
- **Stripe API** for payment integration
- **Dependency Injection** for clean modular design
- **Repository Pattern** & **DTOs** for data handling
- **Swagger / Postman** for API testing and documentation

---

## 🚀 Features
- 🧾 **Product Management:** Add, edit, delete, and view products.
- 👤 **User Management:** Register, login, and manage customers or admins.
- 🛍 **Order Management:** Create and track customer orders.
- 💳 **Payment Integration:** Integrated with Stripe for secure payments.
- 📊 **Reports (optional):** Can be extended to include sales and revenue reports.
- 🔐 **Secure Authentication:** Uses JWT tokens for API access control.
- 🧩 **Layered Architecture:** Follows clean architecture principles for separation of concerns.

---

## 🧰 Getting Started

### 1️⃣ Clone the Repository
```bash
git clone https://github.com/AymanElkilany10/OnlilneStore.web.git
cd OnlilneStore.web
