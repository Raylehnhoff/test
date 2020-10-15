using System;
using System.Threading.Tasks;

namespace Test.Common.Commands
{
    public class ExitCommandHandler : ICommandHandler
    {
        public Task ProcessCommand(string input)
        {
            Environment.Exit(0);
            return Task.CompletedTask;
        }
    }
}