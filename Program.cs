using HyperBean.Services.AdminServices;
using HyperBean.Helpers.AdminHelpers;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSession();
builder.Services.AddDistributedMemoryCache();

var app = builder.Build();
app.UseRouting();
app.UseSession();
app.UseStaticFiles();

app.MapGet("/admin/validate-signin", () =>
{
    
});

app.Run();
