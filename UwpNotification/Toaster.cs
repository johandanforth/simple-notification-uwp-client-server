using Windows.UI.Core;
using Windows.UI.Xaml;

using Microsoft.Toolkit.Uwp.Notifications;

namespace NotificationClient
{
	public static class Toaster
	{
		public static void PopToast(string topic, string message)
		{
			_ = Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
				() =>
				{
					// Generate the toast notification content and pop the toast
					new ToastContentBuilder().SetToastScenario(ToastScenario.Reminder)
						.AddArgument("action",
							"viewEvent")
						.AddArgument("eventId",
							1983)
						// .AddHeroImage()
						.AddText(topic,
							AdaptiveTextStyle.Header)
						.AddText(message,
							AdaptiveTextStyle.Body)
						.AddComboBox("snoozeTime",
							"15",
							("1", "1 minute"),
							("15", "15 minutes"),
							("60", "1 hour"),
							("240", "4 hours"),
							("1440", "1 day"))
						.AddButton(new ToastButton().SetSnoozeActivation("snoozeTime"))
						.AddButton(new ToastButton().SetDismissActivation())
						.Show();
				});
		}

		public static void ConnectedToast()
		{
			_ = Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
				() =>
				{
					// Generate the toast notification content and pop the toast
					new ToastContentBuilder().SetToastScenario(ToastScenario.Default)
						.AddText("Connected",
							AdaptiveTextStyle.Header)
						.AddText("Connected to notification service",
							AdaptiveTextStyle.Body)
						.Show();
				});
		}
	}
}