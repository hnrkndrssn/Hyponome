using Hyponome.Web.Models;

var builder = WebApplication.CreateBuilder(args);

// Add local app configuration
builder.Configuration.AddJsonFile("appsettings.Local.json", true);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.Configure<GitHubOptions>(builder.Configuration.GetSection("GitHub"));
builder.Services.AddSingleton(typeof(IGitHubClientService), typeof(GitHubClientService));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/404.html");
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
