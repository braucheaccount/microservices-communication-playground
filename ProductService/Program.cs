using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Repositories;

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
