using MassTransit;
using Microsoft.EntityFrameworkCore;
using NotificationsAPI.Application.Notifications;
using NotificationsAPI.Contracts;
using NotificationsAPI.Domain.Notifications;

namespace NotificationsAPI.Infrastructure;

public class NotificationsDbContext(DbContextOptions<NotificationsDbContext> o) : DbContext(o)
{
    public DbSet<Notification> Notifications => Set<Notification>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Notification>(e =>
        {
            e.ToTable("notifications");
            e.HasKey(x => x.Id);
            e.HasIndex(x => new { x.CorrelationId, x.Subject }).IsUnique();
        });
    }
}

public class NotificationRepository(NotificationsDbContext db) : INotificationRepository
{
    public Task<bool> ExistsAsync(Guid c, string s, CancellationToken ct) =>
        db.Notifications.AnyAsync(x => x.CorrelationId == c && x.Subject == s, ct);

    public Task<Notification?> GetAsync(Guid id, CancellationToken ct) =>
        db.Notifications.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);

    public async Task<IReadOnlyList<Notification>> ListAsync(CancellationToken ct) =>
        await db.Notifications.AsNoTracking().OrderByDescending(x => x.SentAt).ToListAsync(ct);

    public void Add(Notification n) => db.Notifications.Add(n);

    public async Task SaveAsync(CancellationToken ct) => await db.SaveChangesAsync(ct);
}

public class UserCreatedConsumer(ISender sender)
    : IConsumer<UserCreatedEvent>
{
    public async Task Consume(ConsumeContext<UserCreatedEvent> c) =>
        await sender.Send(
            new SendNotificationCommand
            {
                CorrelationId = c.Message.UserId,
                Recipient = c.Message.Email,
                Subject = "Bem-vindo à FCG",
                Message = $"Olá, {c.Message.Name}! Bem-vindo à FCG.",
            },
            c.CancellationToken
        );
}

public class PaymentProcessedConsumer(ISender sender)
    : IConsumer<PaymentProcessedEvent>
{
    public async Task Consume(ConsumeContext<PaymentProcessedEvent> c)
    {
        if (c.Message.Status == PaymentStatus.Approved)
            await sender.Send(
                new SendNotificationCommand
                {
                    CorrelationId = c.Message.OrderId,
                    Recipient = c.Message.UserId.ToString(),
                    Subject = "Compra confirmada",
                    Message = $"Jogo {c.Message.GameId} adquirido por {c.Message.Price:C}.",
                },
                c.CancellationToken
            );
    }
}
