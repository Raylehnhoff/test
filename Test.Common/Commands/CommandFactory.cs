using System;
using System.Collections.Concurrent;

namespace Test.Common.Commands
{
    public class CommandFactory
    {
        private readonly ConcurrentDictionary<string, Func<ICommandHandler>> commandDictionary = new ConcurrentDictionary<string, Func<ICommandHandler>>();

        public ICommandHandler GetCommandHandler(string command)
        {
            if (commandDictionary.TryGetValue(command.ToUpper(), out var commandFunc))
            {
                return commandFunc();
            }
            throw new InvalidOperationException($"{command} is not registered");
        }

        public void RegisterCommand(string command, Func<ICommandHandler> handlerFunc)
        {
            if (!commandDictionary.TryAdd(command.ToUpper(), handlerFunc))
            {
                throw new InvalidOperationException($"{command} is already registered");
            }
        }
    }
}