using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using NSubstitute;
using Test.Common;
using Test.Common.Commands;
using Test.Common.Messaging;
using Test.Data.Persistence;
using Xunit;

namespace Test.Data.Tests
{
    public class MessageAdapterTests
    {
        private readonly Fixture _fixture;
        public MessageAdapterTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public async Task MessageAdapter_WhenLanguageIsDE_ShouldReturnMessage()
        {
            var settingsMock = Substitute.For<ISettings>();
            var persistenceFactory = new PersistenceCommandFactory();
            settingsMock
                .TargetLanguage
                .Returns("DE");

            var adapter = new MessageAdapter(settingsMock, persistenceFactory);


            var message = await adapter.GetMessageAsync(CancellationToken.None);
            Assert.Equal("Hallo Welt", message.Message);
        }
        [Fact]
        public async Task MessageAdapter_WhenLanguageIsUnexpected_ShouldReturnEnglish()
        {
            var settingsMock = Substitute.For<ISettings>();
            var persistenceFactory = new PersistenceCommandFactory();
            settingsMock
                .TargetLanguage
                .Returns(_fixture.Create<string>());

            var adapter = new MessageAdapter(settingsMock, persistenceFactory);


            var message = await adapter.GetMessageAsync(CancellationToken.None);
            Assert.Equal("Hello World", message.Message);
        }

        [Fact]
        public async Task MessageAdapter_WhenCancellationRequested_ShouldThrow()
        {
            var settingsMock = Substitute.For<ISettings>();
            var persistenceFactory = new PersistenceCommandFactory();
            settingsMock
                .TargetLanguage
                .Returns("RU");

            var adapter = new MessageAdapter(settingsMock, persistenceFactory);

            var cancelledToken = new CancellationToken(true);
            await Assert.ThrowsAsync<OperationCanceledException>(async() => await adapter.GetMessageAsync(cancelledToken));
        }

        [Fact]
        public async Task MessageAdapter_WhenFileOutputTargetIsSpecified_ShouldCallFileCommandHandler()
        {
            var settingsMock = Substitute.For<ISettings>();
            var message = _fixture.Create<MessageResponse>();

            var persistenceFactory = new PersistenceCommandFactory();
            var fileHandlerMock = Substitute.For<ICommandHandler>();
            persistenceFactory.RegisterCommand("file", () => fileHandlerMock);

            settingsMock
                .OutputTarget
                .Returns("file");

            settingsMock
                .FileName
                .Returns("myfile.txt");

            var adapter = new MessageAdapter(settingsMock, persistenceFactory);

            await adapter.WriteReplyAsync(message, CancellationToken.None);
            await fileHandlerMock
                .Received(1)
                .ProcessCommand(Arg.Any<string>());
        }
    }
}
