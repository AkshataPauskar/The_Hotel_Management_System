using Hotel_Management_System.Models;
using Hotel_Management_System.Services;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<HotelManagementDatabaseSettings>(builder.Configuration.GetSection(nameof(HotelManagementDatabaseSettings)));

builder.Services.AddSingleton<IHotelManagementDatabaseSettings>(sp=>sp.GetRequiredService<IOptions<HotelManagementDatabaseSettings>>().Value);

builder.Services.AddSingleton<IMongoClient>(s=>new MongoClient(builder.Configuration.GetValue<string>("HotelManagementDatabaseSettings:ConnectionString")));

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IReservationsService, ReservationsService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
