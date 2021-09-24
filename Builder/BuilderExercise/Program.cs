using System;
using System.Collections.Generic;
using System.Text;

namespace BuilderExercise
{
    internal class ClassItem
    {
        public string Name;
        public List<FieldItem> Fields = new List<FieldItem>();
        
        private int IndentSize = 2;

        public override string ToString()
        {
            var sb = new StringBuilder();
            
            sb.AppendLine($"public class {Name}");
            sb.AppendLine("{");
            foreach (var field in Fields)
            {
                sb.Append(new string(' ', IndentSize));
                sb.AppendLine(field.ToString());
            }
            sb.AppendLine("}");

            return sb.ToString();
        }
    }

    internal class FieldItem
    {
        public string Type;
        public string Name;
        
        public FieldItem(string name, string type)
        {
            Type = type;
            Name = name;
        }

        public override string ToString()
        {
            return $"public {Type} {Name};";
        }
    }
    
    internal class CodeBuilder
    {
        private ClassItem _classItem = new ClassItem();
        
        public CodeBuilder(string className)
        {
            _classItem.Name = className;
        }

        public CodeBuilder AddField(string name, string type)
        {
            _classItem.Fields.Add(new FieldItem(name, type));

            return this;
        }
        
        public override string ToString()
        {
            return _classItem.ToString();
        }
    }
    
    internal class Program
    {
        static void Main(string[] args)
        {
            var cb = new CodeBuilder("Person")
                .AddField("Name", "string")
                .AddField("Age", "int");

            Console.WriteLine(cb);
        }
    }
}