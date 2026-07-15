using MediatR;
using NotificationsAPI.Domain.Notifications;

namespace NotificationsAPI.Application.Notifications;

public class SendNotificationHandler(INotificationRepository r, ILogger<SendNotificationHandler> log) : IRequestHandler<SendNotificationCommand, NotificationDto>
{
    public async Task<NotificationDto> Handle(SendNotificationCommand c, CancellationToken ct)
    {
        if (await r.ExistsAsync(c.CorrelationId, c.Subject, ct))
            return new NotificationDto
            {
                CorrelationId = c.CorrelationId,
                Recipient = c.Recipient,
                Subject = c.Subject,
                Message = c.Message,
            };
        var n = new Notification(c.CorrelationId, c.Recipient, c.Subject, c.Message);
        r.Add(n);
        await r.SaveAsync(ct);
        log.LogInformation("E-MAIL | Para: {Recipient} | {Subject} | {Message}", n.Recipient, n.Subject, n.Message);
        return Mapping.ToDto(n);
    }
}
