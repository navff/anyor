using System.Text.Encodings.Web;
using System.Text.Unicode;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

var port = Environment.GetEnvironmentVariable("PORT");
port = port ?? "8080";
builder.WebHost.UseUrls($"http://localhost:{port}");

var directory = Directory.GetCurrentDirectory();

// Add services to the container.
builder.Services.AddRazorPages(options =>
    {
        // отключаем глобально Antiforgery-токен
        options.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute());

        // Custom routes
        // options.Conventions.AddPageRoute("/Mk/MkList", "/mk");
    })
    .AddRazorRuntimeCompilation(options => options.FileProviders.Add(new PhysicalFileProvider(directory)));

builder.Services.AddSingleton(HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic }));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();


var folderPath = Environment.GetEnvironmentVariable("FOLDER_PATH");
if (!string.IsNullOrEmpty(folderPath))
{
    app.UsePathBase($"/{folderPath}");
}


app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();