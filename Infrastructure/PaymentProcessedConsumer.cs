using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NotificationsAPI.Application.Notifications;
using NotificationsAPI.Contracts;
using NotificationsAPI.Domain.Notifications;

namespace NotificationsAPI.Infrastructure;

public class PaymentProcessedConsumer(ISender sender) : IConsumer<PaymentProcessedEvent>
{
    public async Task Consume(ConsumeContext<PaymentProcessedEvent> c)
    {
        if (c.Message.Status == PaymentStatus.Approved)
            await sender.Send(new SendNotificationCommand { CorrelationId = c.Message.OrderId, Recipient = c.Message.UserId.ToString(), Subject = "Compra confirmada", Message = $"Jogo {c.Message.GameId} adquirido por {c.Message.Price:C}.", }, c.CancellationToken);
    }
}
