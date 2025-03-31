using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace MyGamingList.Api.HealthChecks;

public sealed class DegradedHeathCheck : IHealthCheck {
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new()) {
        return Task.FromResult(HealthCheckResult.Degraded("This healthy check is degraded"));
    }
}
