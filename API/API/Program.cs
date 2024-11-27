using API.DAO;
using API.DAO.Interface;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IReservationDAO>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    return new ReservationDAO(configuration.GetConnectionString("DefaultConnectionString"));
});

builder.Services.AddScoped<IUserDAO>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    return new UserDAO(configuration.GetConnectionString("DefaultConnectionString"));
});

// Add services to the container.

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

app.UseAuthorization();

app.MapControllers();

app.Run();
