using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Diagnostics.CodeAnalysis;
using System.Net.Mime;
using System.Text.Json;

namespace fastfood_payment.API.HealthCheck;

[ExcludeFromCodeCoverage]
public static class HealthCheckResponseWriter
{
    public static async Task Write(HttpContext context, HealthReport report)
    {
        string result = JsonSerializer.Serialize(
            new
            {
                statusApplication = report.Status.ToString(),
                healthChecks = report.Entries.Select(e => new
                {
                    check = e.Key,
                    ErrorMessage = e.Value.Exception?.Message,
                    status = Enum.GetName(typeof(HealthStatus), e.Value.Status)
                })
            });

        context.Response.ContentType = MediaTypeNames.Application.Json;
        await context.Response.WriteAsync(result);
    }
}
