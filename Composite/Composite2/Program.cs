using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Composite2
{
    class Neuron : IEnumerable<Neuron>
    {
        public float Value { get; set; }

        public List<Neuron> In, Out;

        public IEnumerator<Neuron> GetEnumerator()
        {
            yield return this;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    class NeuronLayer : Collection<Neuron>
    {
        
    }

    static class Extensions
    {
        public static void ConnectTo(this IEnumerable<Neuron> self, IEnumerable<Neuron> other)
        {
            foreach (var from in self)
            {
                foreach (var to in other)
                {
                    from.Out.Add(to);
                    to.In.Add(from);
                }
            }
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            var neuron1 = new Neuron();
            var neuron2 = new Neuron();

            neuron1.ConnectTo(neuron2);

            var neuronLayer1 = new NeuronLayer();
            var neuronLayer2 = new NeuronLayer();
            
            neuronLayer1.ConnectTo(neuronLayer2);
            
            Console.ReadKey();
        }
    }
}