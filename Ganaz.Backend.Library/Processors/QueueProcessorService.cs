using Ganaz.Backend.Library.DataStructures;
using Ganaz.Backend.Library.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ganaz.Backend.Library.Processors
{
	#region Helpers Class to give us access to the right background service
	public interface IHostedServiceAccessor<T> where T : IHostedService
    {
       public T Service { get; }
    }

    public class HostedServiceAccessor<T> : IHostedServiceAccessor<T> where T : IHostedService 
    {
        public HostedServiceAccessor(IEnumerable<IHostedService> hostedServices) { Service = hostedServices.OfType<T>().FirstOrDefault(); }		

        public T Service { get; }
    }
	#endregion

    /// <summary>
    /// This background service is in charge to read from Ganaz web socket app.
    /// </summary>
	public class QueueProcessorService : BackgroundService, IQueueProvider
    {
        private readonly ILogger<QueueProcessorService> _logger;        
        private static GanazQueue _queue = new();

        public QueueProcessorService(ILogger<QueueProcessorService> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await ReadGanazWebSocket(stoppingToken);
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            await base.StopAsync(stoppingToken);
        }

        ///<inheritdoc/>
        public List<MessageDto> FetchMessagesInQueue() => _queue.GetQueueMessages();

        ///<inheritdoc/>
        public MessageDto Pop() => _queue.Remove();

        ///<inheritdoc/>
        public List<MessageDto> FetchProcessedMessages() => _queue.GetProcessedMessages();


        private async Task ReadGanazWebSocket(CancellationToken stoppingToken)
		{
            using (var webSocket = new ClientWebSocket())
            {
                await webSocket.ConnectAsync(new Uri("ws://localhost:7777"), CancellationToken.None);
                _logger.Log(LogLevel.Information, "WebSocket Connection Established");

                var buffer = new byte[1024 * 4];

                while (webSocket.State == WebSocketState.Open)
                {
                    try
                    {
                        var result = await webSocket.ReceiveAsync(buffer, CancellationToken.None);

                        if (result.MessageType == WebSocketMessageType.Close)
                        {
                            await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None);
                        }
                        else
                        {
                            string rawMessage = Encoding.ASCII.GetString(buffer);
                            try
                            {
                                if (!string.IsNullOrWhiteSpace(rawMessage))
                                {
                                    _logger.Log(LogLevel.Information, $"Processing message: {rawMessage}");

                                    var message = JsonConvert.DeserializeObject<MessageDto>(rawMessage);

                                    if (message is not null)
                                    {
                                        _queue.Add(message);
                                    }
                                }
                            }
                            catch
                            {
                                _logger.Log(LogLevel.Warning, $"Unexpected Message: {rawMessage}");
                            }

                            Array.Clear(buffer, 0, buffer.Length);

                            await Task.Delay(300, stoppingToken);
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        return;
                    }
                }
            }
        }
	}
}
