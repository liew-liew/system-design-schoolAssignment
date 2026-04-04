using ICarSystem;
using System;
using System.Collections.Generic;

namespace ICarSystem
{
    public class Booking
    {
        public string BookingID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double TotalPrice { get; set; }
        public string PickupOption { get; set; }
        public string ReturnOption { get; set; }
        public iCarStation PickupStation { get; set; }
        public iCarStation ReturnStation { get; set; }
        public DeliveryDetails PickupDeliveryDetails { get; set; }
        public DeliveryDetails ReturnDeliveryDetails { get; set; }
        public double DeliveryFee { get; set; }
        public double ReturnFee { get; set; }

        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
        public int Id { get; set; }




        public Booking()
        {
            // Initialize fees to zero
            DeliveryFee = 0;
            ReturnFee = 0;
        }

        // Method to add pickup station details to the booking
        public void addPickUpStationToBooking(string pickUpOption, iCarStation selectedPickUpStation)
        {
            PickupOption = pickUpOption;
            PickupStation = selectedPickUpStation;
        }

        // Method to add delivery details to the booking
        public void addDeliveryDetailsToBooking(DeliveryDetails pickUpDeliveryDetails)
        {
            PickupDeliveryDetails = pickUpDeliveryDetails;
        }

        // Method to calculate delivery fee
        public double calculateDeliveryFee(DeliveryDetails pickUpDeliveryDetails)
        {
            // Simulating delivery fee calculation
            DeliveryFee = pickUpDeliveryDetails.CalculateDeliveryFee();
            return DeliveryFee;
        }

        // Method to add return station details to the booking
        public void addReturnStationToBooking(string returnOption, iCarStation selectedReturnStation)
        {
            ReturnOption = returnOption;
            ReturnStation = selectedReturnStation;
        }

        // Method to add return delivery details to the booking
        public void addReturnDeliveryDetailsToBooking(DeliveryDetails returnDeliveryDetails)
        {
            ReturnDeliveryDetails = returnDeliveryDetails;
        }


        // Method to calculate return fee
        public double calculateReturnFee(DeliveryDetails returnDeliveryDetails)
        {
            // Simulating return fee calculation
            ReturnFee = returnDeliveryDetails.CalculateDeliveryFee();
            return ReturnFee;
        }

        // Method to calculate the total price of the booking
        public double calculateTotalPrice(double rentalRate, double deliveryFee, double returnFee)
        {
            TotalPrice = (rentalRate * (EndDate - StartDate).TotalDays) + deliveryFee + returnFee;
            return TotalPrice;
        }


        //  ZHAN YANG PART  //



        //// A list to store bookings
        //private static List<Booking> bookingList = new List<Booking>
        //{
        //    new Booking("BKG001", new DateTime(2024, 8, 7, 9, 0, 0), new DateTime(2024, 8, 9, 9, 0, 0), 100.00, "Office", "Office")
        //    {
        //        Id = 1,
        //        VehicleId = 1,
        //        Vehicle = Vehicle.GetVehicleById(1)
        //    }
        //};

        //public static Booking GetBooking(int bookingId)
        //{
        //    return bookingList.Find(b => b.Id == bookingId);
        //}

        //public void UpdateBooking(DateTime newStartDate, DateTime newEndDate, string newPickUpLocation, string newReturnLocation, int newVehicleId, string user)
        //{
        //    this.StartDate = newStartDate;
        //    this.EndDate = newEndDate;
        //    this.PickupOption = newPickUpLocation;
        //    this.ReturnOption = newReturnLocation;
        //    this.VehicleId = newVehicleId;
        //    this.Vehicle = Vehicle.GetVehicleById(newVehicleId);

        //    // Log modifications to a JSON file (omitted for simplicity)
        //}

        //public static void CancelBooking(int bookingId)
        //{
        //    var booking = GetBooking(bookingId);
        //    if (booking != null)
        //    {
        //        bookingList.Remove(booking);
        //        // Log cancellation to a JSON file (omitted for simplicity)
        //    }
        //}


    }
}
