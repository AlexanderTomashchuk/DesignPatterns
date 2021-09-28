using System;

namespace AdapterExercise
{
    public class Square
    {
        public int Side;
    }

    public interface IRectangle
    {
        int Width { get; }
        int Height { get; }
    }

    public static class ExtensionMethods
    {
        public static int Area(this IRectangle rc)
        {
            return rc.Width * rc.Height;
        }
    }
    
    //You are given an IRectangle interface and an extension method on it. Try to define a 
    //SquareToRectangleAdapter that adapts the Square to the IRectangle interface.
    public class SquareToRectangleAdapter : IRectangle
    {
        private readonly Square _square;
        public SquareToRectangleAdapter(Square square)
        {
            //todo
            _square = square;
        }
        
        //todo
        public int Width => _square.Side;
        public int Height => _square.Side;
    }

    class Program
    {
        static void Main(string[] args)
        {
            var square = new Square { Side = 15 };

            var squareArea = new SquareToRectangleAdapter(square).Area();

            Console.WriteLine($"SquareArea = {squareArea}");
            
            Console.ReadKey();
        }
    }
}