# NotificationsAPI

Microsserviço responsável por consumir `UserCreatedEvent` e registrar no console um e-mail de boas-vindas. Também consome `PaymentProcessedEvent` e registra a confirmação da compra quando o pagamento é aprovado.

## Executar

```bash
dotnet run --project NotificationsAPI.sln
```

## Variáveis de ambiente

- `ASPNETCORE_URLS`
- `RabbitMq__Host`
- `RabbitMq__Username`
- `RabbitMq__Password`
- `RabbitMq__UserQueue`
- `RabbitMq__PaymentQueue`

## Docker

```bash
docker build -t fcg/notifications-api:latest .
```

## Kubernetes

```bash
kubectl apply -f k8s/
```
