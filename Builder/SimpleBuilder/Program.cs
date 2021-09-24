using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleBuilder
{
    internal class HtmlElement
    {
        public string Name, Text;
        public List<HtmlElement> Elements = new List<HtmlElement>();
        private int indentSize = 2;

        public HtmlElement()
        {
        }

        public HtmlElement(string name, string text)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Text = text ?? throw new ArgumentNullException(nameof(text));
        }

        private string ToStringImplementation(int indent)
        {
            var sb = new StringBuilder();
            var i = new string(' ', indentSize * indent);
            sb.AppendLine($"{i}<{Name}>");

            if (!string.IsNullOrWhiteSpace(Text))
            {
                sb.Append(new string(' ', indentSize * (indent + 1)));
                sb.AppendLine(Text);
            }

            foreach (var element in Elements)
            {
                sb.Append(element.ToStringImplementation(indent + 1));
            }
            
            sb.AppendLine($"{i}</{Name}>");

            return sb.ToString();
        }

        public override string ToString()
        {
            return ToStringImplementation(0);
        }
    }

    internal class HtmlBuilder
    {
        private string rootName;
        private HtmlElement root = new HtmlElement();

        public HtmlBuilder(string rootName)
        {
            root.Name = rootName;
            this.rootName = rootName;
        }

        public HtmlBuilder AddChild(string childName, string childText)
        {
            var childElement = new HtmlElement(childName, childText);
            
            root.Elements.Add(childElement);

            return this;
        }

        public override string ToString()
        {
            return root.ToString();
        }

        public void Clear()
        {
            root = new HtmlElement {Name = rootName};
        }
    }
    
    internal static class Program
    {
        static void Main(string[] args)
        {
            var builder = new HtmlBuilder("ul");
            builder
                .AddChild("li", "hello")
                .AddChild("li", "world");
            
            Console.WriteLine(builder.ToString());
        }
    }
}