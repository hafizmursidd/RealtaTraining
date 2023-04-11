using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TrainingMVVM;
using TrainingMVVM.Models;
using TrainingMVVM.ViewModels;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddTransient<FetchDataModel>();
builder.Services.AddTransient<FetchDataViewModel>();
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
