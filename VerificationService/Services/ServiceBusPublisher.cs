using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Threading.Tasks;

public class ServiceBusPublisher
{
    private readonly ServiceBusSender _sender;

    public ServiceBusPublisher(ServiceBusClient client, IConfiguration config)
    {
        var queueName = config["Azure:ServiceBus:QueueName"];
        _sender = client.CreateSender(queueName);
    }

    public async Task SendVerificationEmailAsync(string email, string code)
    {
        var message = new ServiceBusMessage(
            JsonSerializer.Serialize(new { email, code })
        );

        await _sender.SendMessageAsync(message);
    }
}