using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CompositeExercise
{
    public interface IValueContainer: IEnumerable<int>
    {
      
    }

    public class SingleValue : IValueContainer
    {
        public int Value;

        public IEnumerator<int> GetEnumerator()
        {
            yield return Value;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class ManyValues : List<int>, IValueContainer
    {
    }

    public static class ExtensionMethods
    {
        public static int Sum(this List<IValueContainer> containers)
        {
            int result = 0;
            foreach (var c in containers)
            foreach (var i in c)
                result += i;
            return result;
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            var sv1 = new SingleValue { Value = 1 };
            var sv2 = new SingleValue { Value = 2 };

            Console.WriteLine($"sv1 + sv2 = {new List<IValueContainer> { sv1, sv2 }.Sum()}");
            
            var mv3 = new ManyValues { 4, -10, 1 };
            
            Console.WriteLine($"sv1 + sv2 + mv3 = {new List<IValueContainer> { sv1, sv2, mv3 }.Sum()}");


            Console.ReadKey();
        }
    }
}