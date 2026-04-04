using System;
using System.Globalization;

namespace ICarSystem
{
    public class UI_Schedule
    {
        private readonly CTL_Schedule _ctlSchedule;

        public UI_Schedule(CTL_Schedule ctlSchedule)
        {
            _ctlSchedule = ctlSchedule;
        }

        public void Start(CarOwner carOwner)
        {
            while (true)
            {
                Console.WriteLine("\nMain Menu:");
                Console.WriteLine("1. Select a vehicle to schedule availability");
                Console.WriteLine("2. Delete an availability");
                Console.WriteLine("3. Exit");
                Console.Write("Enter your choice: ");

                string mainChoice = Console.ReadLine();
                if (mainChoice == "3")
                {
                    break;
                }

                switch (mainChoice)
                {
                    case "1":
                        selectScheduleAvailabilityOption(carOwner);
                        break;
                    case "2":
                        selectEndDate(carOwner);
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        continue;
                }
            }
        }

        public void selectScheduleAvailabilityOption(CarOwner carOwner)
        {
            var vehicles = _ctlSchedule.GetAllVehicles(carOwner);

            if (vehicles.Count == 0)
            {
                Console.WriteLine("No vehicles found.");
                return;
            }

            Console.WriteLine("\nChoose a vehicle to schedule availability for:");
            for (int i = 0; i < vehicles.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {vehicles[i].Make} {vehicles[i].Model} (ID: {vehicles[i].VehicleID})");
            }

            Console.Write("Enter the number of the vehicle: ");
            if (int.TryParse(Console.ReadLine(), out int vehicleChoice) && vehicleChoice > 0 && vehicleChoice <= vehicles.Count)
            {
                var selectedVehicle = vehicles[vehicleChoice - 1];
                _ctlSchedule.DisplayCurrentScheduledDays(selectedVehicle);
                AddAvailability(selectedVehicle);
            }
            else
            {
                Console.WriteLine("Invalid vehicle choice. Please try again.");
            }
        }

        private void AddAvailability(Vehicle vehicle)
        {
            while (true)
            {
                Console.Write("Enter start date (dd-MM-yyyy) or type 'back' to choose a different vehicle: ");
                string startDateInput = Console.ReadLine();
                if (startDateInput.ToLower() == "back")
                {
                    break;
                }

                if (!DateTime.TryParseExact(startDateInput, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startDate))
                {
                    Console.WriteLine("Invalid start date format. Please try again.");
                    continue;
                }

                Console.Write("Enter end date (dd-MM-yyyy) or type 'back' to choose a different start date: ");
                string endDateInput = Console.ReadLine();
                if (endDateInput.ToLower() == "back")
                {
                    continue;
                }

                if (!DateTime.TryParseExact(endDateInput, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime endDate))
                {
                    Console.WriteLine("Invalid end date format. Please try again.");
                    continue;
                }

                if (!_ctlSchedule.ValidateStartDate(startDate))
                {
                    Console.WriteLine("Start date is invalid.");
                    continue;
                }

                if (!_ctlSchedule.ValidateEndDate(startDate, endDate))
                {
                    continue;
                }

                if (_ctlSchedule.OverlapExists(vehicle, startDate, endDate))
                {
                    Console.WriteLine("The entered dates overlap with an existing booking.");
                    continue;
                }

                _ctlSchedule.FinalizeSchedule(vehicle, startDate, endDate);

                // Ask if the user wants to add another availability
                Console.WriteLine("Do you want to schedule another availability for this vehicle? (yes/no)");
                string continueInput = Console.ReadLine().ToLower();
                if (continueInput != "yes")
                {
                    break;
                }
            }
        }

        public void selectEndDate(CarOwner carOwner)
        {
            var vehicles = _ctlSchedule.GetAllVehicles(carOwner);

            if (vehicles.Count == 0)
            {
                Console.WriteLine("No vehicles found.");
                return;
            }

            Console.WriteLine("\nChoose a vehicle to delete availability for:");
            for (int i = 0; i < vehicles.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {vehicles[i].Make} {vehicles[i].Model} (ID: {vehicles[i].VehicleID})");
            }

            Console.Write("Enter the number of the vehicle: ");
            if (int.TryParse(Console.ReadLine(), out int vehicleChoice) && vehicleChoice > 0 && vehicleChoice <= vehicles.Count)
            {
                var selectedVehicle = vehicles[vehicleChoice - 1];
                _ctlSchedule.DisplayCurrentScheduledDays(selectedVehicle);
                DeleteAvailability(selectedVehicle);
            }
            else
            {
                Console.WriteLine("Invalid vehicle choice. Please try again.");
            }
        }

        private void DeleteAvailability(Vehicle vehicle)
        {
            Console.Write("Enter the ID of the availability to delete: ");
            int dateId;
            if (int.TryParse(Console.ReadLine(), out dateId))
            {
                _ctlSchedule.DeleteAvailability(vehicle, dateId);

            }
            else
            {
                Console.WriteLine("Invalid ID.");
            }
        }
    }
}
