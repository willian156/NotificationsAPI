using MediatR;
using Microsoft.AspNetCore.Mvc;
using NotificationsAPI.Application.Notifications;

namespace NotificationsAPI.Controllers;

[ApiController]
[Route("notifications")]
public class NotificationsController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List(CancellationToken ct) =>
        Ok(await sender.Send(new ListNotificationsQuery(), ct));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken ct)
    {
        var notification = await sender.Send(new GetNotificationQuery { Id = id }, ct);
        return notification is null ? NotFound() : Ok(notification);
    }
}
