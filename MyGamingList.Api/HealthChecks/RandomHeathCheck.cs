using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace MyGamingList.Api.HealthChecks;

public sealed class RandomHeathCheck : IHealthCheck {
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new()) {
        int randomResult = Random.Shared.Next(1, 4);

        return randomResult switch {
            1 => Task.FromResult(HealthCheckResult.Healthy("This health check is random")),
            2 => Task.FromResult(HealthCheckResult.Degraded("This health check is random")),
            3 => Task.FromResult(HealthCheckResult.Unhealthy("This health check is random")),
            _ => Task.FromResult(HealthCheckResult.Healthy("This health check is random")),
        };
    }
}
