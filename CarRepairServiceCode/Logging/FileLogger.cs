﻿using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace CarRepairServiceCode.Logging
{
    public class FileLogger : ILogger
    {
        private string filePath;
        private static object _lock = new object();

        public FileLogger(string path)
        {
            filePath = path;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (formatter != null)
            {
                var path = Path.Combine(Environment.CurrentDirectory, filePath);

                lock (_lock)
                {
                    File.AppendAllText(path, formatter(state, exception) + Environment.NewLine);
                }
            }
        }
    }
}
