using MassTransit;
using Microsoft.EntityFrameworkCore;
using ProductService;
using ProductService.Consumers;
using ProductService.Data;
using ProductService.Repositories;
using SharedLogic;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddDbContext<ProductContext>(opts =>
{
    var str = builder.Configuration.GetConnectionString("DefaultConnection");
    opts.UseMySql(str, Microsoft.EntityFrameworkCore.ServerVersion.AutoDetect(str));
});



builder.Services.Configure<RouteOptions>(opts =>
{
    opts.LowercaseUrls = true;
});


builder.Services.AddTransient<IProductRepository, ProductRepository>();



builder.Services.AddMassTransit(x =>
{
    // add a single consumer
    x.AddConsumer<PublishConsumer>();
    x.AddConsumer<SendConsumer>();
    x.AddConsumer<RequestConsumer>();
    x.AddConsumer<ExceptionConsumer>();
    x.AddConsumer<ExceptionFaultConsumer>();

    x.AddConsumer<ReserveProductConsumer>();
    x.AddConsumer<OrderFailedConsumer>();

    // or add all consumers in a specified assembly automatically
    //x.AddConsumers(Assembly.GetEntryAssembly());

    x.UsingRabbitMq((context, config) =>
    {
        // default rabbitmq setup
        config.Host(new Uri("rabbitmq://localhost"), h =>
        {
            h.Username(RabbitMqSettings.Username);
            h.Password(RabbitMqSettings.Password);
        });

         // -- saga endpoints
        config.ReceiveEndpoint("product.order.received", e =>
        {
            e.ConfigureConsumer<ReserveProductConsumer>(context);
        });
        
        config.ReceiveEndpoint("product.order.failed", e =>
        {
            e.ConfigureConsumer<OrderFailedConsumer>(context);
        });


        // -- send example configuration
        config.ReceiveEndpoint("queue:send-example-queue", e =>
        {
            e.ConfigureConsumer<SendConsumer>(context);
        });


        // -- publish example configuration
        config.ReceiveEndpoint("request-example-queue", e =>
        {
            e.ConfigureConsumer<PublishConsumer>(context);
        });


        // -- request response example
        config.ReceiveEndpoint("request-response-example-queue", e =>
        {
            e.ConfigureConsumer<RequestConsumer>(context);
        });


        // -- exception example
        config.ReceiveEndpoint("exception-example-queue", e =>
        {
            e.ConfigureConsumer<ExceptionConsumer>(context);
            //e.ConfigureConsumer<ExceptionFaultConsumer>(context);
        });

        // listen for exception and handle the problem
        config.ReceiveEndpoint("exception-example-queue_error", e =>
        {
            e.ConfigureConsumer<ExceptionFaultConsumer>(context);
        });
    });

});

builder.Services.AddHostedService<MyBackgroundService>();


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
