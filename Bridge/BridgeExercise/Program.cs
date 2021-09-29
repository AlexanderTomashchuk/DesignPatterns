using System;
using static System.Console;

namespace BridgeExercise
{
    //Abstraction
    abstract class Shape
    {
        protected readonly IRenderer Renderer;

        protected Shape(IRenderer renderer)
        {
            Renderer = renderer;
        }

        public abstract string Name { get; }

        public override string ToString()
        {
            return $"Drawing {Name} as {Renderer.WhatToRenderAs}";
        }
    }

    class Triangle : Shape
    {
        public Triangle(IRenderer renderer) : base(renderer)
        {
        }

        public override string Name => "Triangle";
    }

    class Square : Shape
    {
        public Square(IRenderer renderer) : base(renderer)
        {
        }

        public override string Name => "Square";
    }
    
    //Implementation
    interface IRenderer
    {
        string WhatToRenderAs { get; }
    }

    class RasterRenderer : IRenderer
    {
        public string WhatToRenderAs => "pixels";
    }
    
    class VectorRenderer : IRenderer
    {
        public string WhatToRenderAs => "lines";
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            WriteLine(new Triangle(new RasterRenderer()).ToString()); // returns "Drawing Triangle as pixels"
            WriteLine(new Square(new VectorRenderer()).ToString()); // returns "Drawing Square as lines"
            
            ReadKey();
        }
    }
}