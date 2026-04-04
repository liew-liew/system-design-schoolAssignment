using System;
using System.Collections.Generic;

namespace ICarSystem
{
    // AIMAN //
    public class CTL_RentCar
    {
        private UI_RentCar uiRentCar;
        private Booking currentBooking;
        private Vehicle currentCar;
        private Renter currentRenter;  // Tracks the current renter

        public CTL_RentCar(UI_RentCar ui)
        {
            uiRentCar = ui;
        }

        public void CreateEmptyBooking()
        {
            if (currentCar == null)
            {
                throw new InvalidOperationException("No car selected for booking.");
            }

            currentBooking = new Booking();
            currentBooking.Vehicle = currentCar;  // Ensure vehicle is assigned to the booking
        }

        public void SetCurrentRenter(Renter renter)
        {
            currentRenter = renter;
        }

        public List<ScheduleAvailability> GetAvailableDates(int carId)
        {
            currentCar = FindCar(carId);
            return currentCar?.Availabilities ?? new List<ScheduleAvailability>();
        }

        public bool ValidateDate(DateTime startDate, DateTime endDate)
        {
            return startDate < endDate && currentCar?.CheckCarAvailability(startDate, endDate) == true;
        }

        public void AddDatesToBooking(DateTime startDate, DateTime endDate)
        {
            currentBooking.StartDate = startDate;
            currentBooking.EndDate = endDate;
        }

        public void AddPickUpToBooking(string pickUpOption)
        {
            currentBooking.PickupOption = pickUpOption;

            if (pickUpOption == "Manual Pickup")
            {
                List<iCarStation> stations = GetAllStations();
                uiRentCar.DisplayStations(stations);
                string stationId = uiRentCar.SelectPickUpStation();
                var selectedStation = stations.Find(st => st.StationID == stationId);
                AddPickUpStationToBooking(selectedStation);

                currentBooking.DeliveryFee = 0;
            }
            else if (pickUpOption == "Delivery")
            {
                uiRentCar.DisplayDeliveryForm(out string name, out string contactNo, out string address);
                DeliveryDetails pickupDetails = new DeliveryDetails(name, contactNo, address);
                currentBooking.PickupDeliveryDetails = pickupDetails;
                currentBooking.DeliveryFee = currentBooking.calculateDeliveryFee(pickupDetails);
            }
        }

        public void AddPickUpStationToBooking(iCarStation selectedStation)
        {
            currentBooking.PickupStation = selectedStation;
        }

        public void AddReturnToBooking(string returnOption)
        {
            currentBooking.ReturnOption = returnOption;

            if (returnOption == "Manual Return")
            {
                List<iCarStation> stations = GetAllStations();
                uiRentCar.DisplayStations(stations);
                string stationId = uiRentCar.SelectReturnStation();
                var selectedStation = stations.Find(st => st.StationID == stationId);
                AddReturnStationToBooking(selectedStation);

                currentBooking.ReturnFee = 0;
            }
            else if (returnOption == "Delivery")
            {
                uiRentCar.DisplayDeliveryForm(out string name, out string contactNo, out string address);
                DeliveryDetails returnDetails = new DeliveryDetails(name, contactNo, address);
                currentBooking.ReturnDeliveryDetails = returnDetails;
                currentBooking.ReturnFee = currentBooking.calculateReturnFee(returnDetails);
            }
        }

        public void AddReturnStationToBooking(iCarStation selectedStation)
        {
            currentBooking.ReturnStation = selectedStation;
        }

        public void ConfirmBooking()
        {
            double totalPrice = CalculateTotalPrice();

            // Display the booking summary
            uiRentCar.DisplayBookingSummary(totalPrice, currentBooking);

            // Prompt the user for confirmation
            bool isConfirmed = uiRentCar.PromptForConfirmation();

            if (isConfirmed)
            {
                bool paymentSuccessful = MakePayment(currentBooking.BookingID, totalPrice);

                if (paymentSuccessful)
                {
                    currentRenter.AddBooking(currentBooking);
                    AddBookingToCar(currentBooking);
                    uiRentCar.DisplaySuccess("Booking successful");
                }
                else
                {
                    DeleteCurrentBooking();
                    uiRentCar.DisplayFailure("Booking failed");
                }
            }
            else
            {
                DeleteCurrentBooking(); // Optional: Clear the booking if not confirmed
                uiRentCar.DisplayFailure("Booking canceled by user.");
            }
        }



        public double CalculateTotalPrice()
        {
            return currentBooking.calculateTotalPrice((double)currentCar.RentalRate, currentBooking.DeliveryFee, currentBooking.ReturnFee);
        }

        public bool MakePayment(string bookingId, double totalPrice)
        {
            return true; // Simulated payment always succeeds
        }

        public List<iCarStation> GetAllStations()
        {
            return new List<iCarStation>
        {
            new iCarStation("1", "Changi Airport Terminal 3"),
            new iCarStation("2", "Marina Bay Sands"),
            new iCarStation("3", "Jurong East MRT")
        };
        }

        private Vehicle FindCar(int vehicleId)
        {
            return Program.allVehicle.Find(vehicle => vehicle.VehicleID == vehicleId);
        }

        public void DeleteCurrentBooking()
        {
            currentBooking = null;
            Console.WriteLine("Current booking has been deleted.");
        }

        public void AddBookingToCar(Booking booking)
        {
            currentCar?.AddBooking(booking);  // Ensure currentCar is not null
        }
    }
}
