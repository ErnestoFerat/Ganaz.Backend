using Ganaz.Backend.Library.Interfaces;
using Ganaz.Backend.Library.Processors;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Ganaz.Backend.Library.Providers
{
	public class QueueProvider : IQueueProvider
	{
		private readonly ILogger<QueueProvider> _logger;
		private readonly QueueProcessorService _queueProcessorService;

		public QueueProvider(ILogger<QueueProvider> logger, IHostedServiceAccessor<QueueProcessorService> accessor)
		{
			_logger = logger;
			_queueProcessorService = accessor.Service;
		}

		///<inheritdoc/>
		public List<MessageDto> FetchMessagesInQueue() =>
			_queueProcessorService.FetchMessagesInQueue();

		///<inheritdoc/>
		public List<MessageDto> FetchProcessedMessages() =>
			_queueProcessorService.FetchProcessedMessages();


		///<inheritdoc/>
		public MessageDto Pop() => 
			_queueProcessorService.Pop();
	}
}
