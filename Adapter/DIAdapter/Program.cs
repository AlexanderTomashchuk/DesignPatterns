using System;
using System.Collections.Generic;
using Autofac;
using Microsoft.Extensions.DependencyInjection;

namespace DIAdapter
{
    interface ICommand
    {
        void Handle();
    }

    class SaveCommand : ICommand
    {
        public void Handle()
        {
            Console.WriteLine("Saving the file");
        }
    }
    
    class OpenCommand : ICommand
    {
        public void Handle()
        {
            Console.WriteLine("Opening the file");
        }
    }

    class Button
    {
        private readonly ICommand _command;

        public Button(ICommand command)
        {
            _command = command;
        }

        public void Click()
        {
            _command.Handle();
        }
    }
    
    class Editor
    {
        private IEnumerable<Button> _buttons;

        public Editor(IEnumerable<Button> buttons)
        {
            _buttons = buttons;
        }

        public void ClickAll()
        {
            foreach (var button in _buttons)
            {
                button.Click();
            }
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<SaveCommand>().As<ICommand>();
            containerBuilder.RegisterType<OpenCommand>().As<ICommand>();
            containerBuilder.RegisterAdapter<ICommand, Button>(command => new Button(command));
            containerBuilder.RegisterType<Editor>();

            var container = containerBuilder.Build();

            var editor = container.Resolve<Editor>();

            editor.ClickAll();
            
            Console.ReadKey();
        }
    }
}