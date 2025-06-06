using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Efficio.API.Configurations;
using Efficio.API.Middleware;
using Efficio.Core.Application;
using Efficio.Infrastructure;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Add Infrastructure layer with DbContext and other services
builder.Services.AddInfrastructure(builder.Configuration);

// Add Application layer services
// Uncomment if you have a separate method for Application layer registration
// builder.Services.AddApplication();

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerConfiguration();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        builder =>
        {
            builder.WithOrigins(
                    "http://localhost:3000", // React dev server
                    "https://yourfrontend.com") // Production frontend
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        });
});

// Configure JWT Authentication
builder.Services.AddJwtConfiguration(builder.Configuration);

// Add Application layer services
builder.Services.AddApplication();


var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
else
{
    // Use custom error handling middleware in production
    app.UseMiddleware<ErrorHandlingMiddleware>();
    app.UseHsts();
}

app.UseHttpsRedirection();

// Use custom request logging middleware
app.UseMiddleware<RequestLoggingMiddleware>();

// Use CORS policy defined above
app.UseCors("AllowSpecificOrigins");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Swagger
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    
    foreach (var description in provider.ApiVersionDescriptions)
    {
        options.SwaggerEndpoint(
            $"/swagger/{description.GroupName}/swagger.json",
            $"Efficio API {description.GroupName}");
    }
});

// Seed database if needed
// Uncomment if you have a database seeder
// using (var scope = app.Services.CreateScope())
// {
//     var services = scope.ServiceProvider;
//     var dbContext = services.GetRequiredService<AppDbContext>();
//     var logger = services.GetRequiredService<ILogger<Program>>();
//     try
//     {
//         await DatabaseSeeder.SeedAsync(dbContext);
//         logger.LogInformation("Database seeded successfully");
//     }
//     catch (Exception ex)
//     {
//         logger.LogError(ex, "An error occurred while seeding the database");
//     }
// }

app.Run();