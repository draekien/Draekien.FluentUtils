using FluentUtils.MediatR.PipelineBehaviours;
using FluentValidation.AspNetCore;
using MediatR;
using Serilog;

Log.Logger = new LoggerConfiguration()
             .WriteTo.Console()
             .CreateBootstrapLogger();

Log.Information("Starting MediatR Samples");

try
{
    Serilog.Debugging.SelfLog.Enable(Console.WriteLine);

    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration));

    builder.Services.AddMediatR(typeof(Program).Assembly);
    builder.Services.AddFluentValidation();

    builder.Services.AddPipelineBehaviourBuilder()
           .WithExceptionHandlingBehaviour()
           .WithRequestLoggingBehaviour()
           .WithRequestValidationBehaviour();

    builder.Services.AddControllers();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    WebApplication app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shutdown completed");
    Log.CloseAndFlush();
}


public partial class Program { }
