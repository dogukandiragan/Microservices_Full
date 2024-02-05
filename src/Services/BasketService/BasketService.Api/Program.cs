using BasketService.Api.Core.Application.Repository;
using BasketService.Api.Core.Application.Services;
using BasketService.Api.Extensions;
using BasketService.Api.Extensions.Registration;
using BasketService.Api.Infrastructure.Repository;
using BasketService.Api.IntegrationEvents.EventHanders;
using BasketService.Api.IntegrationEvents.Events;
using EventBus.Base;
using EventBus.Base.Abstraction;
using EventBus.Factory;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
ConfigureServicesExt(builder.Services);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
void ConfigureServicesExt(IServiceCollection services)
{
    services.ConfigureAuth(builder.Configuration);
    services.AddSingleton(sp => sp.ConfigureRedis(builder.Configuration));
    services.ConfigureConsul(builder.Configuration);
    services.AddHttpContextAccessor();
    services.AddTransient<IBasketRepository, RedisBasketRepository>();
    services.AddTransient<IIdentityService, BasketService.Api.Core.Application.Services.IdentityService>();


    services.AddSingleton<IEventBus>(sp =>
        EventBusFactory.Create(new()
        {
            ConnectionRetryCount = 5,
            EventNameSuffix = "IntegrationEvent",
            SubscriberClientAppName = "BasketService",
            EventBusType = EventBusType.RabbitMQ
        }, sp)
    );

    services.AddTransient<OrderCreatedIntegrationEventHandler>();
}
void ConfigureSubscription(IServiceProvider serviceProvider)
{
    var eventBus = serviceProvider.GetRequiredService<IEventBus>();

    eventBus.Subscribe<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>();
}

app.RegisterWithConsul(app.Lifetime, builder.Configuration);
ConfigureSubscription(app.Services);

app.UseAuthorization();

app.MapControllers();

app.Run();
