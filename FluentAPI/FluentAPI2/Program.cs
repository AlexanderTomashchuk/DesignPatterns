using System;
using static System.Console;

namespace FluentAPI2
{
    enum CarType
    {
        Sedan,
        Crossover
    }

    class Car :
        ISpecifyCarType,
        ISpecifyWheelSize,
        IBuildCar
    {
        public CarType CarType { get; private set; }
        public int WheelSize { get; private set; }

        private Car()
        {
        }

        public override string ToString()
        {
            return $"{nameof(CarType)}: {CarType}, {nameof(WheelSize)}: {WheelSize}";
        }

        public static ISpecifyCarType Create()
        {
            return new Car();
        }

        public ISpecifyWheelSize OfType(CarType carType)
        {
            CarType = carType;
            return this;
        }

        public IBuildCar WithWheels(int wheelSize)
        {
            switch (CarType)
            {
                case CarType.Crossover when wheelSize < 17 || wheelSize > 20:
                case CarType.Sedan when wheelSize < 15 || wheelSize > 17:
                    throw new ArgumentException($"Wrong size for wheel for {CarType}");
            }

            WheelSize = wheelSize;
            return this;
        }

        public Car Build()
        {
            return this;
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

    class Program
    {
        static void Main(string[] args)
        {
            var car = Car
                .Create()
                .OfType(CarType.Sedan)
                .WithWheels(15)
                .Build();

            var car2 = Car.Create()
                .OfType(CarType.Crossover)
                .WithWheels(18)
                .Build();

            WriteLine(car);
            WriteLine(car2);

            ReadKey();
        }
    }
}