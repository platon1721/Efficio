using System;
using System.IO;
using DAL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Efficio.Infrastructure.Persistence;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        // Määra keskkond, kasutades keskkonna muutujat või vaikimisi "Development"
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
        Console.WriteLine($"Using environment: {environment}");
        
        // Leia projekti juurkaust
        var basePath = Directory.GetCurrentDirectory();
        
        // Liikuge API projekti kausta (kus on appsettings.json)
        var configPath = Path.Combine(Directory.GetParent(basePath).FullName, "Efficio.API");
        
        Console.WriteLine($"Looking for configuration in: {configPath}");
        
        var configuration = new ConfigurationBuilder()
            .SetBasePath(configPath)
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile($"appsettings.{environment}.json", optional: true)
            .Build();
        
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException(
                $"Could not find a connection string named 'DefaultConnection' in appsettings.{environment}.json");
        }
        
        Console.WriteLine($"Connection string found for environment: {environment}");
        
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseNpgsql(connectionString);
        
        return new AppDbContext(optionsBuilder.Options);
    }
}