using System;
using static System.Console;

namespace StepwiseBuilder
{
    enum CarType
    {
        Sedan,
        Crossover
    }

    class Car
    {
        public CarType Type;
        public int WheelSize;

        public override string ToString()
        {
            return $"{nameof(Type)}: {Type}, {nameof(WheelSize)}: {WheelSize}";
        }
    }

    interface ISpecifyCarType
    {
        ISpecifyWheelSize OfType(CarType carType);
    }
    
    interface ISpecifyWheelSize
    {
        IBuildCar WithWheels(int wheelSize);
    }
    
    interface IBuildCar
    {
        Car Build();
    }

    class CarBuilder
    {
        public static ISpecifyCarType Create()
        {
            return new Impl();
        }

        private class Impl : 
            ISpecifyCarType,
            ISpecifyWheelSize,
            IBuildCar
        {
            private Car car = new();

            public ISpecifyWheelSize OfType(CarType type)
            {
                car.Type = type;
                return this;
            }

            public IBuildCar WithWheels(int size)
            {
                switch (car.Type)
                {
                    case CarType.Crossover when size < 17 || size > 20:
                    case CarType.Sedan when size < 15 || size > 17:
                        throw new ArgumentException($"Wrong size of wheel for {car.Type}.");
                }
                car.WheelSize = size;
                return this;
            }

            public Car Build()
            {
                return car;
            }
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            var car = CarBuilder
                .Create()
                .OfType(CarType.Crossover)
                .WithWheels(18)
                .Build();
            
            WriteLine(car);
            
            ReadKey();
        }
    }
}