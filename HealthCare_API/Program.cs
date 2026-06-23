using AutoMapper;
using HealthCare_API.Data;
using HealthCare_API.Mappings;
using HealthCare_API.Extensions;
using Microsoft.EntityFrameworkCore;
using HealthCare_API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
//Profiles
builder.Services.AddAutoMapper(typeof(PatientProfile));
builder.Services.AddAutoMapper(typeof(DoctorProfile));
//Services and Repositories
builder.Services.AddAppServices();
//DB context conecction
builder.Services.AddDbContext<HealthCareDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


var app = builder.Build();

//Auto migrations
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<HealthCareDbContext>();
    db.Database.Migrate();

}

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
    }

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseCors("AllowAngular");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
