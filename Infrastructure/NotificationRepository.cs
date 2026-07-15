using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NotificationsAPI.Application.Notifications;
using NotificationsAPI.Contracts;
using NotificationsAPI.Domain.Notifications;

namespace NotificationsAPI.Infrastructure;

public class NotificationRepository(NotificationsDbContext db) : INotificationRepository
{
    public Task<bool> ExistsAsync(Guid c, string s, CancellationToken ct) => db.Notifications.AnyAsync(x => x.CorrelationId == c && x.Subject == s, ct);
    public Task<Notification?> GetAsync(Guid id, CancellationToken ct) => db.Notifications.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);
    public async Task<IReadOnlyList<Notification>> ListAsync(CancellationToken ct) => await db.Notifications.AsNoTracking().OrderByDescending(x => x.SentAt).ToListAsync(ct);
    public void Add(Notification n) => db.Notifications.Add(n);
    public async Task SaveAsync(CancellationToken ct) => await db.SaveChangesAsync(ct);
}
