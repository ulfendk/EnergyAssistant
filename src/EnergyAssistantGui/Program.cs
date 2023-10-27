using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Text.RegularExpressions;
using EnergyAssistant.BackgroundWorkers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using EnergyAssistantGui.Data;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using MudBlazor.Services;
using UlfenDk.EnergyAssistant;
using UlfenDk.EnergyAssistant.Config;

var dataDir = (args.Length >= 1
                  ? new DirectoryInfo(args[0])?.FullName
                  : new FileInfo(Assembly.GetEntryAssembly()?.Location)?.DirectoryName)
              ?? throw new ArgumentException("Data directory must be provided as the first argument.");

Console.WriteLine($"Datadir: {dataDir}");

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

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();