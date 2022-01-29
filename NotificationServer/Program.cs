using NotificationServer;

// Signal R hub https://docs.microsoft.com/en-us/aspnet/core/signalr/hubs?view=aspnetcore-6.0

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
services.AddSignalR();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapHub<NotificationHub>("/notificationhub");

app.Run();