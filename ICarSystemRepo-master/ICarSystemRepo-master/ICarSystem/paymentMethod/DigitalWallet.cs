using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICarSystem.paymentMethod
{
    public class DigitalWallet
    {
        public int RenterId { get; private set; }
        public string WalletID { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public string Pin { get; set; }

        // Constructor
        public DigitalWallet(int renterId, string walletID, string name, string pin, decimal initialBalance)
        {
            RenterId = renterId;
            WalletID = walletID;
            Name = name;
            Pin = pin;
            Balance = initialBalance;
        }

        public bool MakePayment(decimal amount, string pin)
        {
            if (pin != Pin || amount > Balance)
            {
                return false;
            }
            Balance -= amount;
            return true;
        }

    }

}
