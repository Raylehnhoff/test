using Microsoft.Extensions.DependencyInjection;
using Test.Common.Messaging;
using Test.Data.Persistence;

namespace Test.Data
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataLayer(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IMessageAdapter, MessageAdapter>()
                .AddSingleton<FileCommandHandler>()
                .AddSingleton(sp =>
                {
                    var commandFactory = new PersistenceCommandFactory();
                    commandFactory.RegisterCommand("file", sp.GetService<FileCommandHandler>);
                    return commandFactory;
                });
        }
    }
}
