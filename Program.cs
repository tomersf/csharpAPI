using System.Text;
using api.Data;
using api.Services;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
Env.Load();

var STOCK_COLLECTION_NAME = Environment.GetEnvironmentVariable("STOCK_COLLECTION_NAME");
var COMMENT_COLLECTION_NAME = Environment.GetEnvironmentVariable("COMMENT_COLLECTION_NAME");
var USER_COLLECTION_NAME = Environment.GetEnvironmentVariable("USER_COLLECTION_NAME");
var DATABASE_NAME = Environment.GetEnvironmentVariable("DATABASE_NAME");
var CONNECTION_STRING = Environment.GetEnvironmentVariable("CONNECTION_STRING");

var databaseSettings = new DatabaseSettings
{
    CommentCollectionName = COMMENT_COLLECTION_NAME,
    StockCollectionName = STOCK_COLLECTION_NAME,
    UserCollectionName = USER_COLLECTION_NAME,
    ConnectionString = CONNECTION_STRING,
    DatabaseName = DATABASE_NAME
};

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Abra API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY");
var key = Encoding.ASCII.GetBytes(jwtKey);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});


builder.Services.AddSingleton(databaseSettings);
builder.Services.AddSingleton<ApplicationDBContext>();
builder.Services.AddScoped<StockService>();
builder.Services.AddScoped<CommentService>();
builder.Services.AddScoped<TokenService>();

builder.Services.AddControllers();

builder.Services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    );

                options.AddPolicy("signalr",
                    builder => builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()

                    .AllowCredentials()
                    .SetIsOriginAllowed(hostName => true));
            });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// app.UseHttpsRedirection();
// app.UseCors(cors => cors.AllowAnyHeader()
//                     .AllowAnyMethod()
//                     .AllowAnyOrigin()
//                     .AllowCredentials()
//                     .SetIsOriginAllowed(origin => true)
//                     );
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();