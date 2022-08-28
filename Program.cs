using Microsoft.EntityFrameworkCore;
using Razor_Test.AdditionalEntities;
using Razor_Test.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDbContext<RazorAutomarketDbContext>(options => options
.UseSqlite(builder.Configuration.GetConnectionString("Sqlite")));
builder.Services.AddSingleton<SettingsMaster>();
builder.Services.AddTransient<CryptographyHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();
app.Run();