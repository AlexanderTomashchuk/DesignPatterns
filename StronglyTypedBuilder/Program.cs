using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StronglyTypedBuilder
{
    internal class Email
    {
        // required
        public string To;
        public string Subject;
        
        // optional
        public string Body;
        public List<string> Cc = new List<string>();

        public override string ToString()
        {
            var sb = new StringBuilder();
            
            sb.AppendLine($"{nameof(To)}: {To};");
            sb.AppendLine($"{nameof(Subject)}: {Subject};");

            if (!string.IsNullOrWhiteSpace(Body))
            {
                sb.AppendLine($"{nameof(Body)}: {Body}");
            }

            if (Cc.Any())
            {
                sb.AppendLine($"{nameof(Cc)}:");
                sb.AppendJoin(", ", Cc);
            }

            return sb.ToString();
        }
    }

    internal class EmailBuilder
    {
        private Email email = new Email();

        public EmailSubjectBuilder To(string to)
        {
            email.To = to;
            return new EmailSubjectBuilder(email);
        }
    }
    
    internal class EmailSubjectBuilder
    {
        private Email email;
        public EmailSubjectBuilder(Email email)
        {
            this.email = email;
        }

        public EmailFinalBuilder Subject(string subject)
        {
            email.Subject = subject;
            return new EmailFinalBuilder(email);
        }
    }

    internal class EmailFinalBuilder
    {
        private Email email;

        public EmailFinalBuilder(Email email)
        {
            this.email = email;
        }

        public EmailFinalBuilder Body(string body)
        {
            email.Body = body;
            return this;
        }
        
        public EmailFinalBuilder AddCC(string cc)
        {
            email.Cc.Add(cc);
            return this;
        }

        public Email Build()
        {
            return email;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var email = new EmailBuilder()
                .To("ot@gmail.com")
                .Subject("Subject 1")
                .Body("Body 1")
                .AddCC("kate@gmail.com")
                .AddCC("max@gmail.com")
                .Build();
            
            Console.WriteLine(email);
        }
    }
}