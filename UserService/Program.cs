using MassTransit;
using Microsoft.EntityFrameworkCore;
using SharedLogic;
using SharedLogic.Models;
using UserService.Data;
using UserService.Repositories;

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

builder.Services.AddMassTransit(x =>
{
    // request only
    x.AddRequestClient<RequestResponse>();


    x.UsingRabbitMq((context, config) =>
    {
        // send
        config.Host(new Uri($"{RabbitMqSettings.RabbitMqUri}/queue:send-example-queue"), h =>
        {
            h.Username(RabbitMqSettings.Username);
            h.Password(RabbitMqSettings.Password);
        });

        // publish
        config.Host(new Uri(RabbitMqSettings.RabbitMqUri), h =>
        {
            h.Username(RabbitMqSettings.Username);
            h.Password(RabbitMqSettings.Password);
        });
    });
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
