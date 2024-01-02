using ChauffeurApp.DataAccess.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using ChauffeurApp.Application.MappingProfiles;
using ChauffeurApp.API.Extensions;
using ChauffeurApp.Core.Entities;
using Microsoft.AspNetCore.Identity;
using ChauffeurApp.API.Exceptions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer
(builder.Configuration.GetConnectionString("DefaultConnection")
));
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{

    //seed roles
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationUserRole>>();

    var roles = new[]
    {
        "Admin",
        "Passenger",
        "Chauffeur",
        "Company"
    };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new ApplicationUserRole(role));
        }
    }
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
//img upload
app.UseStaticFiles();

app.MapControllers();

app.Run();
