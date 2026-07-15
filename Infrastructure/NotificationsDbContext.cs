using MassTransit;
using MediatR;
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
