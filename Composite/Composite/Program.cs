using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Composite
{
    class GraphicObject
    {
        public virtual string Name { get; set; } = "Group";
        public string Color { get; set; }

        public List<GraphicObject> Children { get; } = new ();

        private void Print(StringBuilder stringBuilder, int depth)
        {
            stringBuilder.Append(new string('*', depth))
                .Append($"{Name} ")
                .Append(string.IsNullOrEmpty(Color) ? string.Empty : $"with color {Color}")
                .AppendLine();
            foreach (var child in Children)
            {
                child.Print(stringBuilder, depth+1);
            }
        }
        
        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            Print(stringBuilder, 0);
            return stringBuilder.ToString();
        }
    }

    class Circle : GraphicObject
    {
        public override string Name => "Circle";
    }

    class Square : GraphicObject
    {
        public override string Name => "Square";
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            var drawing = new GraphicObject { Name = "New drawing" };
            drawing.Children.Add(new Circle {Color = "Red"});
            drawing.Children.Add(new Square {Color = "White"});

            var group = new GraphicObject { Name = "Group1" };
            group.Children.Add(new Square {Color = "Black"});

            var group2 = new GraphicObject { Name = "Group2" };
            group2.Children.Add(new Square { Color = "Yellow" });
            group2.Children.Add(new Circle { Color = "Grey" });
            
            group.Children.Add(group2);
            
            drawing.Children.Add(group);

            Console.WriteLine(drawing);
            
            Console.ReadKey();
        }
    }
}