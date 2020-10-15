using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Test.Common;
using Test.Common.Messaging;
using Test.Data.Persistence;

namespace Test.Data
{
    /// <summary>
    /// Fetches the message from the backing data source
    /// </summary>
    internal sealed class MessageAdapter : IMessageAdapter
    {
        private static readonly ConcurrentBag<string> Responses = new ConcurrentBag<string>();
        private readonly ISettings _settings;
        private readonly PersistenceCommandFactory _persistenceCommandFactory;

        public MessageAdapter(ISettings settings, PersistenceCommandFactory persistenceCommandFactory)
        {
            _settings = settings;
            _persistenceCommandFactory = persistenceCommandFactory;
        }

        /// <summary>
        /// Fetches a message from whatever data source we have hiding behind the adapter; cancelable
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<MessageResponse> GetMessageAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var message = _settings.TargetLanguage switch
            {
                "DE" => "Hallo Welt",
                "EN" => "Hello World",
                _ => "Hello World"
            };
            return Task.FromResult(new MessageResponse { Message = message });
        }

        public Task<MessageResponse> GetUserResponseAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            if (Responses.TryTake(out var response))
            {
                return Task.FromResult(new MessageResponse()
                {
                    Message = response
                });
            }

            return Task.FromResult(new MessageResponse()
            {
                Message = null
            });
        }

        public async Task WriteReplyAsync(MessageResponse response, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            Responses.Add(response.Message);

            if (_settings.OutputTarget.Equals("file", StringComparison.CurrentCultureIgnoreCase))
            {
                await _persistenceCommandFactory.GetCommandHandler("file").ProcessCommand(response.Message);
            }
        }
    }
}