using Azure.Messaging.ServiceBus;
using Microsoft.EntityFrameworkCore;
using VerificationService.Data;
using VerificationService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure();
        }
    )
);

builder.Services.AddSingleton(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();

    var connectionString = config["Azure:ServiceBus:ConnectionString"];
    var queueName = config["Azure:ServiceBus:QueueName"];

    Console.WriteLine($"ConnectionString: {connectionString}");
    Console.WriteLine($"QueueName: {queueName}");

    return new ServiceBusClient(connectionString);
});

builder.Services.AddSingleton<ServiceBusPublisher>();
builder.Services.AddScoped<VerificationCodeService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();