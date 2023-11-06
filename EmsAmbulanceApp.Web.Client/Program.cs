using EmsAmbulanceApp.Web.Client.Application.Data;
using EmsAmbulanceApp.Web.Client.Application.Infrastructure.Repositories;
using EmsAmbulanceApp.Web.Client.Application.Infrastructure.Services;
using EmsAmbulanceApp.Web.Client.Domain.Entities;
using EmsAmbulanceApp.Web.Client.Domain.Interfaces.IRepositories;
using EmsAmbulanceApp.Web.Client.Domain.Interfaces.IServices;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<EmsAmbulanceAppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddTransient<IOtpService, OtpService>();
builder.Services.AddTransient<IEmsAmbulanceAppUnitOfWork, EmsAmbulanceAppUnitOfWork>();

builder.Services.AddIdentity<Client, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<EmsAmbulanceAppDbContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings, lockout settings, user settings, etc.
    // ...
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme);
builder.Services.ConfigureApplicationCookie(opions => opions.LoginPath = "/Account/Index");

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
