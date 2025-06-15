# Dynamic Mapping System
This project provides a flexible and extensible mapping system for converting between various reservation models (e.g., Google, Booking, etc.) and the internal DIRS21 data model. The mapping logic is not limited to the `Reservation` model. It's also possible to implement mappings for other models, such as `Room` or any other domain-specific entity.
It enables seamless data transformation across different sources and targets with minimal changes.

# How to Add a New Mapper
1. Create a new folder for the data model inside the `MappingSystem.Models` project (e.g., `/Booking`).
2. Add a new `Reservation` class defining the relevant properties for that reservation model.
3. Ensure the namespace of the new data model class aligns with the reservation type (e.g., `Booking`).
4. In the `MappingSystem.Mappers` project, add a new folder under the Reservations folder corresponding to your model (e.g., `/Booking`).
5. Add new mapper classes under this folder to handle conversions from DIRS21 model to your model and vice versa.
6. Inherit these classes from the generic base class `MapperBase<TSource, TTarget>`, specifying source and target types.
7. Implement the `Map` and `Validate` methods to provide mapping and validation logic.
8. The application will automatically register these new mappers into the Dependency Injection container using reflection—no manual registration required.
9. Done! Your new mapper is integrated and ready to use.

# Validation
Validation is implemented directly within mapper classes using simple checks that `throw ValidationException` with messages.
Third-party libraries like `FluentValidation` are not used but can be integrated if needed for more complex scenarios.

# Dependecy Injection
This project uses `Autofac` as the DI container, which is particularly useful in console applications:
 - On startup, the app scans assemblies for classes implementing the `IMapper` interface.
 - All discovered mapper classes are registered dynamically with a transient lifetime.
 - This eliminates the need for manual DI configuration when adding new mappers.

# Architecture
## Project Structure
```
- src
  - MappingSystem.App
    - Program
  - MappingSystem.Models
    - Google
      - Reservation.cs
    - DIRS21
      - Reservation.cs
  - MappingSystem.Mappers
    - Base
      - MapperBase.cs
    - Reservations
      - Google
        - ReservationFromGoogleMapper
        - ReservationToGoogleMapper
  - MappingSystem.Core
    - IMapper.cs
    - MapHandler.cs
- test
  - MappingSystem.Tests
- out
```
## Design Highlights
 - Models and Mappers are separated into distinct projects (`MappingSystem.Models` and `MappingSystem.Mappers`) to promote modularity and independent evolution of data contracts and mapping logic.
 - The core orchestrator is the `MapHandler` class, which exposes a single Map method. It accepts a source data object, a sourceType string, and a targetType string. The handler uses dependency injection to receive all registered mappers and selects the appropriate mapper based on the source and target types.
 - `MapHandler` scans the injected mappers to find one capable of handling the requested transformation. If no suitable mapper is found or the source object type does not match the declared source type, the handler throws exceptions to enforce correctness.
 - All mappers implement the generic `IMapper<TSource, TTarget>` interface, which requires `Map`, `Validate`, and `ValidateType` methods.
 - Common functionality such as type checking (`ValidateType`) and mapper capability checks (`CanHandle`) are abstracted in the base class `MapperBase<TSource, TTarget>`.
 - Adding a new mapper involves creating a new model and a corresponding mapper class implementing `IMapper`. Thanks to the modular design and dependency injection, these new mappers are automatically discovered and integrated without modifying the existing codebase.

## SOLID principles:
 - **SRP**: Each class has one responsibility.
 - **OCP**: The system is open for extension but closed for modification. Adding a new mapper or model doesn’t require changing existing code—just add new classes implementing the IMapper interface.
 - **DIP**: `MapHandler` depends on the abstraction (IMapper interface), not concrete implementations.


# Testing
Each mapper has dedicated unit tests verifying correctness of mapping and validation. The `MapHandler` is tested to ensure proper mapper selection and error handling.

# CI Pipeline
A GitHub Actions workflow is configured with build and test stages to automate the CI process.

# Limitations
 - Designed for relatively straightforward 1:1 field mappings and simple transformations.
 - Complex business rules involving nested or related entities (e.g., Country, Currency, PaymentStatus) require additional logic or a more advanced approach.
 - Does not support mapping of nested collections or JSON objects.
 - Validation is limited to basic data integrity within mappers; cross-entity or external system validations are not implemented.
 - AutoMapper or other third-party mapping libraries are not used.

# Assumptions
 - Assumes that model classes follow consistent naming patterns to allow for generic discovery and mapping (e.g. `Google.Reservation` or `Booking.Reservation`).
 - Assumes each mapper handles a specific source-to-target mapping (from external to inernal and from internal to external mappers)
 - Assumes that dependency injection is correctly configured to discover and register all mappers at application startup.
 - Assumes that callers handle exceptions thrown by validation or mapping failures
   
# Summary
This project provides a flexible and extensible mapping system designed to transform data between various source and target models. It uses dependency injection (using Autofac) to automatically discover and register mapper implementations, enabling easy addition of new mappings with minimal code changes.

The system supports validation within mappers, ensuring data consistency before transformation. Its modular architecture allows for clear separation of concerns and easy scalability across multiple data sources and targets.
