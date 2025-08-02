using HyperBean.Services.AdminServices;
using HyperBean.Helpers.AdminHelpers;
using HyperBean.Models;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSession();
builder.Services.AddDistributedMemoryCache();

var app = builder.Build();
app.UseRouting();
app.UseSession();
app.UseStaticFiles();

app.MapPost("/admin/validate-signin", async (HttpContext context) =>
{
    AdminEndpoints response = new AdminEndpoints();
    return await response.ValidateLogin(context);
});

app.MapGet("/admin/authorize-signin", (HttpContext context) =>
{
    AdminEndpoints response = new AdminEndpoints();
    return response.IsAdminSessionActive(context);
});

app.MapPost("/admin/insert-coffee", async (HttpContext context) =>
{
    ProductEndpoints response = new ProductEndpoints();
    return await response.InsertCoffee(context);
});

app.MapPost("/admin/insert-addon", async (HttpContext context) =>
{
    ProductEndpoints response = new ProductEndpoints();
    return await response.InsertAddOn(context);
});

app.MapGet("/admin/get-coffee", (HttpContext context) =>
{
    ProductEndpoints response = new ProductEndpoints();
    return response.GetCoffee();
});

app.MapGet("/admin/get-addon", (HttpContext context) =>
{
    ProductEndpoints response = new ProductEndpoints();
    return response.GetAddon();
});

app.MapPost("/admin/delete-coffee", async (HttpContext context) =>
{
    ProductEndpoints service = new ProductEndpoints();
    return await service.DeleteCoffee(context);
});

app.MapPost("/admin/delete-addon", async (HttpContext context) =>
{
    ProductEndpoints service = new ProductEndpoints();
    return await service.DeleteAddon(context);
});

app.MapPost("/admin/update-coffee", async (HttpContext context) =>
{
    ProductEndpoints service = new ProductEndpoints();
    return await service.UpdateCoffeeStatus(context);
});

app.MapPost("/admin/update-addon", async (HttpContext context) =>
{
    ProductEndpoints service = new ProductEndpoints();
    return await service.UpdateAddonStatus(context);
});


app.Run();
