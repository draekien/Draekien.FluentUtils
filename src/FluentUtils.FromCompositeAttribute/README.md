# FluentUtils.FromCompositeAttribute

## Example Usage

```c#
// the request class
public class ExampleRequest
{
    [FromRoute(Name = "id")]
    public int Id { get; set; }
    
    [FromQuery(Name = "replace")]
    public bool Replace { get; set; }
    
    [FromBody]
    public Customer Customer { get; set; }
}

public class Customer
{
    public int Id { get; set; }
    public string FullName { get; set; }
}

// controller using FromComposite attribute
// this will allow you to pass in the id in the route
// the replace flag as a querystring
// and the customer as the body.
[HttpPut("{id:int}")]
public async Task<IActionResult> UpdateAsync(
    [FromComposite] ExampleRequest request,
    CancellationToken cancellationToken)
{
    // your controller implementation
}

// example call to controller
UserDto user = new() { FirstName = "John", LastName = "Smith" };
HttpResponseMessage response = await client.PostAsJsonAsync("example/1?replace=true", user, default);
```
