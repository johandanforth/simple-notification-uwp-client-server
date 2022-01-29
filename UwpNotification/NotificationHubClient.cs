using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

using Microsoft.AspNetCore.SignalR.Client;

using Notification.Core;

namespace NotificationClient;

public class NotificationHubClient : INotificationHubClient
{
    private readonly string _message;
    private readonly List<string> _topics;
    private HubConnection _connection;


    public NotificationHubClient(string message, List<string> topics)
    {
        _message = message;
        _topics = topics;
    }

    public async Task Connect()
    {
        _connection = new HubConnectionBuilder()
            .WithUrl(new Uri("http://localhost:5036/notificationhub"))
            .WithAutomaticReconnect(
                new[] { TimeSpan.Zero, TimeSpan.Zero, TimeSpan.FromSeconds(10) })
            .Build();

        _connection.Closed += async error =>
        {
            await Task.Delay(new Random().Next(0,
                5) * 1000);
            await _connection.StartAsync();
        };
        _connection.Reconnecting += error =>
        {
            Debug.Assert(_connection.State == HubConnectionState.Reconnecting);

            Trace.TraceWarning("Reconnecting to message hub...");

            return Task.CompletedTask;
        };
        _connection.Reconnected += connectionId =>
        {
            Debug.Assert(_connection.State == HubConnectionState.Connected);

            Trace.TraceInformation("Reconnected!");

            return Task.CompletedTask;
        };

        // setup messages
        _connection.On(nameof(HubMessages.Connected),
            async () =>
            {
                Trace.TraceInformation("Connected");

                Toaster.ConnectedToast();

                //send message and exit
                if (!string.IsNullOrEmpty(_message))
                {
                    var topic = _message.Split(':')[0].ToLowerInvariant();
                    var message = _message.Split(':')[1];

                    var name = nameof(HubMessages.NotificationFromClient);
                    await _connection.InvokeAsync(name,
                        topic,
                        message);
                    //   App.Current.Exit();
                }
            });
        _connection.On<string, string>(nameof(HubMessages.NotificationFromServer),
            (topic, message) =>
            {
                Trace.TraceInformation("Got message: " + message);

                Toaster.PopToast(topic, message);

            });

        // start the connection
        await _connection.StartAsync();
    }

    public async Task SendNotificationToServer(string topic, string message)
    {
        var name = nameof(HubMessages.NotificationFromClient);
        await _connection.InvokeAsync(name,
            topic,
            message);
    }
}