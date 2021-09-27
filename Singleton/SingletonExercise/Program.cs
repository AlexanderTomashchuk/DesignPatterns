using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SingletonExercise
{
    //Since implementing a singleton is easy, you have a different challenge: write a method called IsSingleton() 
    //This method takes a factory method that returns an object and it's up to you to determine whether or not that
    //object is a singleton instance.
    class SingletonTester
    {
        public static bool IsSingleton(Func<object> func)
        {
            var obj1 = func();
            var obj2 = func();
            return ReferenceEquals(obj1, obj2);
        }
    }

    interface IDatabase
    {
        int GetCityPopulation(string name);
    }

    class SingletonDatabase : IDatabase
    {
        private Dictionary<string, int> _citiesPopulation = new();
        
        private SingletonDatabase()
        {
            Console.WriteLine("SingletonDatabase constructor initialization");
            File.ReadAllLines("citiesPopulation.txt").ToList().ForEach(
                line =>
                {
                    var data = line.Trim().Split("|");
                    _citiesPopulation.Add(data[0], int.Parse(data[1]));
                });
        }
        
        public int GetCityPopulation(string name)
        {
            return _citiesPopulation[name];
        }

        private static Lazy<SingletonDatabase> _instance = new Lazy<SingletonDatabase>(new SingletonDatabase());
        
        public static SingletonDatabase Instance => _instance.Value;
    }

    class Database : IDatabase
    {
        private Dictionary<string, int> _citiesPopulation = new();
        
        public Database()
        {
            Console.WriteLine("Database constructor initialization");
            File.ReadAllLines("citiesPopulation.txt").ToList().ForEach(
                line =>
                {
                    var data = line.Trim().Split("|");
                    _citiesPopulation.Add(data[0], int.Parse(data[1]));
                });
        }
        
        public int GetCityPopulation(string name)
        {
            return _citiesPopulation[name];
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            var cityName = "Mumbai";
            var cityPopulation = SingletonDatabase.Instance.GetCityPopulation(cityName);
            Console.WriteLine($"{cityName} has population {cityPopulation}");

            cityName = "Seoul"; 
            cityPopulation = SingletonDatabase.Instance.GetCityPopulation(cityName);
            Console.WriteLine($"{cityName} has population {cityPopulation}");

            Console.WriteLine("---");
            
            cityName = "Mexico City";
            cityPopulation = new Database().GetCityPopulation(cityName);
            Console.WriteLine($"{cityName} has population {cityPopulation}");

            cityName = "Jakarta";
            cityPopulation = new Database().GetCityPopulation(cityName);
            Console.WriteLine($"{cityName} has population {cityPopulation}");
            
            Console.WriteLine("------");

            var isSingleton = SingletonTester.IsSingleton(() => SingletonDatabase.Instance);
            Console.WriteLine($"It should be true: {isSingleton}");
            
            isSingleton = SingletonTester.IsSingleton(() => new Database());
            Console.WriteLine($"It should be false: {isSingleton}");
        }
    }
}