# FluentUtils.MediatR.Pagination

## Example Usage

Have your mediator query inherit from `PaginatedRequest<TResult>`

```c#
public class MyPaginatedRequest : PaginatedRequest<MyResult> 
{
    // request properties
}

public class MyPaginatedRequestHandler : IRequestHandler<MyPaginatedRequest, PaginatedResponse<MyResult>>
{
    // handle response
}
```
