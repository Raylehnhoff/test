using System.Threading;
using System.Threading.Tasks;

namespace Test.Common.Messaging
{
    /// <summary>
    /// Domain-layer system to fetch messages from the adapter, potentially modify them, and return to the caller
    /// </summary>
    public interface IMessageService
    {
        Task<MessageResponse> GetMessageAsync(CancellationToken token);
        Task WriteReplyAsync(MessageResponse response, CancellationToken token);
        Task<MessageResponse> GetUserResponseAsync(CancellationToken token);
    }
}