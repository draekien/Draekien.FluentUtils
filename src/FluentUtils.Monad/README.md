# FluentUtils.Monad

This library contains an implementation of the Result monad as made popular by Rust.

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
// synchronous callbacks
string result = await MaybeBob().MatchAsync(
    bob => "bob",
    err => "not bob"
);

// async callbacks
Order result = await CreateOrderAsync().MatchAsync(
    async order => await UpdateOrderAsync(order),
    async err => await ErrorOrderAsync(err)
);

// cancellation support
CancellationTokenSource cts = new CancellationTokenSource();

Order result = await CreateOrderAsync().MatchAsync(
    async (order, ct) => await UpdateOrderAsync(order, ct),
    async (err, ct) => await ErrorOrderAsync(err, ct),
    cts.Token
);
```