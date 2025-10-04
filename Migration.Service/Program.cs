using Microsoft.EntityFrameworkCore;
using Migration.Service;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddDbContext<Docker.API.Models.AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});


var host = builder.Build();
host.Run();
