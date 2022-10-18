module Mqtt

open MQTTnet
open MQTTnet.Client

type MqttSettings =
    { Server: string;
      Port: int;
      User: string;
      Password: string;
      ClientId: string;
      UseTls: bool }

let connect mqttSettings =
    let client = MqttFactory().CreateMqttClient()
    let clientOptionsBuilder =
      MqttClientOptionsBuilder()
        .WithTcpServer(mqttSettings.Server, mqttSettings.Port)
        .WithCredentials(mqttSettings.User, mqttSettings.Password)
        .WithClientId(mqttSettings.ClientId)

    if (mqttSettings.UseTls) then clientOptionsBuilder.WithTls() |> ignore

    let connectionResult = client.ConnectAsync(clientOptionsBuilder.Build()) |> Async.AwaitTask |> Async.RunSynchronously
    client

let publish (client: IMqttClient) topic (payload: string) =
  client.PublishAsync(MqttApplicationMessageBuilder()
    .WithTopic(topic)
    .WithContentType("application/json")
    .WithPayload(payload)
    .WithRetainFlag(true)
    .Build())
  |> Async.AwaitTask
  |> Async.RunSynchronously

//let baseTopic = "energyassistant"
//let topic name = sprintf "%s/%s/%s" baseTopic configData.Carnot.Region name
