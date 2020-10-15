using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Common
{
    public interface ISettings
    {
        string TargetLanguage { get; }
        string OutputTarget { get; }
        string FileName { get; set; }
    }
    public sealed class Settings : ISettings
    {
        public string TargetLanguage { get; set; }
        public string OutputTarget { get; set; }
        public string FileName { get; set; }
    }
}
