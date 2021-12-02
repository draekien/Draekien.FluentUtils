# FluentUtils.AutoMapper.Extensions.Microsoft.DependencyInjection

## Example Usage

Use `AddAutoMapperProfiles` and pass it a params array of assemblies which contain
classes implementing the `IMapFrom<>` interface.

```csharp
// Program.cs
var builder = WebApplication.CreateBuilder(args);

// Register automapper profiles in the assembly containing the Program class
builder.Services.AddAutoMapperProfiles(typeof(Program).Assembly);
```
