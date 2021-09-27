using System;
using System.Threading;

namespace AmbientContext2
{
    public interface IService
    {
        void Test();
        // The pattern is the same regardless of which interface it wraps.
    }

    public class VerySimpleImplementation : IService
    {
        public void Test()
        {
            Console.WriteLine("VerySimpleImplementation");
        }
    }
    
    public class MoreComplicatedImplementation : IService
    {
        public void Test()
        {
            Console.WriteLine("MoreComplicatedImplementation");
        }
    }
    
    public class Service
    {
        static ThreadLocal<IService> _default = new ThreadLocal<IService>(
                () => new VerySimpleImplementation() // default value
            );

        public static IService Default => _default.Value;

        public static SavedThreadLocal<IService> SetDefault(IService newValue)
        {
            if (newValue == null)
                throw new ArgumentNullException(nameof(newValue));
            return new SavedThreadLocal<IService>(_default, newValue);
        }
    }
    
    public struct SavedThreadLocal<T> : IDisposable
    {
        T _oldValue;
        ThreadLocal<T> _variable;

        public SavedThreadLocal(ThreadLocal<T> variable, T newValue)
        {
            _variable = variable;
            _oldValue = variable.Value;
            variable.Value = newValue;
        }
        public void Dispose()
        {
            _variable.Value = _oldValue;
        }

        public T OldValue { get { return _oldValue; } }
        public T Value { get { return _variable.Value; } }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            Service.Default.Test();

            using (Service.SetDefault(new MoreComplicatedImplementation()))
            {
                Service.Default.Test();
            }
            
            Service.Default.Test();
        }
    }
}