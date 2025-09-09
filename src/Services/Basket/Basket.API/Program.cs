
using BuildingBlocks.Behaviours;
using BuildingBlocks.Exceptions.Handler;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var assembly = typeof(Program).Assembly;
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
    config.AddOpenBehavior(typeof(LoggingBehaviour<,>));
});

builder.Services.AddMarten(opt =>
{ 
    try
    {
        opt.Connection(builder.Configuration.GetConnectionString("Database")!);
        opt.Schema.For<ShoppingCart>().Identity(x => x.UserName);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error configuring Marten: {ex.Message}");
        throw;
    }
}).UseLightweightSessions();

builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
var app = builder.Build();
// Configure the HTTP request pipeline. 
app.MapCarter();
app.UseExceptionHandler(options => { });
app.Run();
