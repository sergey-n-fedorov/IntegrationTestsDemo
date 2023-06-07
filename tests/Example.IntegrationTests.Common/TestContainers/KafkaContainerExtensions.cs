using Confluent.Kafka;
using Newtonsoft.Json;
using Testcontainers.Kafka;
using Xunit.Abstractions;

namespace Example.IntegrationTests.TestContainers;

public static class KafkaContainerExtensions
{
    private static readonly TimeSpan DefaultReadTimeout = TimeSpan.FromSeconds(3);
    private static readonly TimeSpan SingleMessageReadTimeout = TimeSpan.FromSeconds(1);
    

    public static List<TMessage> ReadAllMessages<TMessage>(
        this KafkaContainer container,
        string topic,
        string? messageKey = null,
        string? consumerGroup = null,
        TimeSpan? timeout = null,
        ITestOutputHelper? testOutputHelper = null)
    {
        timeout ??= DefaultReadTimeout;
        consumerGroup ??= Guid.NewGuid().ToString();
        
        List<TMessage> result = new List<TMessage>();
        
        var config = new ConsumerConfig
        {
            BootstrapServers = container.GetBootstrapAddress(),
            GroupId = consumerGroup,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
        
        using(var cancellationTokenSource = new CancellationTokenSource())
        using (var consumer = new ConsumerBuilder<string, string>(config).Build())
        {
            var cancellationToken = cancellationTokenSource.Token;
            cancellationTokenSource.CancelAfter(timeout.Value);
            
            consumer.Subscribe(topic);

            while (!cancellationToken.IsCancellationRequested)
            {
                var consumeResult = consumer.Consume(SingleMessageReadTimeout);

                if (consumeResult?.Message != null && (messageKey == null || consumeResult.Message.Key == messageKey))
                {
                    try {
                        var message = JsonConvert.DeserializeObject<TMessage>(consumeResult.Message.Value)!;
                        result.Add(message);
                    }
                    catch (Exception e) {
                        testOutputHelper?.WriteLine($"Failed to deserialize message: {consumeResult.Message.Value}, error: {e.Message}");
                    }
                }
            }
        }

        return result;
    }
    
    public static void DeleteAllTopics(this KafkaContainer container, ITestOutputHelper? testOutputHelper = null)
    {
        var adminConfig = new AdminClientConfig {
            BootstrapServers = container.GetBootstrapAddress()
        };

        using (IAdminClient adminClient = new AdminClientBuilder(adminConfig).Build())
        {
            var topics = adminClient.GetMetadata(DefaultReadTimeout).Topics;
            
            foreach (var topic in topics)
            {
                try
                {
                    adminClient.DeleteTopicsAsync(new[] { topic.Topic }).Wait();
                    testOutputHelper?.WriteLine($"Topic '{topic.Topic}' deleted successfully.");
                }
                catch (Exception ex)
                {
                    testOutputHelper?.WriteLine($"Failed to delete topic '{topic.Topic}': {ex.Message}");
                }
            }
        }
    }
}