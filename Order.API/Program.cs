using MassTransit;
using Order.Service;
using Order.Service.Services;
using Polly;
using Polly.Extensions.Http;
using ServiceBus;
using Bus = ServiceBus.Bus;
using IBus = ServiceBus.IBus;




var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//retry policy
// burada transient hatalarý yakalýyoruz ve belirli aralýklarla tekrar deniyoruz
// örneðin 500, 502, 503, 504 gibi hatalar
var retryPolicy = HttpPolicyExtensions
    .HandleTransientHttpError()
    .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));  // burada  exponential backoff yapýyoruz yani 2^n saniye bekliyoruz daha sonra deniyoruz


//circuit brekar
// belirli sayýda baþarýsýz denemeden sonra devreyi açýyoruz ve belirli bir süre bekliyoruz
var circuitBreakerPolicy = HttpPolicyExtensions
    .HandleTransientHttpError()
    .CircuitBreakerAsync(2, TimeSpan.FromSeconds(30)); // 3 baþarýsýz denemeden sonra devreyi aç ve 30 saniye bekle

var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(20)); // 20 saniye timeout süresi


var combindedPolicy = Policy.WrapAsync(retryPolicy, circuitBreakerPolicy, timeoutPolicy);


builder.Services.AddHttpClient<StockService>(options =>
{
    options.BaseAddress = new Uri(builder.Configuration.GetSection("MicroservicesBaseUrl")["Stock"]!);
}).AddPolicyHandler(combindedPolicy);



builder.Services.AddSingleton<IBus, Bus>();
builder.Services.AddScoped<IOrderService, OrderService>();


builder.Services.AddMassTransit(configure=>
{
    

    configure.UsingRabbitMq((context, cfg) =>
    {
        var busOptions = builder.Configuration.GetSection(nameof(BusOption)).Get<BusOption>();

        cfg.Host(new Uri(busOptions!.Url));

        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.Configure<BusOption>(builder.Configuration.GetSection(nameof(BusOption)));

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
