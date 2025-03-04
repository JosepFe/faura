namespace Faura.Infrastructure.Logger.Enrichers;

using Microsoft.AspNetCore.HeaderPropagation;
using Serilog.Core;
using Serilog.Events;

public class CorrelationIdEnricher : ILogEventEnricher
{
    private const string PropertyNameCorrelationId = "CorrelationId";
    private const string HeaderKey = "x-correlation-id";
    private readonly HeaderPropagationValues _headerPropagationValues;

    public CorrelationIdEnricher() : this(new HeaderPropagationValues()) { }

    public CorrelationIdEnricher(HeaderPropagationValues headerPropagationValues) => _headerPropagationValues = headerPropagationValues;

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        if (logEvent == null || propertyFactory == null) return;

        string correlationId = GetCorrelationId();
        var correlationProperty = propertyFactory.CreateProperty(PropertyNameCorrelationId, correlationId);
        logEvent.AddOrUpdateProperty(correlationProperty);
    }

    private string GetCorrelationId()
    {
        if (_headerPropagationValues.Headers != null &&
            _headerPropagationValues.Headers.TryGetValue(HeaderKey, out var correlationFromHeaders))
        {
            return correlationFromHeaders.ToString();
        }

        return Guid.NewGuid().ToString();
    }
}
