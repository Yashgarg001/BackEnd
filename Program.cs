using ChatAppBackEnd.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.WebSockets;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ?? Add services to the container
builder.Services.AddControllers();
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// ? Load JWT Configuration

// ? Configure Entity Framework and Database Connection
builder.Services.AddDbContext<ChatAppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ? Configure JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtConfig");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddSingleton(new JwtServices(
    jwtSettings["Key"],
    jwtSettings["Issuer"],
    jwtSettings["Audience"]
));




// ? Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// ? Add Swagger Configuration
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "ChatApp API",
        Version = "v1"
    });
});

// ? Build the app
var app = builder.Build();

// ? Configure Middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    // ? Enable Swagger only in development
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ChatApp API V1");
        c.RoutePrefix = "swagger";
    });
}

// ? Enable CORS
app.UseCors("AllowAll");

// ? Enable WebSockets
app.UseWebSockets();
app.UseMiddleware<WebSocketMiddleware>();

// ? Enable Authentication and Authorization
app.UseRouting();
app.UseAuthentication(); // ? Authentication Middleware
app.UseAuthorization(); // ? Authorization Middleware

// ? Map Controllers
app.MapControllers();

// ? Run the app
app.Run();
