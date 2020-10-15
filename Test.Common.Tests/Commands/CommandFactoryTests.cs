using System;
using System.Collections.Generic;
using System.Text;
using NSubstitute;
using Test.Common.Commands;
using Xunit;

namespace Test.Common.Tests.Commands
{
    public class CommandFactoryTests
    {
        [Fact]
        public void CommandFactory_WhenHandlerIsRegistered_ShouldReturnHandler()
        {
            var mockHandler = Substitute.For<ICommandHandler>();
            var factory = new CommandFactory();
            factory.RegisterCommand("myCommand", () => mockHandler);
            var handler = factory.GetCommandHandler("MyCoMmand");
            Assert.NotNull(handler);
        }

        [Fact]
        public void CommandFactory_WhenHandlerAlreadyRegistered_ShouldThrow()
        {
            var mockHandler = Substitute.For<ICommandHandler>();
            var factory = new CommandFactory();
            factory.RegisterCommand("myCommand", () => mockHandler);
            Assert.Throws<InvalidOperationException>(() => factory.RegisterCommand("myCommand", () => mockHandler));
        }

        [Fact]
        public void CommandFactory_WhenHandlerIsNotRegistered_Throw()
        {
            var factory = new CommandFactory();
            Assert.Throws<InvalidOperationException>(() => factory.GetCommandHandler("MyCoMmand"));
        }
    }
}
