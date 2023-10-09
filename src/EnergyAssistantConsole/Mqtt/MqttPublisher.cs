using System.Text.Json;
using System.Text.Json.Serialization;
using MQTTnet;
using MQTTnet.Client;
using UlfenDk.EnergyAssistant.Config;

namespace UlfenDk.EnergyAssistant.Mqtt;

public class MqttPublisher : IDisposable
{
    private readonly IMqttClient _client;
    // private readonly MqttClientOptions _options;
    
    public MqttPublisher(MqttOptions options)
    {
        _client = new MqttFactory().CreateMqttClient();
        var optionsBuilder = new MqttClientOptionsBuilder()
            .WithTcpServer(options.Server, options.Port)
            .WithCredentials(options.User, options.Password)
            .WithClientId(options.ClientId);

        if (options.UseTls == true) optionsBuilder.WithTlsOptions(builder => builder
            .UseTls()
            .WithAllowUntrustedCertificates(options.AllowUntrustedCertificate ?? false));

        var connectResult = _client.ConnectAsync(optionsBuilder.Build()).GetAwaiter().GetResult();
        if (connectResult.ResultCode != MqttClientConnectResultCode.Success)
        {
            var msg = $"Failed to connect to MQTT with error code: {connectResult.ResultCode}";
            Console.WriteLine(msg);
            throw new Exception(msg);
        }
    }

    public async Task PublishAsync<T>(string topic, T payload)
    {
        var ms = new MemoryStream();
        await JsonSerializer.SerializeAsync<T>(ms, payload, new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        });
        ms.Position = 0;

        await _client.PublishAsync(new MqttApplicationMessageBuilder()
            .WithTopic(topic)
            .WithContentType("application/json")
            .WithPayload(ms)
            .WithRetainFlag(true)
            .Build());
    }

    public void Dispose()
    {
        _client.Dispose();
    }
}