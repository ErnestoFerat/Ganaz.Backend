using System.Collections.Generic;

namespace Ganaz.Backend.Library.Interfaces
{
	public interface IQueueProvider
	{
		/// <summary>
		/// Gets all the messages in the Queue.
		/// </summary>
		/// <returns></returns>
		public List<MessageDto> FetchMessagesInQueue();
		
		/// <summary>
		/// Removes the highest priority items from queue and returns it's data
		/// </summary>
		/// <returns></returns>
		public MessageDto Pop();
		
		/// <summary>
		/// Gets all processed messages.
		/// </summary>
		/// <returns></returns>
		public List<MessageDto> FetchProcessedMessages();
	}
}
