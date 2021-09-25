using System;
using System.IO;
using System.Xml.Serialization;

namespace CopyThroughSerialization
{
    static class Extensions
    {
        public static T DeepCopy<T>(this T self)
        {
            using var stream = new MemoryStream();
            var serializer = new XmlSerializer(typeof(T));
            serializer.Serialize(stream, self);
            stream.Seek(0, SeekOrigin.Begin);
            return (T) serializer.Deserialize(stream);
        }
    }
    
    public class Person
    {
        public string[] Names;
        public Address Address;
        //private fields can't be selialized
        private string Password;

        public Person()
        {
            
        }

        public Person(string[] names, Address address, string password)
        {
            Names = names;
            Address = address;
            Password = password;
        }

        public override string ToString()
        {
            return $"{nameof(Names)}: {string.Join(" ", Names)}, {nameof(Address)}: {Address}, {nameof(Password)}: {Password}";
        }
    }

    public class Address
    {
        public string StreetName;
        public int HouseNumber;

        public Address()
        {
            
        }
        
        public Address(string streetName, int houseNumber)
        {
            StreetName = streetName;
            HouseNumber = houseNumber;
        }

        public override string ToString()
        {
            return $"{nameof(StreetName)}: {StreetName}, {nameof(HouseNumber)}: {HouseNumber}";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var john = new Person(new[] { "John", "Smith" }, new Address("London Road", 123), "qwerty");

            var jane = john.DeepCopy();
            jane.Names[0] = "Jane";
            jane.Address.HouseNumber = 234;
            jane.Address.StreetName = "Madison Avenue";
            
            Console.WriteLine(john);
            Console.WriteLine(jane);

            Console.ReadKey();
        }
    }
}