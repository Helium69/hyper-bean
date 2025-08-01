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


app.Run();
