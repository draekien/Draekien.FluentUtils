using Microsoft.AspNetCore.Mvc;

namespace FluentUtils.FromCompositeAttribute.Samples.Models;

public class ExampleRequest
{
    [FromRoute(Name = "id")]
    public int Id { get; init; }
    [FromBody]
    public UserDto User { get; set; } = null!;
}

public class UserDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}
