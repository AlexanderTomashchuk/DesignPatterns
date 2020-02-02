using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AsynchronousFactoryMethod
{
    internal class Foo
    {
        public string Data;

        private Foo() {}

        public static async Task<Foo> CreateAsync()
        {
            var result = new Foo();
            
            return await result.InitAsync();
        }
        
        private async Task<Foo> InitAsync()
        {
            await Task.Delay(2000);
            Data = await Task.FromResult("Foo data");

            return this;
        } 
    }
    
    class Program
    {
        static async Task Main(string[] args)
        {
            var foo = await Foo.CreateAsync();

            Console.WriteLine(foo.Data);
        }
    }
}