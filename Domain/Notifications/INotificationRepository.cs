namespace NotificationsAPI.Domain.Notifications;

public interface INotificationRepository
{
    Task<bool> ExistsAsync(Guid correlationId, string subject, CancellationToken ct);
    Task<Notification?> GetAsync(Guid id, CancellationToken ct);
    Task<IReadOnlyList<Notification>> ListAsync(CancellationToken ct);
    void Add(Notification n);
    Task SaveAsync(CancellationToken ct);
}
