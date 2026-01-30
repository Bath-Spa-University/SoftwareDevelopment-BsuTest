using JediAcademy.Presentation.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient("Species", config =>
{
    config.BaseAddress = new Uri(builder.Configuration["SwApi:Species"]);
});

builder.Services.AddHttpClient("Individuals", config =>
{
    config.BaseAddress = new Uri(builder.Configuration["SwApi:Individuals"]);
});

builder.Services.AddScoped<IJediEnrollmentService, JediEnrollmentService>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
