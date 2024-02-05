using IdentityService.Api.Extensions;
using IdentityService.Api.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
 builder.Services.ConfigureConsul(builder.Configuration);
builder.Services.AddScoped<IIdentityService, IdentityService.Api.Application.Services.IdentityService>();


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
app.RegisterWithConsul(app.Lifetime, builder.Configuration);
app.Run(); 
