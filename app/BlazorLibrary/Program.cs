using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorLibrary;
using BookManagementApp.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

string origins = "origins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(origins, policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

var url = builder.Configuration.GetValue<string>("ApiUrl");

builder.Services.AddLibraryClient()
    .ConfigureHttpClient(client => client.BaseAddress = new Uri($"{url}/graphql"));

builder.Services.AddScoped<BookService>();
builder.Services.AddScoped<AuthorService>();

await builder.Build().RunAsync();