using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICarSystem
{
    public class UI_Renter
    {
        private UI_RentCar uiRentCar;

        public UI_Renter()
        {
            uiRentCar = new UI_RentCar(); // Initialize UI_RentCar
        }

        public void RenterMenu(Renter currentRenter, List<Vehicle> availableVehicles)
        {
            while (true)
            {
                Console.WriteLine("\n===============================================");
                Console.WriteLine("                  Renter Menu");
                Console.WriteLine("===============================================");
                Console.ResetColor();

                // Display menu options
                Console.WriteLine("1. View Bookings");
                Console.WriteLine("2. Rent Car");
                Console.WriteLine("3. Modify Booking");
                Console.WriteLine("0. Logout");

                Console.WriteLine("===============================================\n");
                Console.Write("Please select an option: ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        if (currentRenter.Bookings.Count > 0)
                        {
                            foreach (var booking in currentRenter.Bookings)
                            {
                                DisplayBookingDetails(booking);
                            }
                        }
                        else
                        {
                            Console.WriteLine("No bookings found.");
                        }
                        break;
                    case "2":
                        DisplayAvailableVehicles(availableVehicles);
                        Console.WriteLine("Please enter the Vehicle ID of the car you want to rent: ");
                        int selectedVehicleID;
                        if (int.TryParse(Console.ReadLine(), out selectedVehicleID))
                        {
                            Vehicle selectedCar = availableVehicles.Find(v => v.VehicleID == selectedVehicleID);
                            if (selectedCar != null)
                            {
                                uiRentCar.RentCar(selectedCar.VehicleID, currentRenter); // Redirect to UI_RentCar's RentCar method
                            }
                            else
                            {
                                Console.WriteLine("Invalid Vehicle ID. Please try again.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid input. Please enter a valid Vehicle ID.");
                        }
                        break;
                    case "3":
                        // Call a method to modify booking (to be implemented based on your structure)
                        Console.WriteLine("Modify booking functionality is under development.");
                        break;
                    case "0":
                        Console.WriteLine("\nLog out successful. You have been securely signed out.\n");
                        return;
                    default:
                        Console.WriteLine("\nInvalid option. Please try again.\n");
                        break;
                }
            }
        }

        public void DisplayAvailableVehicles(List<Vehicle> vehicles)
        {
            Console.WriteLine("\nAvailable Vehicles:");
            Console.WriteLine("===============================================");
            foreach (var vehicle in vehicles)
            {
                Console.WriteLine($"ID: {vehicle.VehicleID}");
                Console.WriteLine($"Make: {vehicle.Make}");
                Console.WriteLine($"Model: {vehicle.Model}");
                Console.WriteLine($"Year: {vehicle.Year}");
                Console.WriteLine($"Mileage: {vehicle.Mileage}");
                Console.WriteLine($"Rental Rate: ${vehicle.RentalRate}/day");
                Console.WriteLine("===============================================\n");
            }
        }

        public void DisplayBookingDetails(Booking aBooking)
        {
            Console.WriteLine("\n\n===============================================");
            Console.WriteLine($"             Booking {aBooking.BookingID} Details");
            Console.WriteLine("===============================================");
            Console.WriteLine($"Booking ID:         {aBooking.BookingID}");
            Console.WriteLine($"Car ID:             {aBooking.Vehicle.VehicleID}");
            Console.WriteLine($"Start Date:         {aBooking.StartDate.ToString("dd/MM/yyyy h:mm:ss tt")}");
            Console.WriteLine($"End Date:           {aBooking.EndDate.ToString("dd/MM/yyyy h:mm:ss tt")}");
            Console.WriteLine($"Amount (SGD):       {aBooking.TotalPrice}");

            // Display pickup details
            if (aBooking.PickupOption == "Delivery" && aBooking.PickupDeliveryDetails != null)
            {
                Console.WriteLine("Pickup Delivery Details:");
                Console.WriteLine($"    Name: {aBooking.PickupDeliveryDetails.Name}");
                Console.WriteLine($"    Contact No: {aBooking.PickupDeliveryDetails.ContactNo}");
                Console.WriteLine($"    Address: {aBooking.PickupDeliveryDetails.Address}");
                Console.WriteLine($"    Delivery Fee: {aBooking.DeliveryFee}");
            }
            else if (aBooking.PickupStation != null)
            {
                Console.WriteLine($"Pickup Location:    {aBooking.PickupStation.Location}");
            }

            // Display return details
            if (aBooking.ReturnOption == "Delivery" && aBooking.ReturnDeliveryDetails != null)
            {
                Console.WriteLine("Return Delivery Details:");
                Console.WriteLine($"    Name: {aBooking.ReturnDeliveryDetails.Name}");
                Console.WriteLine($"    Contact No: {aBooking.ReturnDeliveryDetails.ContactNo}");
                Console.WriteLine($"    Address: {aBooking.ReturnDeliveryDetails.Address}");
                Console.WriteLine($"    Return Fee: {aBooking.ReturnFee}");
            }
            else if (aBooking.ReturnStation != null)
            {
                Console.WriteLine($"Return Location:    {aBooking.ReturnStation.Location}");
            }

            Console.WriteLine("===============================================\n");
        }

    }
}
