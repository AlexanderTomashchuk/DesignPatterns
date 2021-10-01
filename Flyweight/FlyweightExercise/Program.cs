using System;
using System.Collections.Generic;
using System.Text;

namespace FlyweightExercise
{
    class Sentence
    {
        private readonly string[] words;
        private Dictionary<int, WordToken> _tokens = new();

        public Sentence(string plainText)
        {
            // todo
            words = plainText.Split(" ");
        }

        public WordToken this[int index]
        {
            get
            {
                //todo
                var wordToken = new WordToken();
                _tokens.Add(index, wordToken);
                return wordToken;
            }
        }

        public override string ToString()
        {
            // output formatted text here
            var ws = new List<string>();
            for (var i = 0; i < words.Length; i++)
            {
                var w = words[i];
                if (_tokens.ContainsKey(i) && _tokens[i].Capitalize)
                    w = w.ToUpperInvariant();
                ws.Add(w);
            }
            return string.Join(" ", ws);
        }

        public class WordToken
        {
            public bool Capitalize;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var sentence = new Sentence("this is a test sentence");
            sentence[1].Capitalize = true;
            sentence[2].Capitalize = true;
            Console.WriteLine(sentence);

            Console.ReadKey();
        }
    }
}