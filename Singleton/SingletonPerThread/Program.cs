using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SingletonPerThread
{
    static class SingletonTester
    {
        public static bool IsSingleton(Func<object> func)
        {
            var obj1 = func.Invoke();
            var obj2 = func.Invoke();
            return ReferenceEquals(obj1, obj2);
        }
    }
    
    interface IDatabase
    {
        decimal GetProductPrice(string name);
    }

    class SingletonDatabase : IDatabase
    {
        private readonly Dictionary<string, decimal> _products = new();

        public int ThreadId;
        
        private SingletonDatabase()
        {
            Console.WriteLine("Ctor init");
            ThreadId = Thread.CurrentThread.ManagedThreadId;
            File.ReadAllLines("products.txt").ToList().ForEach(
                line =>
                {
                    var data = line.Split("|");
                    _products.Add(data[0], decimal.Parse(data[1]));
                });
        }
        
        public decimal GetProductPrice(string name)
        {
            return _products[name];
        }

        private static ThreadLocal<SingletonDatabase> _instance =
            new ThreadLocal<SingletonDatabase>(() => new SingletonDatabase());

        public static SingletonDatabase Instance => _instance.Value;
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            var task1 = Task.Run(() =>
            {
                var productName = "Apple";
                var productPrice = SingletonDatabase.Instance.GetProductPrice(productName);
                Console.WriteLine($"Product '{productName}' has a price {productPrice}");

                productName = "Orange";
                productPrice = SingletonDatabase.Instance.GetProductPrice(productName);
                Console.WriteLine($"Product '{productName}' has a price {productPrice}");

                Console.WriteLine($"Task1 = {SingletonDatabase.Instance.ThreadId}");
                Console.WriteLine($"Task1 = {SingletonDatabase.Instance.ThreadId}");
                
                Console.WriteLine($"IsSingleton: {SingletonTester.IsSingleton(() => SingletonDatabase.Instance)}");
            });
            
            var task2 = Task.Run(() =>
            {
                var productName = "Banana";
                var productPrice = SingletonDatabase.Instance.GetProductPrice(productName);
                Console.WriteLine($"Product '{productName}' has a price {productPrice}");
                
                Console.WriteLine($"Task2 = {SingletonDatabase.Instance.ThreadId}");
            });

            Task.WaitAll(task1, task2);
            
            Console.ReadKey();
        }
    }
}