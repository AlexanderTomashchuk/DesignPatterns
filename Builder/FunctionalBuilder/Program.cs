using System;
using System.Collections.Generic;

namespace FunctionalBuilder
{
    internal class Person
    {
        public string Name;

        public string Position;

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}, {nameof(Position)}: {Position}";
        }
    }

    internal class PersonBuilder
    {
        public List<Action<Person>> Actions = new List<Action<Person>>();

        public PersonBuilder Called(string name)
        {
            Actions.Add(a => a.Name = name);
            return this;
        }
        
        public Person Build()
        {
            var person = new Person();
            Actions.ForEach(a => a.Invoke(person));
            return person;
        }
    }

    internal static class PersonBuilderExtensions
    {
        public static PersonBuilder WorksAsA(this PersonBuilder personBuilder, string position)
        {
            personBuilder.Actions.Add(a => a.Position = position);
            
            return personBuilder;
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            var pb = new PersonBuilder();

            var person = pb
                .Called("Alex")
                .WorksAsA("Developer")
                .Build();
            
            Console.WriteLine(person.ToString());
        }
    }
}