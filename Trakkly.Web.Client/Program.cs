using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Trakkly.Shared.Services;
using Trakkly.Web.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Add device-specific services used by the Trakkly.Shared project
builder.Services.AddSingleton<IFormFactor, FormFactor>();

await builder.Build().RunAsync();