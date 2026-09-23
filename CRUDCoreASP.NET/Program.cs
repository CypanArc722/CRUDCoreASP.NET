using CRUDCoreASP.NET.Database;
using CRUDCoreASP.NET.Models;
using CRUDCoreASP.NET.Services;
using Microsoft.EntityFrameworkCore;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<UserDatabaseContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("UserDB")));
//builder.Services.AddScoped<IUserServices, UserServices>();
builder.Services.AddHttpClient<IUserServices, UserServices>();
builder.Services.Configure<API>(builder.Configuration.GetSection("ExpressApi"));
builder.Services.AddScoped(sp =>
    sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<API>>().Value);
builder.Services.AddMemoryCache();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/ErrorViewModel");
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/Home/ErrorViewModel");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
