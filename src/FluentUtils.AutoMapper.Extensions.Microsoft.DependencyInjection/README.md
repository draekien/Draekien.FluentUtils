# FluentUtils.AutoMapper.Extensions.Microsoft.DependencyInjection

## Example Usage

Use `AddAutoMapperProfiles` and pass it a params array of assemblies which contain
classes implementing the `IMapFrom<>` and `IReverseMapFrom<>` interfaces.

```csharp
// Program.cs
var builder = WebApplication.CreateBuilder(args);

// Register automapper profiles in the assembly containing the Program class
builder.Services.AddAutoMapperProfiles(typeof(Program).Assembly);
```

### IMapFrom<>

```csharp
public class User
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public DateTime Created { get; set; }
}

// This will allow automapper to map Users to UserDtos
public class UserDto : IMapFrom<User>
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
}
``` 

### IReverseMapFrom<>

```csharp
public class User
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public DateTime Created { get; set; }
}

// This will allow automapper to map Users to UserDtos and then back to Users
public class UserDto : IReverseMapFrom<User>
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
}
``` 
