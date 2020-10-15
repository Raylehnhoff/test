using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Test.Common;
using Test.Common.Commands;
using Test.Common.Messaging;
using Test.Data;
using Test.Domain;

namespace Test.ConsoleApp
{
    class Program
    {
        private const string SendCommand = "send";
        private const string ExitCommand = "exit";
        private const string QuitCommand = "quit";
        private const string FetchCommand = "fetch";
        static async Task Main(string[] args)
        {
            var serviceProvider = AddDependencyInjection();
            var messageService = serviceProvider.GetService<IMessageService>();
            var message = await messageService.GetMessageAsync(CancellationToken.None);
            var cmdFactory = serviceProvider.GetService<CommandFactory>();

            Console.WriteLine(message.Message);
            while (true)
            {
                Console.WriteLine("Want to talk to yourself? Type something. 'QUIT' to exit, 'FETCH' to get the last message");
                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Nothing entered; exiting");
                    break;
                }
                else
                {
                    var command = input.Replace("'", string.Empty);
                    try
                    {
                        var handler = cmdFactory.GetCommandHandler(command);
                        await handler.ProcessCommand(input);
                    }
                    catch (InvalidOperationException) // tried a command we didn't recognize, so it'll be a send
                    {
                        var sendHandler = cmdFactory.GetCommandHandler(SendCommand);
                        await sendHandler.ProcessCommand(input);
                    }
                }
            }
        }



        static ServiceProvider AddDependencyInjection()
        {
            

            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddSingleton<IConfiguration>(sp => new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", true, true)
                    .Build());

            serviceProvider
                .AddDataLayer()
                .AddDomainLayer();

            serviceProvider.AddSingleton<ISettings>(sp =>
            {
                var cfg = sp.GetService<IConfiguration>();
                return new Settings()
                {
                    TargetLanguage = cfg.GetValue<string>("EnvironmentConfiguration:TargetLanguage") ?? "EN",
                    OutputTarget = cfg.GetValue<string>("EnvironmentConfiguration:OutputTarget") ?? "file",
                    FileName = cfg.GetValue<string>("EnvironmentConfiguration:FileName") ?? "file.txt"
                };
            });

            // registering these allows each handler to have its own dependencies injected
            serviceProvider
                .AddSingleton<ExitCommandHandler>()
                .AddSingleton<ExitCommandHandler>()
                .AddSingleton<FetchCommandHandler>()
                .AddSingleton<SendCommandHandler>()
                ;

            serviceProvider.AddSingleton<CommandFactory>(sp =>
            {
                var factory = new CommandFactory();
                factory.RegisterCommand(ExitCommand, sp.GetService<ExitCommandHandler>);
                factory.RegisterCommand(QuitCommand, sp.GetService<ExitCommandHandler>);
                factory.RegisterCommand(FetchCommand, sp.GetService<FetchCommandHandler>);
                factory.RegisterCommand(SendCommand, sp.GetService<SendCommandHandler>);
                return factory;
            });
            return serviceProvider.BuildServiceProvider();
        }
    }
}
