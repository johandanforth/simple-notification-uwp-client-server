using System.Threading.Tasks;

namespace NotificationClient;

public interface INotificationHubClient
{
	Task Connect();
	Task SendNotificationToServer(string topic, string message);
   
}