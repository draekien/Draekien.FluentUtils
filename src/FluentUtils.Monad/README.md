# FluentUtils.Monad

This library contains an implementation of the Result monad.

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

## Concepts

### Creating Results

#### Result.Ok

Creates the `OkResultType<T>` from a value that you provide, or an `Empty` value
if you do not provide one.

```csharp
ResultType<string> result = Result.Ok("Hello World");
//^? OkResultType<string>

ResultType<Empty> result = Result.Ok();
//^? OkResultType<Empty>
```

#### Result.Error

Creates the `ErrorResultType<T>` from an error that you provide.

```csharp
ResultType<string> result = Result.Error<string>(new Error("Code", "Message"));
//^? ErrorResultType<string>

ResultType<Empty> result = Result.Error(new Error("Code", "Message"));
//^? ErrorResultType<Empty>
```

#### Bind

Binds a factory method into a result type by executing it inside a try/catch
block and converting the
result of the factory into an `OkResultType<T>` if it is successful, or an
`ErrorResultType<T>` if it throws an exception.

```csharp
ResultType<string> okResult = Result.Bind(() => "hello world");
//                 ^? OkResultType<string>

ResultType<string> errorResult = Result.Bind(() => throw Exception());
//                 ^? ErrorResultType<string>
```

### Accessing the Value inside a Result

#### Match

Checks the state of a `ResultType<T>`, executing an `okHandler` if it is an
`OkResultType<T>` or an `errorHandler` if it is an `ErrorResultType<T>`.

> See [Basic Usage](#basic-usage) for an example

#### Unwrap

Extracts the value from a `ResultType<T>` without checking if it is an
`OkResultType<T>`
or an `ErrorResultType<T>`.

> [!CAUTION]
> Unwrapping a `ResultType<T>` will throw an `UnwrapPanicException` if the
> result is a `ErrorResultType<T>`

```csharp
string value = Result.Ok("Hello World").Unwrap();
//     ^? "Hello World"

Result.Error(new Error("Code", "Message")).Unwrap();
//                                         ^? throws UnwrapPanicException
```

### Transforming the Value inside a Result

#### Pipe

Transforms the value of a `ResultType<T>` using the provided function if it is
an `OkResultType<T>`,
otherwise propagates the error of the `ErrorResultType<T>`.

```csharp
bool stringIsGreaterThanFive = Result.Bind(() => "hello world")
                                  .Pipe(value => value.Length)
                                  .Pipe(length => length >= 5)
                                  .Unwrap();
```

### Executing Side Effects on the Value of a Result

#### Tap

Access the value of a result to execute a side effect. The result will be
converted into an `ErrorResultType<T>` if the
side effect fails to execute due to an exception.

> [!NOTE]
> The side effect will only execute on an `OkResultType<T>`

```csharp
Result.Ok("Hello World")
    .Tap(value => Console.WriteLine(value));
```

#### Ensure

Ensures the value of a result passes the provided predicate. The result will be
converted into a `ErrorResultType<T>`
if the predicate does not return `true`

```csharp
string result = Result.Ok("Hello World")
    .Ensure(value => value.Length >= 1)
    .Pipe(value => value.ToUpperCase())
    .Unwrap();  // -> "HELLO WORLD"
```
