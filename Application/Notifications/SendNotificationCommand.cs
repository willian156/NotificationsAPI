using MediatR;
using NotificationsAPI.Domain.Notifications;

namespace NotificationsAPI.Application.Notifications;

public class SendNotificationCommand : IRequest<NotificationDto>
{
    public Guid CorrelationId { get; set; }
    public string Recipient { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}
