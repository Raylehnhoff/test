using Microsoft.Extensions.DependencyInjection;
using Test.Common.Messaging;

namespace Test.Domain
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDomainLayer(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IMessageService, MessageService>();
        }
    }
}