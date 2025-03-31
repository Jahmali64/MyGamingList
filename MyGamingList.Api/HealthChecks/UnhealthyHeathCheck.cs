using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace MyGamingList.Api.HealthChecks;

public sealed class UnhealthyHeathCheck : IHealthCheck {
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new()) {
        return Task.FromResult(HealthCheckResult.Unhealthy("This healthy check is unhealthy"));
    }
}
