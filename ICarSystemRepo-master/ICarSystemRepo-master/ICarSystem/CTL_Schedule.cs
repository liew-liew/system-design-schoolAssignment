using ICarSystem;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ICarSystem
{
    public class CTL_Schedule
    {
        private VehicleRegistrationService registrationService;

        public CTL_Schedule(VehicleRegistrationService registrationService)
        {
            this.registrationService = registrationService;
        }

        public List<Vehicle> GetAllVehicles(CarOwner carOwner)
        {
            return carOwner.Vehicles;
        }

        public Vehicle GetVehicle(int vehicleId)
        {
            return Vehicle.GetVehicleById(vehicleId);
        }

        public bool ValidateStartDate(DateTime startDate)
        {
            return startDate >= DateTime.Now;
        }

        public bool ValidateEndDate(DateTime startDate, DateTime endDate)
        {
            // Ensure that the end date is after the start date
            if (endDate <= startDate)
            {
                Console.WriteLine("End date must be after the start date.");
                return false;
            }
            return true;
        }

        public bool OverlapExists(Vehicle vehicle, DateTime startDate, DateTime endDate)
        {
            return vehicle.Availabilities.Any(a => a.EndDate > startDate && a.StartDate < endDate);
        }

        public void FinalizeSchedule(Vehicle vehicle, DateTime startDate, DateTime endDate)
        {
            int newId = vehicle.Availabilities.Count > 0 ? vehicle.Availabilities.Max(a => a.Id) + 1 : 1;
            vehicle.Availabilities.Add(new ScheduleAvailability(newId, startDate, endDate));

            // Display the updated schedule immediately after adding a new availability
            DisplayCurrentScheduledDays(vehicle);
        }

        public void DeleteAvailability(Vehicle vehicle, int dateId)
        {
            var availabilityToRemove = vehicle.Availabilities.FirstOrDefault(a => a.Id == dateId);
            if (availabilityToRemove != null)
            {
                vehicle.Availabilities.Remove(availabilityToRemove);

                // Update the IDs of the remaining availabilities
                int newId = 1;
                foreach (var availability in vehicle.Availabilities)
                {
                    availability.Id = newId++;
                }
            }

            // Display the updated schedule after deletion
            DisplayCurrentScheduledDays(vehicle);
        }

        public void DisplayCurrentScheduledDays(Vehicle vehicle)
        {
            Console.WriteLine($"\nCurrent Scheduled Days for {vehicle.Make} {vehicle.Model} (ID: {vehicle.VehicleID}):");
            if (vehicle.Availabilities.Count == 0)
            {
                Console.WriteLine("No availabilities scheduled.");
            }
            else
            {
                foreach (var availability in vehicle.Availabilities)
                {
                    Console.WriteLine($"ID: {availability.Id}, Start Date: {availability.StartDate:dd-MM-yyyy}, End Date: {availability.EndDate:dd-MM-yyyy}");
                }
            }
        }
    }
}
