using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace MyGamingList.Api.HealthChecks;

public sealed class HealthyHeathCheck : IHealthCheck {
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new()) {
        return Task.FromResult(HealthCheckResult.Healthy("This healthy check is healthy"));
    }
}
