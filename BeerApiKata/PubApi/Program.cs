using BeerApiKata.Infrastructure.Repository.InMemRepository;
using BeerApiKata.Infrastructure.Repository.Interfaces;
using PubApi.Beer.Interfaces;
using PubApi.Beer.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton(typeof(IGenericRepository<,>), typeof(InMemAsyncRepository<,>));
builder.Services.AddTransient<IBeerService, BeerService>();


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



