using System;
using System.Security.Authentication;

namespace FacetedBuilder
{
    internal class Person
    {
        //address
        public string City, StreetAddress, Postcode;
        
        //employment
        public string CompanyName, Position;
        public int AnnualIncome;

        public override string ToString()
        {
            return $"{nameof(City)}: {City}, {nameof(StreetAddress)}: {StreetAddress}, {nameof(Postcode)}: {Postcode}, {nameof(CompanyName)}: {CompanyName}, {nameof(Position)}: {Position}, {nameof(AnnualIncome)}: {AnnualIncome}";
        }
    }

    internal class PersonBuilder //facade
    {
        protected Person person = new Person();
        
        public PersonJobBuilder Works => new PersonJobBuilder(person);
        public PersonAddressBuilder Lives => new PersonAddressBuilder(person);

        public Person Create()
        {
            return person;
        }
    }

    internal class PersonAddressBuilder : PersonBuilder
    {
        public PersonAddressBuilder(Person person)
        {
            this.person = person;
        }

        public PersonAddressBuilder At(string streetAddress)
        {
            person.StreetAddress = streetAddress;
            return this;
        }

        public PersonAddressBuilder WithPostcode(string postcode)
        {
            person.Postcode = postcode;
            return this;
        }
        
        public PersonAddressBuilder In(string city)
        {
            person.City = city;
            return this;
        }
    }
    
    internal class PersonJobBuilder : PersonBuilder
    {
        public PersonJobBuilder(Person person)
        {
            this.person = person;
        }

        public PersonJobBuilder At(string companyName)
        {
            person.CompanyName = companyName;
            return this;
        }

        public PersonJobBuilder AsA(string position)
        {
            person.Position = position;
            return this;
        }

        public PersonJobBuilder Earning(int amount)
        {
            person.AnnualIncome = amount;
            return this;
        }
    }
    
    internal static class Program
    {
        static void Main(string[] args)
        {
            var pb = new PersonBuilder();

            var person = pb
                .Works.At("EPAM").AsA("Engineer").Earning(100000)
                .Lives.At("123 London Road").In("London").WithPostcode("022132")
                .Create();
            
            Console.WriteLine(person.ToString());
        }
    }
}