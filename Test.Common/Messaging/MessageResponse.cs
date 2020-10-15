using Newtonsoft.Json;

namespace Test.Common.Messaging
{
    public interface IMessageResponse
    {
        string Message { get; set; }
    }
    public class MessageResponse
    {
        [JsonProperty("message")] public string Message { get; set; }
    }
}