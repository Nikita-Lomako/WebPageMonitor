using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WebPageMonitor.Client;
using WebPageMonitor.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Register services
builder.Services.AddScoped<ChangeLogService>();
builder.Services.AddScoped<PageVersionService>();
builder.Services.AddScoped<WatchedPageService>();

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7096") // адрес API
});

await builder.Build().RunAsync();
