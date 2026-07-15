using MediatR;
using NotificationsAPI.Domain.Notifications;

namespace NotificationsAPI.Application.Notifications;

public static class Mapping
{
    public static NotificationDto ToDto(Notification n) => new()
    {
        Id = n.Id,
        CorrelationId = n.CorrelationId,
        Recipient = n.Recipient,
        Subject = n.Subject,
        Message = n.Message,
        SentAt = n.SentAt,
    };
}
