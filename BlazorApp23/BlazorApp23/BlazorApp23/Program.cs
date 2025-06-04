using BlazorApp23.Components;
using BlazorApp23.Components.Pages;
using BlazorApp23.Components.Services;
using BlazorApp23.Components.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<FoodItemService>();
builder.Services.AddScoped<ClientAuthService>();
builder.Services.AddScoped<MenuService>();
builder.Services.AddScoped<CartService>();
builder.Services.AddScoped<SalesReportService>();
// In Program.cs
builder.Services.AddScoped<FeedbackService>();
builder.Services.AddScoped<UserCountService>();


// Add to your Program.cs



// Register AuthenticationService as a scoped service
builder.Services.AddScoped<AuthenticationService>();



// Register authentication services (you may need ASP.NET Identity or another method for authentication)
builder.Services.AddAuthentication()
    .AddCookie(options =>
    {
        options.LoginPath = "/login";  // Set login path
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();  // Enable HTTP Strict Transport Security (HSTS) for production
}

app.UseHttpsRedirection(); // Redirect HTTP requests to HTTPS
app.UseStaticFiles();      // Enable serving static files (like JS, CSS, images)
app.UseAntiforgery();      // Enable anti-forgery tokens (important for security)

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode(); // Enable interactive rendering for Razor components

// Configure authentication middleware
app.UseAuthentication();
app.UseAuthorization();

app.Run();
