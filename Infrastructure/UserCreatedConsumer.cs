using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NotificationsAPI.Application.Notifications;
using NotificationsAPI.Contracts;
using NotificationsAPI.Domain.Notifications;

namespace NotificationsAPI.Infrastructure;

public class UserCreatedConsumer(ISender sender) : IConsumer<UserCreatedEvent>
{
    public async Task Consume(ConsumeContext<UserCreatedEvent> c) => await sender.Send(new SendNotificationCommand { CorrelationId = c.Message.UserId, Recipient = c.Message.Email, Subject = "Bem-vindo à FCG", Message = $"Olá, {c.Message.Name}! Bem-vindo à FCG.", }, c.CancellationToken);
}
