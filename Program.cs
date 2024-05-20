using api.Data;
using api.Services;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);
Env.Load();

var stockCollectionName = Environment.GetEnvironmentVariable("STOCK_COLLECTION_NAME");
var COMMENT_COLLECTION_NAME = Environment.GetEnvironmentVariable("COMMENT_COLLECTION_NAME");
var DATABASE_NAME = Environment.GetEnvironmentVariable("DATABASE_NAME");
var CONNECTION_STRING = Environment.GetEnvironmentVariable("CONNECTION_STRING");

var databaseSettings = new DatabaseSettings
{
    CommentCollectionName = COMMENT_COLLECTION_NAME,
    StockCollectionName = stockCollectionName,
    ConnectionString = CONNECTION_STRING,
    DatabaseName = DATABASE_NAME
};

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSingleton(databaseSettings);
builder.Services.AddSingleton<ApplicationDBContext>();
builder.Services.AddScoped<StockService>();
builder.Services.AddScoped<CommentService>();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// app.UseHttpsRedirection();

app.MapControllers();

app.Run();