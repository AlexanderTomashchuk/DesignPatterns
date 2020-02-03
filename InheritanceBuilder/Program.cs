using System;
using System.Reflection.Metadata.Ecma335;

namespace InheritanceBuilder
{
    internal class Person
    {
        public string Name;
        public string Position;
        public DateTime DateOfBirth;

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}, {nameof(Position)}: {Position}, {nameof(DateOfBirth)}: {DateOfBirth}";
        }
    }

    internal abstract class PersonBuilder
    {
        protected Person person = new Person();

        public Person Build()
        {
            return person;
        }
    }
    
    internal class PersonInfoBuilder<TSelf>
        : PersonBuilder
        where TSelf: PersonInfoBuilder<TSelf>
    {
        public TSelf Called(string name)
        {
            person.Name = name;
            return (TSelf) this;
        }
    }

    internal class PersonJobBuilder<TSelf>
        : PersonInfoBuilder<TSelf>
        where TSelf: PersonJobBuilder<TSelf>
    {
        public TSelf WorksAsA(string position)
        {
            person.Position = position;
            return (TSelf) this;
        }
    }

    internal class PersonBirthDateBuilder<TSelf>
        : PersonJobBuilder<TSelf>
        where TSelf : PersonBirthDateBuilder<TSelf>
    {
        public TSelf Born(DateTime dateOfBirth)
        {
            person.DateOfBirth = dateOfBirth;
            return (TSelf) this;
        }
    }

    internal class MyBuilder : PersonBirthDateBuilder<MyBuilder>
    {
    }
    
    internal class Program
    {
        static void Main(string[] args)
        {
            var builder = new MyBuilder();
            
            var person = builder
                .Born(DateTime.Today)
                .WorksAsA("Developer")
                .Called("Alex")
                .Build();

            Console.WriteLine(person);
        }
    }
}