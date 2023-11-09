using Microsoft.EntityFrameworkCore;
using WeddingPlanner.Models;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();

builder.Services.AddDbContext<MyContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});
builder.Services.AddScoped<WeddingCleanService>();
var app = builder.Build();

var cleanupServiceFactory = app.Services.GetRequiredService<IServiceScopeFactory>(); 
//Eliminar las bodas que ya caducaron
var interval = TimeSpan.FromDays(1); // Ejecución diaria
var timer = new System.Threading.Timer(_ =>
{
    using (var scope = cleanupServiceFactory.CreateScope())
    {
        var scopedCleanupService = scope.ServiceProvider.GetRequiredService<WeddingCleanService>();
        scopedCleanupService.RemoveExpiredWeddingsAsync().Wait(); 
    }
}, null, TimeSpan.Zero, interval);


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



app.Run();
