using System.Threading;
using System.Threading.Tasks;
using Test.Common.Messaging;

namespace Test.Common.Commands
{
    public class SendCommandHandler : ICommandHandler
    {
        private readonly IMessageService _messageService;

        public SendCommandHandler(IMessageService messageService)
        {
            _messageService = messageService;
        }
        public async Task ProcessCommand(string input)
        {
            await _messageService.WriteReplyAsync(new MessageResponse()
            {
                Message = input
            }, CancellationToken.None);
        }
    }
}
