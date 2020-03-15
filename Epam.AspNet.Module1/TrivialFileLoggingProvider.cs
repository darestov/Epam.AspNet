using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace Epam.AspNet.Module1
{
    internal class TrivialFileLoggingProvider : ILoggerProvider
    {
        private string fileName;

        public TrivialFileLoggingProvider(string fileName)
        {
            this.fileName = fileName;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(categoryName, fileName);
        }

        public void Dispose()
        {
        }
    }

    internal class FileLogger : ILogger
    {
        private string categoryName;
        private string fileName;
        object lockObj = new object();

        public FileLogger(string categoryName, string fileName)
        {
            this.categoryName = categoryName;
            this.fileName = fileName;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return new EmptyDisposable();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            string msg = $"{logLevel} :: {categoryName} :: {formatter(state, exception)} :: username :: {DateTime.Now}";

            lock (lockObj)
            {
                using (var writer = File.AppendText(fileName))
                {
                    writer.WriteLine(msg);
                }
            }
        }

        private class EmptyDisposable : IDisposable
        {
            public void Dispose()
            {
                
            }
        }
    }
}