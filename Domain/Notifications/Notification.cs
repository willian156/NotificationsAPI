namespace NotificationsAPI.Domain.Notifications;

public class Notification
{
    private Notification() { }

    public Notification(Guid correlationId, string recipient, string subject, string message)
    {
        Id = Guid.NewGuid();
        CorrelationId = correlationId;
        Recipient = recipient;
        Subject = subject;
        Message = message;
        SentAt = DateTimeOffset.UtcNow;
    }

    public Guid Id { get; private set; }
    public Guid CorrelationId { get; private set; }
    public string Recipient { get; private set; } = string.Empty;
    public string Subject { get; private set; } = string.Empty;
    public string Message { get; private set; } = string.Empty;
    public DateTimeOffset SentAt { get; private set; }
}

public interface INotificationRepository
{
    Task<bool> ExistsAsync(Guid correlationId, string subject, CancellationToken ct);
    Task<Notification?> GetAsync(Guid id, CancellationToken ct);
    Task<IReadOnlyList<Notification>> ListAsync(CancellationToken ct);
    void Add(Notification n);
    Task SaveAsync(CancellationToken ct);
}
