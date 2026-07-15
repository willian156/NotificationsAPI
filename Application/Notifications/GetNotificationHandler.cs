using MediatR;
using NotificationsAPI.Domain.Notifications;

namespace NotificationsAPI.Application.Notifications;

public class GetNotificationHandler(INotificationRepository r) : IRequestHandler<GetNotificationQuery, NotificationDto?>
{
    public async Task<NotificationDto?> Handle(GetNotificationQuery q, CancellationToken ct)
    {
        var n = await r.GetAsync(q.Id, ct);
        return n is null ? null : Mapping.ToDto(n);
    }
}
