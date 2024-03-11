using System.Collections.Concurrent;
using System.Text.Json;

namespace TestSse
{
    public record SseClient(HttpResponse Response, CancellationTokenSource Cancel);
    public class SseHolder : ISseHolder
    {
        private readonly ILogger<SseHolder> logger;
        private readonly ConcurrentDictionary<string, SseClient> clients = new();

        public SseHolder(ILogger<SseHolder> logger,
            IHostApplicationLifetime applicationLifetime)
        {
            this.logger = logger;
            applicationLifetime.ApplicationStopping.Register(OnShutdown);
        }
        public async Task AddAsync(HttpContext context)
        {
            var clientId = CreateId();
            var cancel = new CancellationTokenSource();
            var client = new SseClient(Response: context.Response, Cancel: cancel);
            if (clients.TryAdd(clientId, client))
            {
                EchoAsync(clientId, client);
                context.RequestAborted.WaitHandle.WaitOne();
                RemoveClient(clientId);
                await Task.FromResult(true);
            }
        }
        public async Task SendMessageAsync(SseMessage message)
        {
            foreach (var c in clients)
            {
                if (c.Key == message.Id)
                {
                    continue;
                }
                var messageJson = JsonSerializer.Serialize(message);
                await c.Value.Response.WriteAsync($"data: {messageJson}\r\r", c.Value.Cancel.Token);
                await c.Value.Response.Body.FlushAsync(c.Value.Cancel.Token);
            }
        }
        private async void EchoAsync(string clientId, SseClient client)
        {
            try
            {
                var clientIdJson = JsonSerializer.Serialize(new SseClientId { ClientId = clientId });
                client.Response.Headers.Add("Content-Type", "text/event-stream");
                client.Response.Headers.Add("Cache-Control", "no-cache");
                client.Response.Headers.Add("Connection", "keep-alive");
               client.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                // Send ID to client-side after connecting
                await client.Response.WriteAsync($"data: {clientIdJson}\r\r", client.Cancel.Token);
                await client.Response.Body.FlushAsync(client.Cancel.Token);
            }
            catch (OperationCanceledException ex)
            {
                logger.LogError($"Exception {ex.Message}");
            }

        }
        private void OnShutdown()
        {
            var tmpClients = new List<KeyValuePair<string, SseClient>>();
            foreach (var c in clients)
            {
                c.Value.Cancel.Cancel();
                tmpClients.Add(c);
            }
            foreach (var c in tmpClients)
            {
                clients.TryRemove(c);
            }
        }
        public void RemoveClient(string id)
        {
            var target = clients.FirstOrDefault(c => c.Key == id);
            if (string.IsNullOrEmpty(target.Key))
            {
                return;
            }
            target.Value.Cancel.Cancel();
            clients.TryRemove(target);
        }
        private string CreateId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
