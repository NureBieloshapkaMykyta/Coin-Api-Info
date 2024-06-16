using Coin_Api_Info.Extensions;
using Coin_Api_Info.MappingProfiles;
using Infrastructure;
using Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddPersistence(builder.Configuration, "DefaultConnection");
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddAutoMapper(cfg => 
{
    cfg.AddProfile<CryptoCurrencyProfile>();
});

var app = builder.Build();

await app.ApplyMigration();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();