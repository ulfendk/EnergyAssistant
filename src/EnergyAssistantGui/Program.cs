using System.Reflection;
using EnergyAssistant.BackgroundWorkers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using EnergyAssistantGui.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using MudBlazor.Services;
using UlfenDk.EnergyAssistant;

var dataDir = (args.Length >= 1
    ? new DirectoryInfo(args[1])?.FullName
    : new FileInfo(Assembly.GetEntryAssembly()?.Location)?.DirectoryName)
    ?? throw new ArgumentException("Data directory must be provided as the first argument.");

var configDir = Path.Combine(dataDir, "energyassistant");
if (!Directory.Exists(configDir)) Directory.CreateDirectory(configDir);

var builder = WebApplication.CreateBuilder(args);

StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices();

builder.Services
    .AddEnergyAssistant(configDir)
    .AddHostedService<EnergyAssistantServiceWrapper>();

var app = builder.Build();

await app.Services.MigrateDatabaseAsync();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();