using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
   .AddReverseProxy()
   .LoadFromConfig(builder.Configuration.GetSection("Yarp"));
builder.Services.AddRateLimiter(rateLimiterOptions =>
{
    rateLimiterOptions.AddFixedWindowLimiter("fixed", options =>
    {
        options.Window = TimeSpan.FromSeconds(5);
        options.PermitLimit = 2;
    });
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("secure", policy =>
    policy.RequireAuthenticatedUser());
});

builder.Services.AddControllers();

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.MapReverseProxy();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();
app.Run();
