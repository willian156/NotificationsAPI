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

public class GetNotificationQuery : IRequest<NotificationDto?>
{
    public Guid Id { get; set; }
}

public class ListNotificationsQuery : IRequest<IReadOnlyList<NotificationDto>> { }

public class NotificationDto
{
    public Guid Id { get; set; }
    public Guid CorrelationId { get; set; }
    public string Recipient { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTimeOffset SentAt { get; set; }
}

public static class Mapping
{
    public static NotificationDto ToDto(Notification n) =>
        new()
        {
            Id = n.Id,
            CorrelationId = n.CorrelationId,
            Recipient = n.Recipient,
            Subject = n.Subject,
            Message = n.Message,
            SentAt = n.SentAt,
        };
}

public class SendNotificationHandler(
    INotificationRepository r,
    ILogger<SendNotificationHandler> log
) : IRequestHandler<SendNotificationCommand, NotificationDto>
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
        log.LogInformation(
            "E-MAIL | Para: {Recipient} | {Subject} | {Message}",
            n.Recipient,
            n.Subject,
            n.Message
        );
        return Mapping.ToDto(n);
    }
}

public class GetNotificationHandler(INotificationRepository r)
    : IRequestHandler<GetNotificationQuery, NotificationDto?>
{
    public async Task<NotificationDto?> Handle(GetNotificationQuery q, CancellationToken ct)
    {
        var n = await r.GetAsync(q.Id, ct);
        return n is null ? null : Mapping.ToDto(n);
    }
}

public class ListNotificationsHandler(INotificationRepository r)
    : IRequestHandler<ListNotificationsQuery, IReadOnlyList<NotificationDto>>
{
    public async Task<IReadOnlyList<NotificationDto>> Handle(
        ListNotificationsQuery q,
        CancellationToken ct
    ) => (await r.ListAsync(ct)).Select(Mapping.ToDto).ToList();
}
