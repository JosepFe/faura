using System;

namespace Faura.Infrastructure.Messaging.Options;

public class PulsarOptions
{
    public const string SectionName = "Pulsar";

    public string? ServiceUrl { get; set; }
    public string? Topic { get; set; }
    public string? SubscriptionName { get; set; }
    public int ProducerQueueSize { get; set; }
    public string? ProducerName { get; set; }
    public int ConsumerQueueSize { get; set; }
    public int RetryCount { get; set; }
    public TimeSpan RetryInterval { get; set; }
    public bool EnableSchemaValidation { get; set; }
    public string? SchemaType { get; set; }
    public bool EnableMetrics { get; set; }
} 