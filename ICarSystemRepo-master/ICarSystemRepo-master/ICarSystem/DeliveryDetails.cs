using System;

namespace ICarSystem
{
    public class DeliveryDetails
    {
        public string Name { get; private set; }
        public string ContactNo { get; private set; }
        public string Address { get; private set; }

        public DeliveryDetails(string name, string contactNo, string address)
        {
            Name = name;
            ContactNo = contactNo;
            Address = address;
        }

        public double CalculateDeliveryFee()
        {
            // Logic to calculate delivery fee based on distance or other factors
            return 50.00; // Example flat fee
        }
    }
}
