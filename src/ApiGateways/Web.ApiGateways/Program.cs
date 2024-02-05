using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
 

var builder = WebApplication.CreateBuilder(args);




builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOcelot().AddConsul();


builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath + "/Configurations/")
    .AddJsonFile("ocelot.json", false, true)
    .AddEnvironmentVariables();

var app = builder.Build();
 
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
 
app.UseAuthorization();

app.MapControllers();
await app.UseOcelot();
app.Run();
