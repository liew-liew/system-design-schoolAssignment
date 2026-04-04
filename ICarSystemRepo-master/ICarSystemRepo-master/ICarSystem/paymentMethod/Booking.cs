using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICarSystem.paymentMethod
{
    public class Booking
    {
        public string BookingID { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public double TotalPrice { get; private set; }
        public string PickupOption { get; private set; }
        public string ReturnOption { get; private set; }

        public Booking(string bookingID, DateTime startDate, DateTime endDate, double totalPrice, string pickupOption, string returnOption)
        {
            BookingID = bookingID;
            StartDate = startDate;
            EndDate = endDate;
            TotalPrice = totalPrice;
            PickupOption = pickupOption;
            ReturnOption = returnOption;
        }


    }

}
