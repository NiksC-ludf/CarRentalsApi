using CarRentals.Data;
using CarRentals.Repositories;
using CarRentals.Services.BestRentals;
using CarRentals.Services.CarOfferAggregator;
using CarRentals.Services.NorthernRentals;
using CarRentals.Services.SouthRentals;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<ApplicationDbContext>();
builder.Services.AddHttpClient<IBestRentalsService, BestRentalsService>();
builder.Services.AddHttpClient<INorthernRentalsService, NorthernRentalsService>();
builder.Services.AddHttpClient<ISouthRentalsService, SouthRentalsService>();
builder.Services.AddScoped<ICarOfferAggregatorService, CarOfferAggregatorService>();
builder.Services.AddScoped<ICarOfferRetrievalService, CarOfferRetrievalService>();
builder.Services.AddScoped<ICarOfferRepository, CarOfferRepository>();
builder.Services.AddLogging();
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
}); ;

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
