using MediatR;
using NotificationsAPI.Domain.Notifications;

namespace NotificationsAPI.Application.Notifications;

public class ListNotificationsQuery : IRequest<IReadOnlyList<NotificationDto>>
{
}
