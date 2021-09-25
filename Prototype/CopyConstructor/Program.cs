using System;

namespace CopyConstructor
{
    class Person
    {
        public string[] Names;
        public Address Address;
        private string Password;

        public Person(Person other)
        {
            Names = (string[])other.Names.Clone();
            Address = new Address(other.Address);
            Password = other.Password;
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

    class Address
    {
        public string StreetName;
        public int HouseNumber;

        public Address(Address other)
        {
            StreetName = other.StreetName;
            HouseNumber = other.HouseNumber;
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

            var jane = new Person(john);
            jane.Names[0] = "Jane";
            jane.Address.HouseNumber = 234;
            jane.Address.StreetName = "Madison Avenue";
            
            Console.WriteLine(john);
            Console.WriteLine(jane);

            Console.ReadKey();
        }
    }
}