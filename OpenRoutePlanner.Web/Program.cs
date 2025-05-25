using OpenRoutePlanner.Components;
using OpenRoutePlanner.ModelManagers;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddOutputCache();
builder.Services.AddHttpContextAccessor();

builder.Services.AddHttpClient<RoutesModelManager>(client =>
{
    client.BaseAddress = new("http://apiservice:8080");
});

builder.Services.AddHttpClient<DriverModelManager>(client =>
{
    client.BaseAddress = new("http://apiservice:8080");
});

builder.Services.AddHttpClient<AccountModelManager>(client =>
{
    client.BaseAddress = new("http://apiservice:8080");
});

builder.Services.AddHttpClient<TractorsModelManager>(client =>
{
    client.BaseAddress = new("http://apiservice:8080");
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.UseOutputCache();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapDefaultEndpoints();

app.Run();
