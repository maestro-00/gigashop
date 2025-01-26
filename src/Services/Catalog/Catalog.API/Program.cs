var builder = WebApplication.CreateBuilder(args);

// Add services to the container. 
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
}); 

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

var app = builder.Build();

// Configure the HTTP request pipeline. 
app.MapCarter();
app.Run();