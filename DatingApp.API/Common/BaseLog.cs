using System;
using System.IO;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;

namespace DatingApp.API.Common
{
    public abstract class BaseLog : IDisposable
    {
        
        #region Abstract Properties
        abstract public string ModuleName { get; }
        abstract public string ComponentVersion { get; }
        abstract public string UserIdentity { get; }
        #endregion

        #region Private Properties
        private LogLevel _loglevel;
        private string _memberName;
        private string _fileName;
        private int _linenumber;
        #endregion
        
        private readonly ILogger _log;
        public BaseLog(ILoggerFactory loggerFactory)
        {
            _log = loggerFactory.CreateLogger("Values");
           // _log = loggerFactory.AddFil; //.CreateLogger(this.ModuleName);
        }

        #region BeginScope
        public IDisposable BeginScope(LogLevel logLevel = LogLevel.Information, [CallerMemberName] string memberName = "", [CallerFilePath] string fileName = "", [CallerLineNumber] int lineNumber = 0)
        {
            this._loglevel = logLevel;
            this._memberName = memberName;
            this._fileName = fileName;
            this._linenumber = lineNumber;

            Enter(this._loglevel, this._memberName, this._fileName, this._linenumber);

            return null;
        }
        #endregion

        #region Write
        public void Write(string message, LogLevel logLevel = LogLevel.Information, [CallerMemberName] string memberName = "", [CallerFilePath] string fileName = "", [CallerLineNumber] int lineNumber = 0)
        {
            string entry;
            if (string.IsNullOrEmpty(memberName) || string.IsNullOrEmpty(fileName))
                entry = $"{message}";
            else
                entry = $"{Path.GetFileNameWithoutExtension(fileName)}.{memberName} - {message}";
            _log.LogInformation(10000, entry, null, null);
        }
        public void Write(Exception ex, LogLevel logLevel = LogLevel.Error, [CallerMemberName] string memberName = "", [CallerFilePath] string fileName = "", [CallerLineNumber] int lineNumber = 0)
        {
            string entry;
            if (string.IsNullOrEmpty(memberName) || string.IsNullOrEmpty(fileName))
                entry = $"EXCEPTION - {ex.Message}";
            else
                entry = $"EXCEPTION - {Path.GetFileNameWithoutExtension(fileName)}.{memberName} - {ex.Message}";
            _log.LogError(10000, entry, ex, null);
        }

        private void Enter(LogLevel logLevel = LogLevel.Information, [CallerMemberName] string memberName = "", [CallerFilePath] string fileName = "", [CallerLineNumber] int lineNumber = 0)
        {
            string entry = $"Enter - {logLevel} Format =  {Path.GetFileNameWithoutExtension(fileName)}, {memberName}, {lineNumber.ToString()}";
            _log.LogInformation(1000, entry, null ,null);
        }

        private void Exit(LogLevel logLevel = LogLevel.Information, [CallerMemberName] string memberName = "", [CallerFilePath] string fileName = "", [CallerLineNumber] int lineNumber = 0)
        {
            string entry = $"End - {logLevel} Format =  {Path.GetFileNameWithoutExtension(fileName)}, {memberName}, {lineNumber.ToString()}";
            _log.LogInformation(1000, entry, null ,null);
        }

        public void Dispose()
        {
            Exit(this._loglevel, this._memberName, this._fileName, this._linenumber);
        }
        #endregion
    }
}