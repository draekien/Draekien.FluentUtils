using FluentUtils.FromCompositeAttribute.Samples.Models;
using Microsoft.AspNetCore.Mvc;

namespace FluentUtils.FromCompositeAttribute.Samples.Controllers;

[ApiController]
[Route("[controller]")]
public class ExampleController : ControllerBase
{
    [HttpPost("{id:int}")]
    public IActionResult Update([FromComposite] ExampleRequest request)
    {
        // echo original request to show that everything is populating
        return Ok(request);
    }
}
