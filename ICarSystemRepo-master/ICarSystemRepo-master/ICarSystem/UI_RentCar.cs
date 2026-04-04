using System;
using System.Collections.Generic;

namespace ICarSystem
{
    // AIMAN //
    public class UI_RentCar
    {
        private CTL_RentCar ctlRentCar;

        public UI_RentCar()
        {
            ctlRentCar = new CTL_RentCar(this); // Initialize the controller
        }

        public void RentCar(int carId, Renter renter)
        {
            ctlRentCar.SetCurrentRenter(renter);  // Set the current renter before starting the booking
            ctlRentCar.GetAvailableDates(carId);  // This will set the currentCar
            ctlRentCar.CreateEmptyBooking();

            List<ScheduleAvailability> dateList = ctlRentCar.GetAvailableDates(carId);
            if (dateList.Count == 0)
            {
                DisplayNoAvailableDates();
                return;
            }

            DisplayDates(dateList);

            // SelectDateRange and check if user has returned to the menu
            if (!SelectDateRange())
            {
                return;
            }

            // SelectPickUp and check if user has returned to the menu
            string pickUpOption = SelectPickUp();
            if (pickUpOption == null)  // Assuming null indicates returning to menu
            {
                return;
            }
            ctlRentCar.AddPickUpToBooking(pickUpOption);

            // SelectReturn and check if user has returned to the menu
            string returnOption = SelectReturn();
            if (returnOption == null)  // Assuming null indicates returning to menu
            {
                return;
            }
            ctlRentCar.AddReturnToBooking(returnOption);

            ctlRentCar.ConfirmBooking();
        }




        public void DisplayNoAvailableDates()
        {
            Console.WriteLine("\nNo available dates for the selected vehicle.");
        }

        public void DisplayDates(List<ScheduleAvailability> dateList)
        {
            Console.WriteLine("\nAvailable Dates:");
            foreach (var availability in dateList)
            {
                Console.WriteLine($"ID: {availability.Id} - From: {availability.StartDate:dd/MM/yyyy} To: {availability.EndDate:dd/MM/yyyy}");
            }
        }

        public bool SelectDateRange()
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("Enter the start date (dd/MM/yyyy) or press 0 to return to the Renter Menu:");
                    string startDateInput = Console.ReadLine().Trim();

                    if (startDateInput == "0")
                    {
                        Console.WriteLine("Returning to the Renter Menu...");
                        return false;  // Indicate that the user wants to return
                    }

                    DateTime startDate = DateTime.ParseExact(startDateInput, "dd/MM/yyyy", null);

                    Console.WriteLine("Enter the end date (dd/MM/yyyy) or press 0 to return to the Renter Menu:");
                    string endDateInput = Console.ReadLine().Trim();

                    if (endDateInput == "0")
                    {
                        Console.WriteLine("Returning to the Renter Menu...");
                        return false;  // Indicate that the user wants to return
                    }

                    DateTime endDate = DateTime.ParseExact(endDateInput, "dd/MM/yyyy", null);

                    if (!ctlRentCar.ValidateDate(startDate, endDate))
                    {
                        DisplayFailure("Invalid date range. Please try again.");
                    }
                    else
                    {
                        ctlRentCar.AddDatesToBooking(startDate, endDate);
                        return true;  // Indicate that the date selection was successful
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Invalid Date Input. Please enter the dates in the correct format (dd/MM/yyyy) or press 0 to return to the Renter Menu.");
                }
            }
        }




        public string SelectPickUp()
        {
            DisplayPickUpOption();
            return Console.ReadLine();
        }

        public void DisplayPickUpOption()
        {
            Console.WriteLine("Choose Pickup Option (Manual Pickup / Delivery):");
        }

        public string SelectReturn()
        {
            DisplayReturnOption();
            return Console.ReadLine();
        }

        public void DisplayReturnOption()
        {
            Console.WriteLine("Choose Return Option (Manual Return / Delivery):");
        }

        public void DisplayStations(List<iCarStation> listOfStations)
        {
            Console.WriteLine("\nAvailable Stations:");
            foreach (var station in listOfStations)
            {
                Console.WriteLine($"Station ID: {station.StationID}, Location: {station.Location}");
            }
        }

        public string SelectPickUpStation()
        {
            Console.WriteLine("Enter the Station ID for pickup:");
            return Console.ReadLine();
        }

        public string SelectReturnStation()
        {
            Console.WriteLine("Enter the Station ID for return:");
            return Console.ReadLine();
        }

        public void DisplayDeliveryForm(out string name, out string contactNo, out string address)
        {
            Console.WriteLine("Enter delivery details:");
            Console.Write("Name: ");
            name = Console.ReadLine();
            Console.Write("Contact No: ");
            contactNo = Console.ReadLine();
            Console.Write("Address: ");
            address = Console.ReadLine();
        }


        public void DisplayBookingSummary(double totalPrice, Booking bookingDetails)
        {
            Console.WriteLine("\nBooking Summary:");
            Console.WriteLine($"Total Price: {totalPrice}");
            Console.WriteLine($"Pickup Option: {bookingDetails.PickupOption}");
            if (bookingDetails.PickupOption == "Delivery")
            {
                Console.WriteLine($"Pickup Delivery Details: {bookingDetails.PickupDeliveryDetails.Name}, {bookingDetails.PickupDeliveryDetails.ContactNo}, {bookingDetails.PickupDeliveryDetails.Address}");
                Console.WriteLine($"Delivery Fee: {bookingDetails.DeliveryFee}");
            }
            else if (bookingDetails.PickupStation != null)
            {
                Console.WriteLine($"Pickup Station: {bookingDetails.PickupStation.Location}");
            }

            Console.WriteLine($"Return Option: {bookingDetails.ReturnOption}");
            if (bookingDetails.ReturnOption == "Delivery")
            {
                Console.WriteLine($"Return Delivery Details: {bookingDetails.ReturnDeliveryDetails.Name}, {bookingDetails.ReturnDeliveryDetails.ContactNo}, {bookingDetails.ReturnDeliveryDetails.Address}");
                Console.WriteLine($"Return Fee: {bookingDetails.ReturnFee}");
            }
            else if (bookingDetails.ReturnStation != null)
            {
                Console.WriteLine($"Return Station: {bookingDetails.ReturnStation.Location}");
            }
        }

        public void DisplaySuccess(string message)
        {
            Console.WriteLine(message);
        }

        public void DisplayFailure(string message)
        {
            Console.WriteLine(message);
        }

        public bool PromptForConfirmation()
        {
            Console.WriteLine("Do you want to proceed with the booking? (yes/no):");
            string input = Console.ReadLine().Trim().ToLower();

            return input == "yes";
        }

    }
}
