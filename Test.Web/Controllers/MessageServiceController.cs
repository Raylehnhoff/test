using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Test.Common.Messaging;

namespace Test.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessageServiceController : ControllerBase
    {
        private readonly ILogger<MessageServiceController> _logger;
        private readonly IMessageService _messageService;

        public MessageServiceController(ILogger<MessageServiceController> logger, IMessageService messageService)
        {
            _logger = logger;
            _messageService = messageService;
        }

        [HttpGet]
        public async Task<ActionResult<MessageResponse>> Get(CancellationToken token)
        {
            //useful for gathering metrics on when a request came in -- not too useful in this case though
            _logger.LogInformation("{dt} | {controller}.{method}", DateTime.UtcNow, nameof(MessageServiceController), nameof(Get));
            var message = await _messageService.GetMessageAsync(token);

            return new OkObjectResult(message);
        }
    }
}