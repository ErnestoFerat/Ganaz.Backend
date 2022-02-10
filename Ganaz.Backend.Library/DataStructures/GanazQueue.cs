using System.Collections.Generic;

namespace Ganaz.Backend.Library.DataStructures
{
	/// <summary>
	/// Queue to handle Ganaz websocket messages.
	/// </summary>
	public class GanazQueue
	{
		private List<MessageDto> _queue = new List<MessageDto>();
		private List<MessageDto> _processedMessages = new List<MessageDto>();
		private int _highestPriority = 0;
		private int _lowerPriority = 0;

		public int Count => _queue.Count;

		/// <summary>
		/// Inserts element into the list based on its priority.
		/// </summary>
		/// <param name="item"></param>
		public void Add(MessageDto item)
		{
			if (item is not null && item.Priority > 0)
			{
				if (item.Priority > _highestPriority)
				{
					_queue.Insert(Count, item);
					_highestPriority = item.Priority;
					_lowerPriority = _queue[0].Priority;
				}
				else
				{
					if (item.Priority <= _lowerPriority)
					{
						_queue.Insert(0, item);
						_lowerPriority = item.Priority;
					}
					else
					{
						int insertAt = 0;
						for (int i = 0; i <= _queue.Count; i++)
						{
							if (item.Priority < _queue[i].Priority)
							{
								insertAt = i;
								break;
							}
						}
						_queue.Insert(insertAt, item);
					}
				}
			}
		}

		/// <summary>
		/// Removes item from the queue with the highest priority
		/// </summary>
		public MessageDto Remove()
		{
			if (_queue.Count > 0)
			{
				var item = _queue[Count - 1];
				_processedMessages.Add(item);

				_queue.Remove(item);

				return item;
			}
			return null;
		}

		/// <summary>
		/// Gets all removed messages from queue.
		/// </summary>
		/// <returns></returns>
		public List<MessageDto> GetProcessedMessages() => _processedMessages;

		/// <summary>
		/// Gets all messages in the queue.
		/// </summary>
		/// <returns></returns>
		public List<MessageDto> GetQueueMessages() => _queue;

		public void Clear() => _queue.Clear();

		public bool Contains(MessageDto item) => _queue.Contains(item);

		
		public int IndexOf(MessageDto item) => _queue.IndexOf(item);
	}
}
