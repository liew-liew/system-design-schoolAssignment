using ICarSystem.paymentMethod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICarSystem
{
    public class Payment
    {
        public string PaymentID { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string PaymentMethod { get; set; } // CreditCard, DebitCard, DigitalWallet

        // Constructor
        public Payment(string paymentID, decimal amount, string paymentMethod)
        {
            PaymentID = paymentID;
            Amount = amount;
            Date = DateTime.Now;
            PaymentMethod = paymentMethod;
        }

        // Method to complete the payment using a credit card
        public bool CompletePayment(CreditCard creditCard)
        {
            if (PaymentMethod != "CreditCard")
            {
                return false;
            }

            return creditCard.MakePurchase(Amount);
        }

        // Method to complete the payment using a debit card
        public bool CompletePayment(DebitCard debitCard)
        {
            if (PaymentMethod != "DebitCard")
            {
                return false;
            }

            return debitCard.MakePurchase(Amount);
        }

        // Method to complete the payment using a digital wallet
        public bool CompletePayment(DigitalWallet digitalWallet, string pin)
        {
            if (PaymentMethod != "DigitalWallet")
            {
                return false;
            }

            return digitalWallet.MakePayment(Amount, pin);
        }
    }


}
