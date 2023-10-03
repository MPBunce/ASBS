using webapi.Service;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

DotNetEnv.Env.Load();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//AUTH STUFF
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Minimal API",
        Version = "v1"
    });


    options.OperationFilter<SecurityRequirementsOperationFilter>();
});


builder.Services.AddAuthentication().AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateAudience = false,
        ValidateIssuer = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                builder.Configuration.GetSection("AppSettings:Token").Value!))
    };
});


// Connection to cosmodb
builder.Services.AddSingleton<IPatientService>(options =>
{
    string container = Environment.GetEnvironmentVariable("CONTAINER");
    string db = Environment.GetEnvironmentVariable("DB");
    string key = Environment.GetEnvironmentVariable("COSMOSKEY");

    var cosmosClient = new Microsoft.Azure.Cosmos.CosmosClient("https://asbs.documents.azure.com/", key);

    return new PatientService(cosmosClient, db, container);
});

builder.Services.AddSingleton<IPhysiotherapistService>(options =>
{
    string container = Environment.GetEnvironmentVariable("CONTAINER");
    string db = Environment.GetEnvironmentVariable("DB");
    string key = Environment.GetEnvironmentVariable("COSMOSKEY");

    var cosmosClient = new Microsoft.Azure.Cosmos.CosmosClient("https://asbs.documents.azure.com/", key);

    return new PhysiotherapistService(cosmosClient, db, container);
});

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI( c => c.SwaggerEndpoint(
        "/swagger/v1/swagger.json",
        "v1"
   ));
}


app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint(
    "/swagger/v1/swagger.json",
    "v1"
));


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();