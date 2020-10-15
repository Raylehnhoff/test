using System.Threading;
using System.Threading.Tasks;

namespace Test.Common.Messaging
{
    /// <summary>
    /// Fetches the message from the backing data source
    /// </summary>
    public interface IMessageAdapter
    {
        /// <summary>
        /// Fetches a message from whatever data source we have hiding behind the adapter; cancelable
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<MessageResponse> GetMessageAsync(CancellationToken token);

        Task WriteReplyAsync(MessageResponse response, CancellationToken token);
        Task<MessageResponse> GetUserResponseAsync(CancellationToken token);
    }
}