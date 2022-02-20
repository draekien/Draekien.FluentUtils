# FluentUtils.MediatR.PipelineBehaviours

## Example Usage

```csharp
// Register all included pipeline behaviours
builder.Services.AddFluentUtilsPipelineBehaviours();

// Register pipeline behaviours manually
builder.Services.AddPipelineBehaviourBuilder()
       .WithExceptionHandlingBehaviour()
       .WithRequestLoggingBehaviour()
       .WithRequestValidationBehaviour();
```
