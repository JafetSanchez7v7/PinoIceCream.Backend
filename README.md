# PinoIceCream - Enterprise Management System 🍦

Este es un sistema de gestión empresarial (ERP) desarrollado con **.NET 9**, diseñado bajo principios de **Clean Architecture** y **Domain-Driven Design (DDD)**. 

Desarrollado por: **[JafetSanchez7v7](https://github.com/JafetSanchez7v7)**

##  Arquitectura y Patrones

El proyecto está estructurado en capas para garantizar la separación de responsabilidades y la facilidad de mantenimiento:

* **API Layer:** Implementación de RESTful Endpoints y Middleware global.
* **Application Layer:** Lógica de negocio, DTOs y Mappings.
* **Core / Domain Layer:** Entidades de negocio y base de auditoría automática.
* **Infrastructure Layer:** EF Core, Repository Pattern y Unit of Work.

##  Tech Stack

* **Backend:** .NET 9 (C#)
* **Database:** SQL Server
* **ORM:** Entity Framework Core
* **Intelligence:** ML.NET (Planned)

##  Key Features (Implemented & Planned)

- [x] **Global Exception Handling:** Middleware centralizado para respuestas estandarizadas.
- [x] **Automatic Auditing:** Registro automático de fechas de creación y modificación.
- [ ] **Secure Authentication:** Implementación de **JWT (JSON Web Tokens)** y manejo de roles.
- [ ] **Predictive Inventory:** Modelo de **Machine Learning (ML.NET)** para predicción de demanda y stock.
- [ ] **Transactional Integrity:** Lógica atómica para operaciones críticas.

---
*Enfoque en escalabilidad y estándares de ingeniería FAANG.*