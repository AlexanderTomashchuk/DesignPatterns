using System;

namespace FactoryExercise
{
    internal class Person
    {
        public int Id { get; }
        public string Name { get; }

        private Person(int id, string name)
        {
            Id = id;
            Name = name;
        }
        
        public static PersonFactory Factory => new PersonFactory();

        internal class PersonFactory
        {
            private static int _currentId = 0; 
            
            public Person CreatePerson(string name)
            {
                return new Person(_currentId++, name);
            }
        }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Name)}: {Name}";
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            var person0 = Person.Factory.CreatePerson("Alex");
            var person1 = Person.Factory.CreatePerson("Kate");
            var person2 = Person.Factory.CreatePerson("Opanas");

            var person3 = new Person.PersonFactory().CreatePerson("Onton");
            
            Console.WriteLine(person0);
            Console.WriteLine(person1);
            Console.WriteLine(person2);
            Console.WriteLine(person3);
        }
    }
}