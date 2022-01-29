using Microsoft.AspNetCore.Mvc;

namespace FluentUtils.FromCompositeAttribute.Samples.Models;

public class ExampleRequest
{
    [FromRoute(Name = "id")]
    public int Id { get; init; }
    [FromQuery(Name = "replace")]
    public bool Replace { get; init; }
    [FromBody]
    public UserDto User { get; init; } = null!;
}

public class UserDto
{
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
}
