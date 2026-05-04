using FraudDetection.API.Configuration;
using FraudDetection.Infrastructure.Data;
using FraudDetection.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Iniciando FraudDetection API...");

    var builder = WebApplication.CreateBuilder(args);

    // Serilog como logger principal
    builder.Host.UseSerilog((context, services, configuration) =>
        configuration.ReadFrom.Configuration(context.Configuration));

    builder.Services.AddControllers();
    builder.Services.AddOpenApi();
    builder.Services.AddProjectDependencies(builder.Configuration);

    var app = builder.Build();

    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.Migrate();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        foreach (var role in new[] { "Admin", "User" })
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    app.MapOpenApi();

    // Loga cada request automaticamente
    app.UseSerilogRequestLogging(options =>
    {
        options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} respondeu {StatusCode} em {Elapsed:0.0000}ms";
    });

    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "API encerrada inesperadamente.");
}
finally
{
    Log.CloseAndFlush();
}