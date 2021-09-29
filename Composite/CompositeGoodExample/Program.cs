using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static System.Console;

namespace CompositeGoodExample
{
    enum Severity
    {
        Info = 0,
        Warning = 1,
        Error = 2,
    }

    record LogEntry(DateTime DateTime, Severity Severity, string Message);

    abstract class LogImportRule
    {
        public abstract bool ShouldImport(LogEntry logEntry);
    }

    class SingleLogImportRule : LogImportRule
    {
        private readonly Func<LogEntry, bool> _predicate;

        public SingleLogImportRule(Func<LogEntry, bool> predicate)
        {
            _predicate = predicate;
        }
        
        public override bool ShouldImport(LogEntry logEntry)
        {
            return _predicate.Invoke(logEntry);
        }
    }

    abstract class CompositeLogImportRule : LogImportRule
    {
        private readonly LogImportRule[] _logImportRules;
        protected LogImportRule[] LogImportRules => _logImportRules;

        protected CompositeLogImportRule(params LogImportRule[] logImportRules)
        {
            _logImportRules = logImportRules;
        }
    }

    class AndCompositeLogImportRule : CompositeLogImportRule
    {
        public AndCompositeLogImportRule(params LogImportRule[] logImportRules) : base(logImportRules)
        {
        }

        public override bool ShouldImport(LogEntry logEntry)
        {
            return LogImportRules.All(rule => rule.ShouldImport(logEntry));
        }
    }

    class OrCompositeLogImportRule : CompositeLogImportRule
    {
        public OrCompositeLogImportRule(params LogImportRule[] logImportRules) : base(logImportRules)
        {
        }

        public override bool ShouldImport(LogEntry logEntry)
        {
            return LogImportRules.Any(rule => rule.ShouldImport(logEntry));
        }
    }

    static class LogRuleFactory
    {
        public static LogImportRule Import(Func<LogEntry, bool> predicate)
        {
            return new SingleLogImportRule(predicate);
        }

        public static LogImportRule Or(this LogImportRule left, Func<LogEntry, bool> predicate)
        {
            return new OrCompositeLogImportRule(left, Import(predicate));
        }

        public static LogImportRule And(this LogImportRule left, Func<LogEntry, bool> predicate)
        {
            return new AndCompositeLogImportRule(left, Import(predicate));
        }
    }

    static class LogImportRuleExtensions
    {
        public static IEnumerable<LogEntry> Filter(this IEnumerable<LogEntry> logEntries, LogImportRule rule)
        {
            return logEntries.Where(rule.ShouldImport);
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            var logEntries = new List<LogEntry>
            {
                new LogEntry(DateTime.Now.AddDays(-1).Date, Severity.Info, "Test message 1"),
                new LogEntry(DateTime.Now.AddDays(-3).Date, Severity.Error, "Test message 2"),
                new LogEntry(DateTime.Now.AddDays(-10).Date, Severity.Warning, "Test message 3"),
                new LogEntry(DateTime.Now.AddDays(-30).Date, Severity.Info, "Test message 4"),
                new LogEntry(DateTime.Now.AddDays(-100).Date, Severity.Warning, "Test message 5"),
                new LogEntry(DateTime.Now.AddDays(-300).Date, Severity.Error, "Test message 6")
            };

            var rule1 = new SingleLogImportRule(log => log.Severity == Severity.Error);

            var filteredEntries = logEntries.Where(log => rule1.ShouldImport(log));
            filteredEntries.ToList().ForEach(WriteLine);
            WriteLine("---");

            var rule2 = new SingleLogImportRule(log =>
                log.DateTime > DateTime.Now.AddDays(-20) ||
                log.DateTime < DateTime.Now.AddDays(-100));
            var filteredEntries2 = logEntries.Where(log => rule2.ShouldImport(log));
            filteredEntries2.ToList().ForEach(WriteLine);
            WriteLine("---");

            var rule3 = new OrCompositeLogImportRule(
                new SingleLogImportRule(log => log.Severity == Severity.Info),
                new SingleLogImportRule(log => log.DateTime > DateTime.Now.AddDays(-5)));
            var filteredEntries3 = logEntries.Where(log => rule3.ShouldImport(log));
            filteredEntries3.ToList().ForEach(WriteLine);
            WriteLine("---");
            
            //factory method usage
            WriteLine("With factory");
            var ruleCreatedUsingFactory = LogRuleFactory.Import(le => le.Severity == Severity.Info);
            logEntries.Where(log => ruleCreatedUsingFactory.ShouldImport(log)).ToList()
                .ForEach(WriteLine);
            WriteLine("---");
            
            ruleCreatedUsingFactory = LogRuleFactory
                .Import(le => le.Severity == Severity.Warning)
                .Or(le => le.Severity == Severity.Error)
                .And(le => le.DateTime > DateTime.Now.AddDays(-130));
            logEntries.Where(log => ruleCreatedUsingFactory.ShouldImport(log)).ToList()
                .ForEach(WriteLine);
            WriteLine("---");

            logEntries.Filter(ruleCreatedUsingFactory).ToList().ForEach(WriteLine);
            
            ReadKey();
        }
    }
}