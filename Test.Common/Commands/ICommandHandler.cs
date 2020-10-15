using System.Threading.Tasks;

namespace Test.Common.Commands
{
    public interface ICommandHandler
    {
        Task ProcessCommand(string input);
    }
}