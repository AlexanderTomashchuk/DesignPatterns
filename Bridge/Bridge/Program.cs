using System;
using static System.Console;

namespace Bridge
{
    //Abstraction
    abstract class Payment
    {
        protected readonly IPaymentSystem PaymentSystem;

        protected Payment(IPaymentSystem paymentSystem)
        {
            PaymentSystem = paymentSystem;
        }
        
        public abstract void MakePayment();
    }

    class CreditCardPayment : Payment
    {
        public CreditCardPayment(IPaymentSystem paymentSystem) : base(paymentSystem)
        {
        }
        
        public override void MakePayment()
        {
            PaymentSystem.ProcessPayment("Card Payment");
        }
    }

    class NetBankingPayment : Payment
    {
        public NetBankingPayment(IPaymentSystem paymentSystem) : base(paymentSystem)
        {
        }

        public override void MakePayment()
        {
            PaymentSystem.ProcessPayment("NetBanking Payment");
        }
    }

    //Implementation
    interface IPaymentSystem
    {
        void ProcessPayment(string paymentData);
    }

    class CitiBankPaymentSystem : IPaymentSystem
    {
        public void ProcessPayment(string paymentData)
        {
            WriteLine($"Using Citibank gateway for {paymentData}");
        }
    }

    class IDBIBankPaymentSystem : IPaymentSystem
    {
        public void ProcessPayment(string paymentData)
        {
            WriteLine($"Using IDBIBank gateway for {paymentData}");
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            var payment1 = new CreditCardPayment(new CitiBankPaymentSystem());
            payment1.MakePayment();

            var payment2 = new CreditCardPayment(new IDBIBankPaymentSystem());
            payment2.MakePayment();

            var payment3 = new NetBankingPayment(new CitiBankPaymentSystem());
            payment3.MakePayment();

            ReadKey();
        }
    }
}