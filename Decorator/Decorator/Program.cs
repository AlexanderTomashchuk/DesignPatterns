using System;
using System.IO;

namespace Decorator
{
    interface IShape
    {
        string AsString();
    }

    class Circle : IShape
    {
        public float Radius { get; }

        public Circle(float radius)
        {
            Radius = radius;
        }

        public string AsString()
        {
            return $"A circle with radius: {Radius}";
        }
    }

    class Square : IShape
    {
        public float Side { get; }

        public Square(float side)
        {
            Side = side;
        }
        
        public string AsString()
        {
            return $"A square with side: {Side}";
        }
    }

    class ColoredShape : IShape
    {
        private IShape _shape;
        private string _color;

        public ColoredShape(IShape shape, string color)
        {
            _shape = shape;
            _color = color;
        }

        public string AsString()
        {
            return $"{_shape.AsString()} has a color: {_color}";
        }
    }

    class TransparentShape : IShape
    {
        private IShape _shape;
        private float _transparency;

        public TransparentShape(IShape shape, float transparency)
        {
            _shape = shape;
            _transparency = transparency;
        }

        public string AsString()
        {
            return $"{_shape.AsString()} has a {_transparency * 100}% transparency";
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            var shape = new Circle(10.5f);
            Console.WriteLine(shape.AsString());

            var coloredShape = new ColoredShape(shape, "red");
            Console.WriteLine(coloredShape.AsString());

            var transparentShape = new TransparentShape(coloredShape, 0.5f);
            Console.WriteLine(transparentShape.AsString());
            
            Console.ReadKey();
        }
    }
}