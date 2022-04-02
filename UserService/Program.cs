using MassTransit;
using Microsoft.EntityFrameworkCore;
using SharedLogic;
using SharedLogic.Models;
using SharedLogic.Saga;
using UserService.Data;
using UserService.Repositories;
using UserService.StateMachines;

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

    x.AddSagaStateMachine<OrderStateMachine, OrderStateInstance>()
        .InMemoryRepository();

    // request only
    x.AddRequestClient<RequestResponse>();

    x.AddRequestClient<CheckOrderStatusResponse>();
    x.AddRequestClient<OrderNotFoundResponse>();


    x.UsingRabbitMq((context, config) =>
    {
        // -- send specific
        config.Host(new Uri("rabbitmq://localhost/queue:send-example-queue"), h =>
        {
            h.Username(RabbitMqSettings.Username);
            h.Password(RabbitMqSettings.Password);
        });

        // -- publish specific
        config.Host(new Uri("rabbitmq://localhost"), h =>
        {
            h.Username(RabbitMqSettings.Username);
            h.Password(RabbitMqSettings.Password);

        });

        // -- saga specific
        config.ReceiveEndpoint("order.saga", e =>
        {
            e.ConfigureSaga<OrderStateInstance>(context);
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
