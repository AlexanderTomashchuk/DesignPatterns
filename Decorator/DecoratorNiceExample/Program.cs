using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using static System.Console;

namespace DecoratorNiceExample
{
    enum Severity
    {
        Info = 0,
        Warning = 1,
        Error = 2,
    }

    record LogEntry(DateTime DateTime, Severity Severity, string Message);

    interface ILogSaver
    {
        Task SaveLogEntry(string applicationId, LogEntry logEntry);
    }

    sealed class ElasticsearchLogSaver : ILogSaver
    {
        public Task SaveLogEntry(string applicationId, LogEntry logEntry)
        {
            var jsonString = JsonConvert.SerializeObject(logEntry);
            WriteLine(jsonString);

            // Сохраняем переданную запись в Elasticsearch
            return Task.FromResult<object>(null);
        }
    }

    abstract class LogSaverDecorator : ILogSaver
    {
        protected ILogSaver _decoratee;

        protected LogSaverDecorator(ILogSaver decoratee)
        {
            _decoratee = decoratee;
        }

        public abstract Task SaveLogEntry(string applicationId, LogEntry logEntry);
    }

    class ThrottlingLogSaverDecorator : LogSaverDecorator
    {
        private int _quotaCount;

        public ThrottlingLogSaverDecorator(ILogSaver decoratee, int quotaCount) : base(decoratee)
        {
            _quotaCount = quotaCount;
        }

        private bool QuotaReached() => _quotaCount <= 0;

        private void IncrementUserQuota()
        {
            _quotaCount--;
        }

        public override Task SaveLogEntry(string applicationId, LogEntry logEntry)
        {
            if (QuotaReached())
            {
                throw new Exception("Quota is reached");
            }

            var result = _decoratee.SaveLogEntry(applicationId, logEntry);
            
            IncrementUserQuota();

            return result;
        }

        class Program
        {
            static async Task Main(string[] args)
            {
                var applicationId = "123";
                var logEntry = new LogEntry(DateTime.Now, Severity.Error, "Test error message");

                ILogSaver logSaver = new ElasticsearchLogSaver();
                await logSaver.SaveLogEntry(applicationId, logEntry);

                WriteLine("---");
                
                ILogSaver logSaver2 = new ThrottlingLogSaverDecorator(new ElasticsearchLogSaver(), 15);
                
                for (var i = 1; i <= 100; i++)
                {
                    WriteLine($"Iteration number: {i}");
                    await logSaver2.SaveLogEntry(applicationId, logEntry);
                }

                ReadKey();
            }
        }
    }
}