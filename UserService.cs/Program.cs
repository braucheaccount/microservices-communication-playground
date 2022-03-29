using Microsoft.EntityFrameworkCore;
using UserService.cs.Data;
using UserService.cs.Repositories;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<UserContext>(opts =>
{
    var str = builder.Configuration.GetConnectionString("DefaultConnection");
    opts.UseMySql(str, Microsoft.EntityFrameworkCore.ServerVersion.AutoDetect(str));
});

builder.Services.AddTransient<IUserRepository, UserRepository>();

builder.Services.Configure<RouteOptions>(opts =>
{
    opts.LowercaseUrls = true;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
