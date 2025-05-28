using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using FitnessTrackerApp;
using FitnessTrackerApp.Services;
using Blazored.LocalStorage;
using FitnessTrackerApp.Models;
using System.Net.Http.Json;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// === Enregistrement des services ===
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<PoidsService>();
builder.Services.AddHttpClient<SupabaseService>();
builder.Services.AddSingleton<UpdateService>();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// === Chargement des settings Supabase ===
var httpClient = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
var settings = await httpClient.GetFromJsonAsync<SupabaseSettings>("settings.json") ?? new SupabaseSettings();
builder.Services.AddSingleton(settings);

// === Build & Initialisation ===
var host = builder.Build();

// Initialiser localStorage si vide (doit se faire après le build)
var poidsService = host.Services.GetRequiredService<PoidsService>();
await poidsService.GetExercicesAsync(); // force l'init si pas fait

await host.RunAsync();
