using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AmbientContext
{
    interface ILogger
    {
        void Log(string message);
    }

    class Logger : ILogger
    {
        public void Log(string message)
        {
            var jObject = new JObject();
            jObject.TryAdd("message", message);
            foreach (var logProperty in LoggerContext.ContextStack.LogProperties)
            {
                jObject.TryAdd(logProperty.Key, logProperty.Value);
            }
            
            var log = JsonConvert.SerializeObject(jObject,
                new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore});
            
            Console.WriteLine(log);
        }
    }

    class LoggerContext
    {
        public static IDisposable PushProperty(string key, string value)
        {
            return new ContextStack(key, value);
        }

        public sealed class ContextStack : IDisposable
        {
            private static Stack<KeyValuePair<string, string>> _logProperties = new();

            public static List<KeyValuePair<string, string>> LogProperties
            {
                get
                {
                    var list = _logProperties.ToList();
                    list.Reverse();
                    return list;
                }
            }

            public ContextStack(string key, string value)
            {
                _logProperties.Push(new KeyValuePair<string, string>(key, value));
            }
        
            public void Dispose()
            {
                if (_logProperties.Any())
                {
                    _logProperties.Pop();
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var logger = new Logger();

            logger.Log("Test log message level 0");

            using (LoggerContext.PushProperty("key1", "value1"))
            {
                logger.Log("Test log message level 1");

                using (LoggerContext.PushProperty("key2", "value2"))
                {
                    logger.Log("Test log message level 2");
                }
                
                logger.Log("Test log message level 1");

            }

            logger.Log("Test log message level 0");

            using (LoggerContext.PushProperty("key1", "value1"))
            {
                logger.Log("Test log message level 1");
            }

            Console.ReadKey();
        }
    }
}