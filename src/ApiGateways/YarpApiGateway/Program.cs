using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixed", opt =>
    {
        opt.PermitLimit = 5;
        opt.Window = TimeSpan.FromSeconds(10);
    });
});
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("Web", p =>
    {
        p.WithOrigins("http://localhost:3000", "http://localhost:8080")
         .AllowAnyMethod()
         .AllowAnyHeader()
         .AllowCredentials();
    });
});
var app = builder.Build();
app.UseCors("Web");
app.UseRateLimiter();
app.MapReverseProxy();
app.Run();