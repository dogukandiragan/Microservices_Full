
using CatalogService.Api.Extensions.CatalogService.Api.Extensions;
using CatalogService.Api.Infrastructure;
using CatalogService.Api.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;


var webApplicationOptions = new WebApplicationOptions
{
    Args = args,
    WebRootPath = "pics",
    ContentRootPath = Directory.GetCurrentDirectory()
};
 
var builder = WebApplication.CreateBuilder(webApplicationOptions);


builder.Services.Configure<CatalogSettings>(builder.Configuration.GetSection("CatalogSettings"));


// Add services to the container.
 
builder.Services.AddControllers();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

 builder.Services.ConfigureConsul(builder.Configuration);

 
builder.Services.AddDbContext<CatalogContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

 

app.UseAuthorization();

app.MapControllers();
app.RegisterWithConsul(app.Lifetime, builder.Configuration);
app.Run();

