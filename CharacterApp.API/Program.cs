using CharacterApp.Data;
using CharacterApp.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<CharacterDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DemoDB")));
builder.Services.AddScoped<ISpeicesRepository, SpeicesRepository>();
builder.Services.AddScoped<ISpeicesService, SpeicesService>();
builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped<IItemService, ItemService>();

builder.Services.AddControllers();
var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI();
// }

app.UseHttpsRedirection();

app.MapControllers();

app.MapGet("/", () => "pong!")
.WithName("Health Check")
.WithOpenApi();

app.Run();