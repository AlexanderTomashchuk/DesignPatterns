using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace ResolvingDependencyOfMultipleImplementationsOfSameInterface
{
    interface IService
    {
        void TestMethod();
    }

    class ServiceA : IService
    {
        public void TestMethod()
        {
            Console.WriteLine("ServiceA");
        }
    }
    
    class ServiceB : IService
    {
        public void TestMethod()
        {
            Console.WriteLine("ServiceB");
        }
    }

    class Consumer
    {
        private readonly IService _service;

        public Consumer(ServiceResolver serviceResolver)
        {
            _service = serviceResolver("A");
        }

        public void Test()
        {
            _service.TestMethod();
        }
    }
    
    delegate IService ServiceResolver(string key);
    
    class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient<ServiceA>();
            serviceCollection.AddTransient<ServiceB>();
            serviceCollection.AddTransient<Consumer>();
            serviceCollection.AddTransient<ServiceResolver>(serviceProvider => key =>
            {
                return key switch
                {
                    "A" => serviceProvider.GetService<ServiceA>(),
                    "B" => serviceProvider.GetService<ServiceB>(),
                    _ => throw new KeyNotFoundException()
                };
            });

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var consumer = serviceProvider.GetRequiredService<Consumer>();
            
            consumer.Test();
            
            Console.ReadKey();
        }
    }
}