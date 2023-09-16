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
    string primaryKey = builder.Configuration.GetSection("").GetValue<string>("g3kxaCxaHwh0sHM15ojFnq80rU13a5LLCOhXqcT99boZTUNlWzMBBIexMIYB4OZt9Z7lYjrgLB6UACDbgkupAA==");
    string dbName = builder.Configuration.GetSection("").GetValue<string>("ASBS");
    string containerName = builder.Configuration.GetSection("").GetValue<string>("SpotPhysio");

    var cosmosClient = new Microsoft.Azure.Cosmos.CosmosClient("https://asbs.documents.azure.com:443/", "g3kxaCxaHwh0sHM15ojFnq80rU13a5LLCOhXqcT99boZTUNlWzMBBIexMIYB4OZt9Z7lYjrgLB6UACDbgkupAA==");

    return new PatientService(cosmosClient, "ASBS", "SpotPhysio");
});

builder.Services.AddSingleton<IPhysiotherapistService>(options =>
{
    string url = builder.Configuration.GetSection("").GetValue<string>("https://asbs.documents.azure.com:443/");
    string primaryKey = builder.Configuration.GetSection("").GetValue<string>("g3kxaCxaHwh0sHM15ojFnq80rU13a5LLCOhXqcT99boZTUNlWzMBBIexMIYB4OZt9Z7lYjrgLB6UACDbgkupAA==");
    string dbName = builder.Configuration.GetSection("").GetValue<string>("ASBS");
    string containerName = builder.Configuration.GetSection("").GetValue<string>("SpotPhysio");

    var cosmosClient = new Microsoft.Azure.Cosmos.CosmosClient("https://asbs.documents.azure.com:443/", "g3kxaCxaHwh0sHM15ojFnq80rU13a5LLCOhXqcT99boZTUNlWzMBBIexMIYB4OZt9Z7lYjrgLB6UACDbgkupAA==");

    return new PhysiotherapistService(cosmosClient, "ASBS", "SpotPhysio");
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