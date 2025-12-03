---
applyTo: "**/*.cs"
description: "Coding style conventions for C# and .NET projects. When generating C# code, always follow these guidelines."
---

# C# and .NET Coding Styles, Conventions and Instructions

When generating C# code, always follow modern .NET conventions (C# 14 / .NET 10).

## General Guidelines

- Follow the official Microsoft .NET C# coding conventions: https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions
- Prefer clarity and readability over brevity.
- Use consistent formatting and naming throughout the codebase.

## Naming Conventions

- Use `PascalCase` for class, method, and property names.
- Use `camelCase` for local variables and method parameters.
- Use `ALL_CAPS` for constants.
- Prefix interfaces with `I` (e.g., `IOrderService`).
- Use meaningful, descriptive names; avoid abbreviations.

## Formatting

- Use 4 spaces for indentation (no tabs).
- Use file-scoped namespaces to simplify structure and improve readability.
- Add a blank line between method definitions.
- Place opening braces on a new line for methods, properties, and types (unless using file-scoped namespaces, then follow the file-scoped style).

### Example: File-Scoped Namespaces

```csharp
// Before
namespace MyNamespace
{
    public class ExampleClass
    {
        // ...existing code...
    }
}
// After
namespace MyNamespace;

public class ExampleClass
{
    // ...existing code...
}
```

- All new files must use file-scoped namespaces. Refactor existing files during updates or maintenance.

## Variable Declaration

- Use `var` for local variable declarations when the type is obvious.
- Prefer explicit types if it improves clarity.

### Example

```csharp
// Before
int x = 1;
double y = 2.0;
string z = "Hello";
ProductBacklogItem item = new ProductBacklogItem("Test", "Test", 1, 1, 1);
// After
var x = 1;
var y = 2.0;
var z = "Hello";
var item = new ProductBacklogItem("Test", "Test", 1, 1, 1);
```

## Sealed Classes

- Make classes `sealed` by default. If a class needs to be inherited, mark it as `virtual` explicitly.

## Use Nameof with Exceptions

- When throwing exceptions, use `nameof` to refer to the parameter name instead of hardcoding it.

### Example

```csharp
// Before
throw new ArgumentNullException("parameterName");
// After
throw new ArgumentNullException(nameof(parameterName));
```

## Code Structure

- One type per file (class, interface, enum, etc.).
- Organize files by feature/domain when possible.
- Group using directives at the top of the file, outside the namespace.
- Place related types in the same namespace.
- Use partial classes only when necessary (e.g., for code generation).

## Comments & Documentation

- Use XML documentation comments (`///`) for public members and types.
- Write comments to explain why, not what, when necessary.
- Remove commented-out code before committing.
- All comments and documentation must be always in English. Specific terms can be in the original language if they are widely recognized, and between backticks (e.g., `Alvarán`).

## Null Checks & Exceptions

- Use guard clauses for argument validation.
- Use `nameof` for parameter names in exceptions.

## Modern C# Features

- Use pattern matching and expression-bodied members where appropriate.
- Prefer object and collection initializers.
- Avoid using primary constructors.
- Prefer asynchronous methods (`async`/`await`) with cancellation tokens.
- Inject dependencies through built-in Dependency Injector (DI) container.
- Use trace capabilities and logs via `ILogger<T>`.

## Manage settings files

If applicable for the type of project (for example web application), manage different settings files for different environments and users as follows:

```csharp
if (Debugger.IsAttached)
{
    builder.Configuration.AddJsonFile(@"appsettings.debug.json", optional: true, reloadOnChange: true);
}

builder.Configuration.AddJsonFile($@"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                     .AddJsonFile($@"appsettings.{Environment.UserName}.json", optional: true, reloadOnChange: true)
                     .AddEnvironmentVariables();
```

## References

- Adhere to Microsoft's [coding conventions](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions).
