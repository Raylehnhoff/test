using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using NSubstitute;
using Test.Common.Commands;
using Test.Common.Messaging;
using Xunit;

namespace Test.Common.Tests.Commands
{
    public class FetchCommandHandlerTests
    {
        private readonly Fixture _fixture = new Fixture();
        [Fact]
        public async Task FetchCommandHandler_ShouldCallService()
        {
            var msgService = Substitute.For<IMessageService>();
            msgService.GetUserResponseAsync(Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(_fixture.Create<MessageResponse>()));
            var fetchHandler = new FetchCommandHandler(msgService);
            await fetchHandler.ProcessCommand("hello world");
            await msgService
                .Received(1)
                .GetUserResponseAsync(Arg.Any<CancellationToken>());

        }
    }
}
