using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICarSystem
{
    public class Renter : User  // Make Renter class public
    {
        private int renterId;

        public int RenterId


        {
            get { return renterId; }
            set { renterId = value; }
        }

        public bool IsVerified { get; set; }  // Public getter and setter for IsVerified
        private List<Booking> bookings = new List<Booking>();

        public Renter(int renterId, string username, string password, bool isVerified) : base(username, password)
        {
            this.RenterId = renterId;
            this.bookings = new List<Booking>();
            this.IsVerified = isVerified;
        }

        public List<Booking> Bookings
        {
            get { return bookings; }
            set { bookings = value; }
        }

        public void AddBooking(Booking booking)
        {
            bookings.Add(booking);
        }

    }
}
