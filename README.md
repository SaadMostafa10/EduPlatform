# 🎓 EduPlatform - Educational Platform Backend

**EduPlatform** is an educational platform backend built with **ASP.NET Core Web API**, designed to provide a secure, scalable, and maintainable foundation for managing educational content, students, assessments, attendance, and platform services.

## 🔗 API Documentation

- **Scalar API UI:** Explore and test the API through the configured Scalar documentation interface.

## 🚀 Key Features

- **Advanced Authentication:** Secure authentication using **ASP.NET Core Identity**, JWT Access Tokens, Refresh Tokens, Email Confirmation, and OTP-based Password Reset.
- **Role-Based Authorization:** Protects platform resources using role-based access control with additional resource-level authorization where required.
- **Lessons Management:** Complete lesson management with searching, filtering, sorting, pagination, and Grade-based access control.
- **Grade Management:** Grade management with seeded initial data and specification-based querying.
- **Specification-Based Queries:** Reusable specifications for filtering, searching, sorting, pagination, and related-data retrieval.
- **Repository & Unit of Work:** Encapsulates data access through Repository and Unit of Work abstractions.
- **Global Exception Handling:** Centralized exception handling through custom exceptions and global middleware.
- **Validation:** Request and business validation for platform operations, including lesson and authentication-related validation.
- **Module-Based Statistics:** Management modules provide database-level aggregated statistics through dedicated module repositories and query projections.
- **Secure Token Management:** Refresh tokens are persisted, validated, revoked, and rotated as part of the authentication lifecycle.

## 🏗️ Project Architecture (Onion Architecture)

The project follows **Onion Architecture** with clear separation between domain, application, infrastructure, and presentation concerns.

### 📁 Project Breakdown

1. **Domain**
   - The core of the application.
   - Contains entities, repository abstractions, specifications, and core contracts.
   - Has no dependency on infrastructure or presentation concerns.

2. **Services**
   - Contains application/business logic.
   - Coordinates use cases between the API and infrastructure abstractions.
   - Handles application workflows, validation orchestration, authorization logic, and mapping.

3. **Services.Abstractions**
   - Contains application service interfaces and contracts.
   - Provides loose coupling between the Web API and service implementations.

4. **Persistence**
   - Handles data access and infrastructure concerns.
   - Contains EF Core `DbContext`, entity configurations, migrations, seed data, Repository implementations, and Unit of Work.

5. **Shared**
   - Contains shared contracts and reusable models.
   - Includes API DTOs, common response models, constants, and shared types.

6. **WebApi**
   - The presentation/API layer.
   - Contains controllers, middleware configuration, dependency injection setup, authentication configuration, and API documentation configuration.

## 🔐 Authentication & Security

EduPlatform uses **ASP.NET Core Identity** with **JWT-based authentication**.

Authentication features include:

- User Registration
- Login
- JWT Access Tokens
- Refresh Tokens
- Refresh Token Revocation
- Logout
- Email Confirmation
- OTP-based Password Reset
- Role-Based Authorization
- Resource-level authorization where required

## 📊 Module Statistics

Management pages contain statistics cards such as:

- Total
- Active
- Inactive
- Expired
- Total Usages
- Remaining Usages

For statistics that belong to a specific module, the project follows a consistent query pattern:

```text
Controller
    ↓
Module Service
    ↓
Module Repository
    ↓
Stats Projection
    ↓
Module Service
    ↓
Stats DTO
    ↓
Controller
