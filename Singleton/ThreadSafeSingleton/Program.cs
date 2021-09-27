using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ThreadSafeSingleton
{
    interface IDatabase
    {
        decimal GetProductPrice(string name);
    }

    class SingletonDatabase : IDatabase
    {
        private readonly Dictionary<string, decimal> _products = new();

        private SingletonDatabase()
        {
            Console.WriteLine("Ctor init");
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

        private static Lazy<SingletonDatabase> _instance = new (new SingletonDatabase());

        public static SingletonDatabase Instance => _instance.Value;
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            var productName = "Apple";
            var productPrice = SingletonDatabase.Instance.GetProductPrice(productName);
            Console.WriteLine($"Product '{productName}' has a price {productPrice}");

            productName = "Orange";
            productPrice = SingletonDatabase.Instance.GetProductPrice(productName);
            Console.WriteLine($"Product '{productName}' has a price {productPrice}");
            
            Console.ReadKey();
        }
    }
}