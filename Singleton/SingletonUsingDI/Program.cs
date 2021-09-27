using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace SingletonUsingDI
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

    class OrdinaryDatabase : IDatabase
    {
        private readonly Dictionary<string, decimal> _products = new();

        public OrdinaryDatabase()
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
    }

    class ConfigurableRecordFinder
    {
        private readonly IDatabase _database;

        public ConfigurableRecordFinder(IDatabase database)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public decimal GetTotalProductsPrice(params string[] names)
        {
            return names.Sum(name => _database.GetProductPrice(name));
        }
    }

    class DummyDatabase : IDatabase
    {
        public decimal GetProductPrice(string name)
        {
            return new Dictionary<string, int>
            {
                ["alpha"] = 1,
                ["beta"] = 2,
                ["gamma"] = 3
            }[name];
        }
    }
    
    public class UnitTests
    {
        [Fact]
        public void ConfigurableTotalSumTest()
        {
            var configurableRecordFinder = new ConfigurableRecordFinder(new DummyDatabase());
            var names = new[] { "beta", "gamma" };
            configurableRecordFinder.GetTotalProductsPrice(names).Should().Be(5);
        }

        [Fact]
        public void DIPopulationTest()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IDatabase, OrdinaryDatabase>();
            serviceCollection.AddTransient<ConfigurableRecordFinder>();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var configurableRecordFinder = serviceProvider.GetRequiredService<ConfigurableRecordFinder>();

            configurableRecordFinder.GetTotalProductsPrice("Apple", "Orange").Should().Be(34.25M);

            SingletonTester.IsSingleton(() => serviceProvider.GetRequiredService<IDatabase>()).Should().BeTrue();
        }
    }
}