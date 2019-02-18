using System;
using System.Runtime.CompilerServices;
using DatingApp.API.Common;
using Microsoft.Extensions.Logging;

namespace DatingApp.API.Base
{
    public interface ILog
    {
        string TransactionId { get; }
        void Write(string message, LogLevel logLevel = LogLevel.Debug, [CallerMemberName] string memberName = "", [CallerFilePath] string fileName = "", [CallerLineNumber] int lineNumber = 0);
        void Write(Exception ex, LogLevel logLevel = LogLevel.Error, [CallerMemberName] string memberName = "", [CallerFilePath] string fileName = "", [CallerLineNumber] int lineNumber = 0);
        IDisposable BeginScope(LogLevel logLevel = LogLevel.Information, [CallerMemberName] string memberName = "", [CallerFilePath] string fileName = "", [CallerLineNumber] int lineNumber = 0);
    }

    public class Log : BaseLog, ILog
    {
        private static readonly string _componentVersion;
        public Log(ILoggerFactory loggerFactory) : base(loggerFactory)
        {
           
        }

        static Log()
        {
             _componentVersion = typeof(Log).Assembly.GetName().Version.ToString();
        }

        public string TransactionId => throw new NotImplementedException();

        public override string ModuleName => "DatingApp.Common";

        public override string ComponentVersion => _componentVersion;

        public override string UserIdentity => throw new NotImplementedException();
    }
}