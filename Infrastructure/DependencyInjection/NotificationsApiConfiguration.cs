using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NotificationsAPI.Application.Notifications;
using NotificationsAPI.Domain.Notifications;

namespace NotificationsAPI.Infrastructure.DependencyInjection;

public static class NotificationsApiConfiguration
{
    public static IServiceCollection AddNotificationsApi(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddNotificationsApplication();
        services.AddNotificationsPersistence(configuration);
        services.AddNotificationsMessaging(configuration);
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerWithBearerAuthentication();
        return services;
    }

    public static WebApplication UseNotificationsApi(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.MapControllers();
        return app;
    }

    public static async Task InitializeNotificationsDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        await scope
            .ServiceProvider.GetRequiredService<NotificationsDbContext>()
            .Database.EnsureCreatedAsync();
    }

    private static void AddNotificationsApplication(this IServiceCollection services)
    {
        services.AddMediatR(configuration =>
            configuration.RegisterServicesFromAssemblyContaining<SendNotificationCommand>()
        );
    }

    private static void AddNotificationsPersistence(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddDbContext<NotificationsDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Database"))
        );
        services.AddScoped<INotificationRepository, NotificationRepository>();
    }

    private static void AddNotificationsMessaging(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddMassTransit(configurator =>
        {
            configurator.AddConsumer<UserCreatedConsumer>();
            configurator.AddConsumer<PaymentProcessedConsumer>();
            configurator.UsingRabbitMq((context, rabbit) =>
            {
                rabbit.Host(configuration["RabbitMq:Host"] ?? "localhost", "/", host =>
                {
                    host.Username(configuration["RabbitMq:Username"] ?? "guest");
                    host.Password(configuration["RabbitMq:Password"] ?? "guest");
                });
                rabbit.ReceiveEndpoint(
                    configuration["RabbitMq:UserQueue"] ?? "notifications-users",
                    endpoint => endpoint.ConfigureConsumer<UserCreatedConsumer>(context)
                );
                rabbit.ReceiveEndpoint(
                    configuration["RabbitMq:PaymentQueue"] ?? "notifications-payments",
                    endpoint => endpoint.ConfigureConsumer<PaymentProcessedConsumer>(context)
                );
            });
        });
    }

    private static void AddSwaggerWithBearerAuthentication(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            var bearerScheme = new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "Informe somente o token JWT. O prefixo Bearer será adicionado automaticamente.",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer",
                },
            };

            options.AddSecurityDefinition("Bearer", bearerScheme);
            options.AddSecurityRequirement(
                new OpenApiSecurityRequirement { [bearerScheme] = Array.Empty<string>() }
            );
        });
    }
}
