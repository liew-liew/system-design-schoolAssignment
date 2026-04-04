using System;
using System.Collections.Generic;

//------AIMAN---- - //

namespace ICarSystem
{
    class Program
    {
        public static List<Booking> Bookings = new List<Booking>();
        private static Renter testRenter;
        private static CarOwner testCarOwner;
        public static List<Vehicle> allVehicle = new List<Vehicle>();

        static void Main(string[] args)
        {
            // Add dummy data
            AddTestData();

            // Main menu
            VehicleRegistrationService registrationService = new VehicleRegistrationService(); // Initialize as needed

            UI_MainMenu mainMenu = new UI_MainMenu(registrationService);
            mainMenu.MainMenu(testRenter, testCarOwner, allVehicle);
        }

        public static void AddTestData()
        {
            // AIMAN TEST DATA //

            // Initialize dummy Renter and CarOwner
            testRenter = new Renter(1, "Aiman", "password123", true);
            testCarOwner = new CarOwner("Jovan", "securePass");

            // Create and initialize two Vehicles
            Vehicle vehicleWithAvailability = new Vehicle(1, "Toyota", "Camry", 2020, 15000, "url-to-photo", "INS123", "Full Coverage", 100.0m);
            Vehicle vehicleWithoutAvailability = new Vehicle(2, "Honda", "Civic", 2019, 20000, "url-to-photo", "INS456", "Full Coverage", 90.0m);

            // Add the vehicle with availability to the CarOwner's list
            testCarOwner.Vehicles.Add(vehicleWithAvailability);
            testCarOwner.Vehicles.Add(vehicleWithoutAvailability);

            // Create Availability Schedules for the Vehicle with availability
            ScheduleAvailability availability1 = new ScheduleAvailability(1, new DateTime(2024, 08, 01), new DateTime(2024, 08, 10));
            ScheduleAvailability availability2 = new ScheduleAvailability(2, new DateTime(2024, 08, 15), new DateTime(2024, 08, 20));

            // Add the Availabilities to the vehicle
            vehicleWithAvailability.Availabilities.Add(availability1);
            vehicleWithAvailability.Availabilities.Add(availability2);

            // Add vehicles to the allVehicle list
            allVehicle.Add(vehicleWithAvailability);
            allVehicle.Add(vehicleWithoutAvailability);

            // Create and initialize a Booking for the vehicle with availability
            Booking booking = new Booking
            {
                BookingID = "BKG001",
                StartDate = new DateTime(2024, 08, 05, 9, 0, 0),
                EndDate = new DateTime(2024, 08, 10, 9, 0, 0),
                Vehicle = vehicleWithAvailability,
            };
            booking.calculateTotalPrice((double)vehicleWithAvailability.RentalRate, booking.DeliveryFee, booking.ReturnFee);

            // Add the Booking to the Renter
            testRenter.AddBooking(booking);

            iCarStation station1 = new iCarStation("1", "Changi Airport Terminal 3");
            iCarStation station2 = new iCarStation("2", "Marina Bay Sands");
            iCarStation station3 = new iCarStation("3", "Jurong East MRT");

            // Add the Booking to the global list
            Bookings.Add(booking);

            // JOVAN TEST DATA //

            VehicleRegistrationService registrationService = new VehicleRegistrationService();
            Vehicle vehicle1 = new Vehicle(3, "Toyota", "Camry", 2021, 5000, "photos.jpg", "INS123", "Full Coverage", 70.00m);
            Vehicle vehicle2 = new Vehicle(4, "Honda", "Civic", 2020, 3000, "photos2.jpg", "INS456", "Full Coverage", 90.00m);

            // Add some initial availability data
            vehicle1.Availabilities.Add(new ScheduleAvailability(1, new DateTime(2024, 10, 1), new DateTime(2024, 10, 10)));
            vehicle1.Availabilities.Add(new ScheduleAvailability(2, new DateTime(2024, 10, 15), new DateTime(2024, 10, 20)));
            vehicle2.Availabilities.Add(new ScheduleAvailability(1, new DateTime(2024, 9, 1), new DateTime(2024, 9, 5)));
            vehicle2.Availabilities.Add(new ScheduleAvailability(2, new DateTime(2024, 9, 10), new DateTime(2024, 9, 15)));

            // Add some hard-coded booked dates for testing overlap
            vehicle1.Availabilities.Add(new ScheduleAvailability(3, new DateTime(2024, 11, 1), new DateTime(2024, 11, 5)));
            vehicle1.Availabilities.Add(new ScheduleAvailability(4, new DateTime(2024, 11, 10), new DateTime(2024, 11, 15)));
            vehicle2.Availabilities.Add(new ScheduleAvailability(3, new DateTime(2024, 10, 1), new DateTime(2024, 10, 5)));
            vehicle2.Availabilities.Add(new ScheduleAvailability(4, new DateTime(2024, 10, 10), new DateTime(2024, 10, 15)));


            registrationService.AddVehicle(vehicle1);
            registrationService.AddVehicle(vehicle2);

            testCarOwner.Vehicles.Add(vehicle1);
            testCarOwner.Vehicles.Add(vehicle2);
        }
    }
}