using System;
using System.Threading;
using System.Threading.Tasks;
using Test.Common.Messaging;

namespace Test.Common.Commands
{
    public class FetchCommandHandler : ICommandHandler
    {
        private readonly IMessageService _messageService;

        public FetchCommandHandler(IMessageService messageService)
        {
            _messageService = messageService;
        }
        public async Task ProcessCommand(string input)
        {
            var res = await _messageService.GetUserResponseAsync(CancellationToken.None);
            
            Console.WriteLine($"Fetch says: {res?.Message ?? "nothing"}");
        }
    }
}
