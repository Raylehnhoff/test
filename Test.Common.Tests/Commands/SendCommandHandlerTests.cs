using AutoFixture;
using NSubstitute;
using System.Threading;
using System.Threading.Tasks;
using Test.Common.Commands;
using Test.Common.Messaging;
using Xunit;

namespace Test.Common.Tests.Commands
{
    public class SendCommandHandlerTests
    {
        private readonly Fixture _fixture = new Fixture();

        [Fact]
        public async Task SendCommandHandler_ShouldCallService()
        {
            var cannedResponse = _fixture.Create<MessageResponse>();
            var msgService = Substitute.For<IMessageService>();
            msgService.WriteReplyAsync(Arg.Any<MessageResponse>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(cannedResponse));

            var fetchHandler = new FetchCommandHandler(msgService);
            await fetchHandler.ProcessCommand(string.Empty);
            await msgService
                .Received(1)
                .GetUserResponseAsync(Arg.Any<CancellationToken>());
        }
    }
}