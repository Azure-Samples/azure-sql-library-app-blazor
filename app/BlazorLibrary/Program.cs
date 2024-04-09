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

builder.Services.AddLibraryClient()
    .ConfigureHttpClient(client => client.BaseAddress = new Uri("http://127.0.0.1:5001/graphql"));

builder.Services.AddScoped<BookService>();
builder.Services.AddScoped<AuthorService>();

await builder.Build().RunAsync();