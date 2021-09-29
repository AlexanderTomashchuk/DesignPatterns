using System;
using System.Collections.Generic;
using System.Linq;
using static System.Console;

namespace CompositeSpecification
{
    public enum Color
    {
        Red,
        Green,
        Blue
    }

    public enum Size
    {
        Small,
        Medium,
        Large,
        Yuge
    }

    public class Product
    {
        public string Name;
        public Color Color;
        public Size Size;

        public Product(string name, Color color, Size size)
        {
            Name = name ?? throw new ArgumentNullException(paramName: nameof(name));
            Color = color;
            Size = size;
        }
    }

    public class ProductFilter
    {
        // let's suppose we don't want ad-hoc queries on products
        public IEnumerable<Product> FilterByColor(IEnumerable<Product> products, Color color)
        {
            foreach (var p in products)
                if (p.Color == color)
                    yield return p;
        }

        public static IEnumerable<Product> FilterBySize(IEnumerable<Product> products, Size size)
        {
            foreach (var p in products)
                if (p.Size == size)
                    yield return p;
        }

        public static IEnumerable<Product> FilterBySizeAndColor(IEnumerable<Product> products, Size size, Color color)
        {
            foreach (var p in products)
                if (p.Size == size && p.Color == color)
                    yield return p;
        } // state space explosion
        // 3 criteria = 7 methods

        // OCP = open for extension but closed for modification
    }

    // we introduce two new interfaces that are open for extension

    public abstract class Specification<T>
    {
        public abstract bool IsSatisfied(Product p);
    }

    public abstract class CompositeSpecification<T> : Specification<T>
    {
        protected readonly Specification<T>[] _specifications;

        protected CompositeSpecification(params Specification<T>[] specifications)
        {
            _specifications = specifications;
        }
    }

    // combinator
    public class AndSpecification<T> : CompositeSpecification<T>
    {
        public AndSpecification(params Specification<T>[] specifications) : base(specifications) 
        {
        }

        public override bool IsSatisfied(Product p)
        {
            return _specifications.All(sp => sp.IsSatisfied(p));
        }
    }

    public class OrSpecification<T> : CompositeSpecification<T>
    {
        public OrSpecification(params Specification<T>[] specifications) : base(specifications)
        {
        }

        public override bool IsSatisfied(Product p) => _specifications.Any(sp => sp.IsSatisfied(p));
    }

    public class ColorSpecification : Specification<Product>
    {
        private Color color;

        public ColorSpecification(Color color)
        {
            this.color = color;
        }

        public override bool IsSatisfied(Product p)
        {
            return p.Color == color;
        }
    }

    public class SizeSpecification : Specification<Product>
    {
        private Size size;

        public SizeSpecification(Size size)
        {
            this.size = size;
        }

        public override bool IsSatisfied(Product p)
        {
            return p.Size == size;
        }
    }

    public interface IFilter<T>
    {
        IEnumerable<T> Filter(IEnumerable<T> items, Specification<T> spec);
    }
    
    public class BetterFilter : IFilter<Product>
    {
        public IEnumerable<Product> Filter(IEnumerable<Product> items, Specification<Product> spec)
        {
            foreach (var i in items)
                if (spec.IsSatisfied(i))
                    yield return i;
        }
    }

    public class Demo
    {
        static void Main(string[] args)
        {
            var apple = new Product("Apple", Color.Green, Size.Small);
            var tree = new Product("Tree", Color.Green, Size.Large);
            var house = new Product("House", Color.Blue, Size.Large);

            Product[] products = { apple, tree, house };

            var pf = new ProductFilter();
            WriteLine("Green products (old):");
            foreach (var p in pf.FilterByColor(products, Color.Green))
                WriteLine($" - {p.Name} is green");

            // ^^ BEFORE

            // vv AFTER
            var bf = new BetterFilter();
            WriteLine("Green products (new):");
            foreach (var p in bf.Filter(products, new ColorSpecification(Color.Green)))
                WriteLine($" - {p.Name} is green");

            WriteLine("Large products");
            foreach (var p in bf.Filter(products, new SizeSpecification(Size.Large)))
                WriteLine($" - {p.Name} is large");

            WriteLine("Large blue items");
            foreach (var p in bf.Filter(products,
                new AndSpecification<Product>(new ColorSpecification(Color.Blue), new SizeSpecification(Size.Large)))
            )
            {
                WriteLine($" - {p.Name} is big and blue");
            }
            
            WriteLine("Large or small green items");
            foreach (var p in bf.Filter(products,
                new AndSpecification<Product>(
                    new OrSpecification<Product>(
                        new SizeSpecification(Size.Large), new SizeSpecification(Size.Small)),
                    new ColorSpecification(Color.Green)))
            )
            {
                WriteLine($" - {p.Name} is {p.Size.ToString().ToLower()} and {p.Color.ToString().ToLower()}");
            }
        }
    }
}