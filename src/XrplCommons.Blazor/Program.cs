using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using XrplCommons.Client.IoC;

using XrplCommons.Blazor;

WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");

builder.Services.AddXrplCommonsClient(options =>
{
    options.BaseUrl = builder.HostEnvironment.BaseAddress;
});

WebAssemblyHost host = builder.Build();
await host.RunAsync();
