using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using static System.Console;

namespace Adapter
{
    enum Severity
    {
        Info = 0,
        Warning = 1,
        Error = 2,
    }
    
    //lets say developer 1 created the SqlServerLogSaver for saving logs in SQL DB
    class SqlServerLogSaver
    {
        public void Save(DateTime dateTime, Severity severity, string message)
        {
            WriteLine("Saving log in DB...");
            WriteLine($"Table row: {dateTime}|{severity}|{message}");
        }

        public void SaveException(DateTime dateTime, string message, Exception exception)
        {
            WriteLine("Saving log in DB (Error)...");
            WriteLine($"Table row(error): {dateTime}|{Severity.Error}|{message}|{exception.Message}");
        }
    }
    
    //then developer 2 created the ElasticsearchLogSaver for saving logs in Elasticsearch
    class ElasticsearchLogSaver
    {
        public void Save(SimpleLogEntry simpleLogEntry)
        {
            WriteLine("Saving log in Elasticsearch...");
            WriteLine(JsonConvert.SerializeObject(simpleLogEntry));
        }

        public void SaveException(ExceptionLogEntry exceptionLogEntry)
        {
            WriteLine("Saving log in Elasticsearch (Error)...");
            WriteLine(JsonConvert.SerializeObject(exceptionLogEntry));
        }
    }

    record LogEntry(DateTime DateTime, Severity Severity, string Message);

    record SimpleLogEntry(Severity Severity, string Message) : LogEntry(DateTime.Now, Severity, Message);

    record ExceptionLogEntry(DateTime DateTime, string Message, Exception Exception) : LogEntry(DateTime,
        Severity.Error, Message);
    
    //the interfaces of SqlServerLogSaver and ElasticsearchLogSaver are different
    //but this does not allow them to be used in a polymorphic way which is bad
    
    //so to fix this we have to create another abstract layer by highlighting the ILogSaver interface
    //and instead of changing existing classes (SqlServerLogSaver and ElasticsearchLogSaver) and implementing
    //a new interface in them, we can create an intermediate layer of adapters
    
    interface ILogSaver
    {
        void Save(LogEntry logEntry);
    }

    class SqlServerLogSaverAdapter : ILogSaver
    {
        private readonly SqlServerLogSaver _sqlServerLogSaver;

        public SqlServerLogSaverAdapter(SqlServerLogSaver sqlServerLogSaver)
        {
            _sqlServerLogSaver = sqlServerLogSaver;
        }

        public void Save(LogEntry logEntry)
        {
            switch (logEntry)
            {
                case SimpleLogEntry simpleLogEntry:
                    _sqlServerLogSaver.Save(simpleLogEntry.DateTime, simpleLogEntry.Severity, simpleLogEntry.Message);
                    return;
                case ExceptionLogEntry exceptionLogEntry:
                    _sqlServerLogSaver.SaveException(exceptionLogEntry.DateTime, exceptionLogEntry.Message, exceptionLogEntry.Exception);
                    return;
                default:
                    throw new KeyNotFoundException();
            }
        }
    }
    
    class ElasticsearchLogSaverAdapter : ILogSaver
    {
        private readonly ElasticsearchLogSaver _elasticsearchLogSaver;

        public ElasticsearchLogSaverAdapter(ElasticsearchLogSaver elasticsearchLogSaver)
        {
            _elasticsearchLogSaver = elasticsearchLogSaver;
        }

        public void Save(LogEntry logEntry)
        {
            switch (logEntry)
            {
                case SimpleLogEntry simpleLogEntry: _elasticsearchLogSaver.Save(simpleLogEntry);
                    return;
                case ExceptionLogEntry exceptionLogEntry: _elasticsearchLogSaver.SaveException(exceptionLogEntry);
                    return;
                default:
                    throw new KeyNotFoundException();
            }
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            var dateTimeNow = DateTime.Now;
            var infoMessage = "Info log message";
            var errorMessage = "ERROR log message";
            var exception = new Exception("EXCEPTION message");
            
            //old approach
            var sqlServerLogSaver = new SqlServerLogSaver();
            sqlServerLogSaver.Save(dateTimeNow, Severity.Info, infoMessage);
            sqlServerLogSaver.SaveException(dateTimeNow, errorMessage, exception);
            WriteLine("---");
            
            var elasticsearchLogSaver = new ElasticsearchLogSaver();
            elasticsearchLogSaver.Save(new SimpleLogEntry(Severity.Info, infoMessage));
            elasticsearchLogSaver.SaveException(new ExceptionLogEntry(dateTimeNow, errorMessage, exception));
            WriteLine("---");
            
            WriteLine("-----------");
            
            //new approach
            var simpleLogEntry = new SimpleLogEntry(Severity.Info, infoMessage);
            var exceptionLogEntry = new ExceptionLogEntry(dateTimeNow, errorMessage, exception);
            
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient<SqlServerLogSaver>();
            serviceCollection.AddTransient<ElasticsearchLogSaver>();
            serviceCollection.AddTransient<SqlServerLogSaverAdapter>();
            serviceCollection.AddTransient<ElasticsearchLogSaverAdapter>();
            
            var serviceProvider = serviceCollection.BuildServiceProvider();
            
            ILogSaver[] logSavers =
            {
                serviceProvider.GetRequiredService<SqlServerLogSaverAdapter>(),
                serviceProvider.GetRequiredService<ElasticsearchLogSaverAdapter>()
            };

            foreach (var logSaver in logSavers)
            {
                logSaver.Save(simpleLogEntry);
                logSaver.Save(exceptionLogEntry);
                
                WriteLine("---");
            }
            
            WriteLine("-----------");
            
            ReadKey();
        }
    }
}