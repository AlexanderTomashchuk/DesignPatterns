using System;

namespace PrototypeExercise
{
    public class Point
    {
        public int X, Y;

        public override string ToString()
        {
            return $"{nameof(X)}: {X}, {nameof(Y)}: {Y}";
        }
    }

    public class Line
    {
        public Point Start, End;

        public Line DeepCopy()
        {
            // todo
            return new Line
            {
                Start = new Point{ X = Start.X, Y = Start.Y},
                End = new Point {X = End.X, Y = End.Y}
            };
        }

        public override string ToString()
        {
            return $"{nameof(Start)}: {Start}, {nameof(End)}: {End}";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var line1 = new Line { Start = new Point { X = 1, Y = 1 }, End = new Point { X = 2, Y = 2 } };
            
            var line2 = line1.DeepCopy();
            line2.End = new Point { X = 3, Y = 3 };
            
            Console.WriteLine(line1);
            Console.WriteLine(line2);

            Console.ReadKey();
        }
    }
}