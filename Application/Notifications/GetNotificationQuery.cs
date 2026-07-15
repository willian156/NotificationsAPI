using MediatR;
using NotificationsAPI.Domain.Notifications;

namespace NotificationsAPI.Application.Notifications;

public class GetNotificationQuery : IRequest<NotificationDto?>
{
    public Guid Id { get; set; }
}
