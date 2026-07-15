using NotificationsAPI.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddNotificationsApi(builder.Configuration);

var app = builder.Build();

await app.InitializeNotificationsDatabaseAsync();
app.UseNotificationsApi();

await app.RunAsync();
