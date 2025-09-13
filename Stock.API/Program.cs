using MassTransit;
using ServiceBus;
using Stock.Service.Consumers;
using Bus = ServiceBus.Bus;
using IBus = ServiceBus.IBus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IBus, Bus>();
//builder.Services.AddHostedService<OrderCreatedEventConsumerBackgroundService>();

builder.Services.Configure<BusOption>(builder.Configuration.GetSection(nameof(BusOption)));


builder.Services.AddMassTransit(configure =>
{
    configure.AddConsumer<OrderCreatedEventConsumer>();

    configure.UsingRabbitMq((context, cfg) =>
    {


        cfg.UseMessageRetry(r => r.Interval(5, TimeSpan.FromSeconds(5))); // 5 kere dene 5 saniye arayla

        cfg.UseInMemoryOutbox(context); // ayný mesajýn birden fazla iþlenmesini engeller.

        var busOptions = builder.Configuration.GetSection(nameof(BusOption)).Get<BusOption>();

        cfg.Host(new Uri(busOptions!.Url));

        // create order created event queue endpoint
        cfg.ReceiveEndpoint(BusConst.StockOrderCreatedEventQueue, e =>
        {
           e.Consumer<OrderCreatedEventConsumer>(context);
        });


        //cfg.ConfigureEndpoints(context); => otomatik olarak tüm consumer larý tarar ve hepsi için endpoint oluþturur.
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
