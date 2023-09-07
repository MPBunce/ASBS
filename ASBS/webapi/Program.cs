using webapi.Service;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

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
    string url = builder.Configuration.GetSection("").GetValue<string>("https://asbs.documents.azure.com:443/");
    string primaryKey = builder.Configuration.GetSection("").GetValue<string>("TnxTI79r47EJ3MG4c8zVGSVdaspyVSvLanzq6bia3nQHZZDtBbWWi37g6d9yOu6xYoZCai9DsMVGACDbR4EaRg==");
    string dbName = builder.Configuration.GetSection("").GetValue<string>("ASBS");
    string containerName = builder.Configuration.GetSection("").GetValue<string>("SpotPhysio");

    var cosmosClient = new Microsoft.Azure.Cosmos.CosmosClient("https://asbs.documents.azure.com:443/", "TnxTI79r47EJ3MG4c8zVGSVdaspyVSvLanzq6bia3nQHZZDtBbWWi37g6d9yOu6xYoZCai9DsMVGACDbR4EaRg==");

    return new PatientService(cosmosClient, "ASBS", "SpotPhysio");
});



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