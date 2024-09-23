using CoreIdentity.Data;
using CoreIdentity.Helpers;
using CoreIdentity.Models.Email;
using CoreIdentity.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
{
  options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<EMailHelper>();

builder.Services.AddIdentity<AppUser, AppRole>(options =>
{
  options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
  options.User.RequireUniqueEmail = true;

  options.Password.RequireDigit = true;
  options.Password.RequiredLength = 8;
  options.Password.RequiredUniqueChars = 1;
  options.Password.RequireLowercase = true;
  options.Password.RequireUppercase = true;
  options.Password.RequireNonAlphanumeric = false;

  options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
  options.Lockout.MaxFailedAccessAttempts = 5;

  options.SignIn.RequireConfirmedEmail = true;

}).AddEntityFrameworkStores<AppDbContext>()
  .AddDefaultTokenProviders();



//uilder.Services.AddDbContext<AppDbContext>(options =>
//{
//  options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
//});

// Add services to the container.
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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
