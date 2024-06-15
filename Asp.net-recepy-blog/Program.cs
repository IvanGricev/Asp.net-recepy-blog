// Usings
using Microsoft.EntityFrameworkCore;
using Asp.net_recepy_blog.Pages.Dbcontrol;
using Asp.net_recepy_blog.Pages.Services;


// Builder decloration
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();

builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("MyDatabase"),
        new MySqlServerVersion(new Version(8, 0, 21))));

// Builder scoped
builder.Services.AddScoped<IUserServices, UserServices>();
builder.Services.AddScoped<UserServices>();
builder.Services.AddScoped<IPostsServices, PostsServices>();
builder.Services.AddScoped<PostsServices>();

// Email
builder.Services.AddSingleton(new EmailServices("smtp.yandex.ru", 465, "ivangricev@yandex.com", "zglmlrdoheigzhmk"));

// Cookie
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseSession();

app.UseRouting();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Rest
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
