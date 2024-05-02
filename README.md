# CharacterApp-BackEnd
## Overview
Character App is a demo application to demonstrate various features of ASP.NET Core interacting with SQL Database via Entity Framework Core in a way that is presented in Revature training.

Character App is an application that allows users to create and manage different characters and their inventory.

## Currently Working Features
- CRUD Characters
- CRUD Speices
- CRUD Items
- Manage Character's Items
- Various input validation

## Concepts applied on this project
### Overall
- 3-tier architecture (Controller, Service, Data)
- Dependency Injection (program.cs and constructors of classes)
- Dependency Inversion
- Logging
- Null checks, null coalescing, and null forgiving operators
- Exception Throwing/Handling
- Async operations
- Utilizing DTOs

### Data Layer
- Repository Pattern
- EF Core Code First via migrations
- EF Core Fluent API (in DbContext)
- Data Transfer Objects
- EF Core queries (in repositories)
  - Including Join Tables/Sub-entities

### Service Layer
- Tuple
- Dictionary
- Null coalescing operator (??)
- Null forgiving operator(!)
- Converting from nullable to non-nullable
- Ternary operator(expression ? if true : if false)
- input validation/Exception throwing

### Controller Layer
- GET/POST/PUT/DELETE endpoints
- returning data
- returning IActionResult
- Query Parameters
- Route Parameters
- getting data via Request Body

### Models
- Defining M-M relationship
- Nullable properties
- default values for properties

## Upcoming Features
- Unit Testing
  - Models/Services/Data/Controller
  - With Moq
- Documentation for Character Controller/Service/Repository
- API Documentation
- Build and run instructions
- Deployment to Azure Web App
- Connection to Azure blob storage for image storage
