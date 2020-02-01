using System;

namespace ImmutableBuilder
{
    internal class MailMessage
    {
        private string _to, _subject, _body;
        
        private MailMessage() {}
        
        public string To => _to;

        public string Subject => _subject;

        public string Body => _body;

        public override string ToString()
        {
            return $"{nameof(To)}: {To}, {nameof(Subject)}: {Subject}, {nameof(Body)}: {Body}";
        }

        public static MailMessageBuilder With()
        {
            return new MailMessageBuilder();
        }
        
        public class MailMessageBuilder
        {
            private readonly MailMessage _mailMessage = new MailMessage();
            
            public MailMessageBuilder To(string to)
            {
                _mailMessage._to = to;
                return this;
            }

            public MailMessageBuilder Subject(string subject)
            {
                _mailMessage._subject = subject;
                return this;
            }

            public MailMessageBuilder Body(string body)
            {
                _mailMessage._body = body;
                return this;
            }

            public MailMessage Build()
            {
                return _mailMessage;
            }
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            var mailMessage = MailMessage.With()
                .To("ot@gmail.com")
                .Subject("Subject 1")
                .Body("Body 1")
                .Build();
            
            var mailMessage2 = new MailMessage.MailMessageBuilder()
                .To("ot@gmail.com")
                .Subject("Subject 1")
                .Body("Body 1")
                .Build();
            
            Console.WriteLine(mailMessage);
            Console.WriteLine(mailMessage2);
        }
    }
}