using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICarSystem.paymentMethod
{
    public class CreditCard
    {
        public int RenterId { get; private set; }
        public string CardNumber { get; set; }
        public string CardHolderName { get; set; }
        public string ExpiryDate { get; set; } // format: MM/YY
        public string CVV { get; set; }
        public decimal Balance { get; set; }

        // Constructor
        public CreditCard(int renterId,string cardNumber, string cardHolderName, string expiryDate, string cvv, decimal initialBalance)
        {
            RenterId = renterId;
            CardNumber = cardNumber;
            CardHolderName = cardHolderName;
            ExpiryDate = expiryDate;
            CVV = cvv;
            Balance = initialBalance;
        }

        public bool MakePurchase(decimal amount)
        {
            if (amount > Balance)
            {
                return false;
            }
            Balance -= amount;
            return true;
        }


    }
}
