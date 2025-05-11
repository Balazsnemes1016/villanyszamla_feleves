using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// 🔹 CORS engedélyezés
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

// 🔹 Kontroller szolgáltatás regisztrálása
builder.Services.AddControllers();

var app = builder.Build();

// 🔹 CORS használata
app.UseCors("AllowAll");

// 🔹 Routing és kontroller aktiválása
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();
