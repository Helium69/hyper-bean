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

app.MapGet("/user/get-user-account", (HttpContext context) =>
{
    UserEndpoints service = new UserEndpoints();
    return service.GetCurrentUser(context);
});

app.MapGet("/user/validate-user-session", (HttpContext context) =>
{
    UserEndpoints service = new UserEndpoints();
    return service.ValidateUserSession(context);
});

app.MapPost("/user/validate-user-login", async (HttpContext context) =>
{
    UserEndpoints service = new UserEndpoints();
    return await service.ValidateUserLogin(context);
});

app.MapPost("/user/update-user-status", async (HttpContext context) =>
{
    UserEndpoints service = new UserEndpoints();
    return await service.UpdateStatus(context);
});

app.MapGet("/user/get-available-coffee", (HttpContext context) =>
{
    ProductEndpoints service = new ProductEndpoints();
    return service.GetAvailableCoffee();
});

app.MapGet("/user/get-available-addon", (HttpContext context) =>
{
    ProductEndpoints service = new ProductEndpoints();
    return service.GetAvailableAddOn();
});

app.MapPost("/user/add-funds", async (HttpContext context) =>
{
    UserEndpoints service = new UserEndpoints();
    return await service.AddFunds(context);
});

app.MapPost("/user/buy-coffee", async (HttpContext context) =>
{
    UserEndpoints service = new UserEndpoints();
    return await service.BuyCoffee(context);
});

app.MapGet("/user/get-transaction", (HttpContext context) =>
{
    UserEndpoints service = new UserEndpoints();
    return service.GetTransaction(context);
});

app.MapGet("/user/logout", (HttpContext context) =>
{
    UserEndpoints service = new UserEndpoints();
    return service.Logout(context);
});


app.Run();
