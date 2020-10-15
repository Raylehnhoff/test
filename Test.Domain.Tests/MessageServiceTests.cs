using AutoFixture;
using NSubstitute;
using System.Threading;
using System.Threading.Tasks;
using Test.Common.Messaging;
using Xunit;

namespace Test.Domain.Tests
{
    public class MessageServiceTests
    {
        private readonly Fixture _fixture;

        public MessageServiceTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public async Task MessageService_ShouldCallAdapter()
        {
            var adapter = Substitute.For<IMessageAdapter>();
            var returnString = _fixture.Create<string>();
            adapter.GetMessageAsync(Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(new MessageResponse()
                {
                    Message = returnString
                }));

            var service = new MessageService(adapter);
            var res = await service.GetMessageAsync(CancellationToken.None);
            Assert.Equal(returnString, res.Message);
        }
    }
}