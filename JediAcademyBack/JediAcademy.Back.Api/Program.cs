using JediAcademy.Back.Application.Interfaces;
using JediAcademy.Back.Application.Queries;
using JediAcademy.Back.Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly, typeof(GetStudents).Assembly);
});

builder.Services.AddDbContext<JediStudentsDbContext>(options =>
{
    options.UseInMemoryDatabase("JediStudents");
});

builder.Services.AddScoped<IJediStudentsDbContext>(provider =>
    provider.GetRequiredService<JediStudentsDbContext>());

builder.Services.AddControllers();

var app = builder.Build();

// Seed in-memory database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var context = services.GetRequiredService<JediStudentsDbContext>();
        await JediStudentsDbContextSeed.SeedDataAsync(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating or seeding the database.");
        throw;
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();