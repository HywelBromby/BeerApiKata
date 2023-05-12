using BeerApiKata.Infrastructure.Repository.InMemRepository;
using BeerApiKata.Infrastructure.Repository.Interfaces;
using BeerApiKata.Infrastructure.Validation.GenericFluentValidator;
using BeerApiKata.Infrastructure.Validation.Interfaces;
using PubApi.Beer.Interfaces;
using PubApi.Beer.Services;
using PubApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WireUpDependancies(builder);

var app = builder.Build();

WireUpMiddleware(app);


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

static void WireUpDependancies(WebApplicationBuilder builder)
{
    builder.Services.AddSingleton(typeof(IGenericRepository<,>), typeof(InMemAsyncRepository<,>));
    builder.Services.AddTransient<IBeerService, BeerService>();
    builder.Services.AddSingleton(typeof(IGenericValidator<>), typeof(GenericFluentValidator<>));
    
}

static void WireUpMiddleware(WebApplication app)
{
    app.UseMiddleware<ExceptionHandlingMiddleware>();
}