using Catalog.API.Data;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container. 
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
    config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
    config.AddOpenBehavior(typeof(LoggingBehaviour<,>));
});  
builder.Services.AddCarter();
builder.Services.AddMarten(opt =>
{ 
    try
    {
        opt.Connection(builder.Configuration.GetConnectionString("Database")!);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error configuring Marten: {ex.Message}");
        throw;
    }
}).UseLightweightSessions();

if (builder.Environment.IsDevelopment())
    builder.Services.InitializeMartenWith<CatalogInitialData>();

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddHealthChecks().AddNpgSql(builder.Configuration.GetConnectionString("Database")!);
var app = builder.Build();

// Configure the HTTP request pipeline. 
app.UseExceptionHandler(options => {});
// app.UseExceptionHandler(exceptionHandler =>
// {
//     exceptionHandler.Run(async context =>
//     {
//         var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
//         if (exception == null)
//         {
//             return;
//         }
//
//         var problemDetails = new ProblemDetails
//         {
//             Title = exception.Message,
//             Status = StatusCodes.Status500InternalServerError,
//             Detail = exception.StackTrace
//         };
//         
//         var logger = context.RequestServices.GetService<ILogger<Program>>();
//         logger.LogError(exception, exception.Message);
//         
//         context.Response.StatusCode = StatusCodes.Status500InternalServerError;
//         context.Response.ContentType = "application/problem+json";
//         
//         await context.Response.WriteAsJsonAsync(problemDetails);
//     });
// });
app.UseHealthChecks("/health",new HealthCheckOptions()
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapCarter();
app.Run();