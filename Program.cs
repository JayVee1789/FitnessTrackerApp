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

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<PoidsService>();
builder.Services.AddHttpClient<SupabaseService>();
builder.Services.AddSingleton<UpdateService>();
//var settings = await builder.Services
//    .BuildServiceProvider()
//    .GetRequiredService<HttpClient>()
//    .GetFromJsonAsync<SupabaseSettings>("settings.json");

//builder.Services.AddSingleton(settings);

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
var httpClient = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
var settings = await httpClient.GetFromJsonAsync<SupabaseSettings>("settings.json") ?? new SupabaseSettings();
builder.Services.AddSingleton(settings);
await builder.Build().RunAsync();
