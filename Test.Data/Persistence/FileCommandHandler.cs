using System;
using System.IO;
using System.Threading.Tasks;
using Test.Common;
using Test.Common.Commands;

namespace Test.Data.Persistence
{
    internal class FileCommandHandler : ICommandHandler
    {
        private readonly ISettings _settings;

        public FileCommandHandler(ISettings settings)
        {
            _settings = settings;
        }

        public async Task ProcessCommand(string input)
        {
            if (string.IsNullOrWhiteSpace(_settings.FileName))
            {
                throw new ArgumentNullException(nameof(_settings.FileName));
            }
            var docPath =
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            using var outputFile = new StreamWriter(Path.Combine(docPath, _settings.FileName), true);
            await outputFile.WriteLineAsync(input);
        }
    }
}