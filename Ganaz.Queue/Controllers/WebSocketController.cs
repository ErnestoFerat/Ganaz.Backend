using Ganaz.Backend.Library.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Ganaz.Queue.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
	[Produces("application/json")]
	public class WebSocketController : ControllerBase
	{
        private readonly ILogger<WebSocketController> _logger;
        private readonly IQueueProvider _queueProvider; 

        public WebSocketController(ILogger<WebSocketController> logger, IQueueProvider queueProvider)
        {
            _logger = logger;
            _queueProvider = queueProvider;
        }

        /// <summary>
        /// Removes the highest priorirty Item in the Queue.
        /// </summary>
        /// <returns>The highest priority Item Removed from Queue.</returns>
        [HttpGet("/Pop")]
        public async Task<IActionResult> Pop()
        {

            var item = _queueProvider.Pop();
            return Ok(item);           
        }


        /// <summary>
        /// Gets all the items in the Queue, ordered by priority.
        /// </summary>
        /// <returns></returns>
        [HttpGet("/GetMessagesInQueue")]
        public async Task<IActionResult> GetMessagesInQueue()
        {
            var items = _queueProvider.FetchMessagesInQueue();
            return Ok(items.OrderByDescending(i => i.Priority));
        }


        /// <summary>
        /// Gets all removed items from the queue.
        /// </summary>
        /// <returns></returns>
        [HttpGet("/GetProcessedMessages")]
        public async Task<IActionResult> GetProcessedMessages()
        {

            var items = _queueProvider.FetchProcessedMessages();
            return Ok(items);
        }
    }
}

