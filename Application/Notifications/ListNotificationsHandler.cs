using MediatR;
using NotificationsAPI.Domain.Notifications;

namespace NotificationsAPI.Application.Notifications;

public class ListNotificationsHandler(INotificationRepository r) : IRequestHandler<ListNotificationsQuery, IReadOnlyList<NotificationDto>>
{
    public async Task<IReadOnlyList<NotificationDto>> Handle(ListNotificationsQuery q, CancellationToken ct) => (await r.ListAsync(ct)).Select(Mapping.ToDto).ToList();
}
