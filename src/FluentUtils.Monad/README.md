# FluentUtils.Monad

This library contains an implementation of the Result monad as made popular by
Rust.

## Basic Usage

```csharp
var okResult = Result.Ok("bob");

// access value of result
okResult.Match(
    value => Console.WriteLine(value),       // "bob"
    err => Console.WriteLine(err.ToString()) // skipped
);

var errResult = Result.Error(new Error(new ErrorCode("1"), new ErrorMessage("No bob")));

errResult.Match(
    value => Console.WriteLine(value),       // skipped
    err => Console.WriteLine(err.ToString()) // "1: Not bob"
);
```

## Async Usage

```csharp
Order result = await CreateOrderAsync().MatchAsync(
    async (order, token) => await UpdateOrderAsync(order, token),
    async (err, token) => await ErrorOrderAsync(err, token),
    cancellationToken
);
```