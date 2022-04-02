using MassTransit;
using SharedLogic;
using ShipmentService.Consumers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<ShipmentConsumer>();

    x.UsingRabbitMq((context, config) =>
    {
        // default rabbitmq setup
        config.Host(new Uri("rabbitmq://localhost"), h =>
        {
            h.Username(RabbitMqSettings.Username);
            h.Password(RabbitMqSettings.Password);
        });

        config.ReceiveEndpoint("product.order.shipment", e =>
        {
            e.ConfigureConsumer<ShipmentConsumer>(context);
        });

    });

});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
