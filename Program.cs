using HyperBean.Services.AdminServices;
using HyperBean.Helpers.AdminHelpers;
using HyperBean.Helpers.UserHelpers;
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

app.MapGet("/admin/get-users", (HttpContext context) =>
{
    UserEndpoints response = new UserEndpoints();
    return response.GetUsers();
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

app.MapGet("/admin/logout", (HttpContext context) =>
{
    AdminEndpoints service = new AdminEndpoints();
    return service.Logout(context);
});

// USER ENDPOINTS

app.MapPost("/user/insert-user", async (HttpContext context) =>
{
    UserEndpoints service = new UserEndpoints();
    return await service.InsertUser(context);
});

app.MapPost("/user/get-user-account", () =>
{

});

app.MapGet("/user/validate-user-login", () =>
{

});

app.MapPost("/user/update-user-status", async (HttpContext context) =>
{
    UserEndpoints service = new UserEndpoints();
    return await service.UpdateStatus(context);
});


app.Run();
