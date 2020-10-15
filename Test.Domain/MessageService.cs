using System.Threading;
using System.Threading.Tasks;
using Test.Common.Messaging;

namespace Test.Domain
{
    /// <summary>
    /// Domain-layer system to fetch messages from the adapter, potentially modify them, and return to the caller
    /// </summary>
    internal sealed class MessageService : IMessageService
    {
        private readonly IMessageAdapter _adapter;

        public MessageService(IMessageAdapter adapter)
        {
            _adapter = adapter;
        }

        public Task<MessageResponse> GetMessageAsync(CancellationToken token) => _adapter.GetMessageAsync(token);

        public Task WriteReplyAsync(MessageResponse response, CancellationToken token) =>
            _adapter.WriteReplyAsync(response, token);

        public async Task<MessageResponse> GetUserResponseAsync(CancellationToken token)
        {
            var res = await _adapter.GetUserResponseAsync(token);
            if (res.Message == null)
            {
                return new MessageResponse()
                {
                    Message = "No messages waiting for you :("
                };
            }

            return res;
        }
    }
}