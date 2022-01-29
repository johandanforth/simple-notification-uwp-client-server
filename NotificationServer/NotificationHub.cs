using System.Diagnostics;

using Microsoft.AspNetCore.SignalR;

using Notification.Core;

namespace NotificationServer
{
	public class NotificationHub : Hub
	{
		public NotificationHub()
		{
			Trace.TraceInformation("Hub created");
		}

		public override async Task OnConnectedAsync()
		{
			Trace.TraceInformation(Context.ConnectionId + " connected");
			await Groups.AddToGroupAsync(Context.ConnectionId, "SignalR Users");

			await Clients.Caller.SendAsync(nameof(HubMessages.Connected));
			await base.OnConnectedAsync();
		}

		public override async Task OnDisconnectedAsync(Exception? exception)
		{
			Trace.TraceInformation(Context.ConnectionId + " disconnected");
			await Groups.RemoveFromGroupAsync(Context.ConnectionId, "SignalR Users");
			await base.OnDisconnectedAsync(exception);
		}

		/// <summary>
		///     Server receives a notification from a client
		/// </summary>
		/// <param name="topic"></param>
		/// <param name="message"></param>
		/// <returns></returns>
		public async Task NotificationFromClient(string topic, string message)
		{
			// forward to all clients
			var name = nameof(HubMessages.NotificationFromServer);
			await Clients.All.SendAsync(name, topic, message);
		}
	}
}