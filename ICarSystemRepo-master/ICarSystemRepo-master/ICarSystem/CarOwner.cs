using System;
using System.Collections.Generic;

namespace ICarSystem
{
    public class CarOwner : User
    {
        public List<Vehicle> Vehicles { get; set; }

        public CarOwner(string username, string password) : base(username, password)
        {
            Vehicles = new List<Vehicle>();
        }

        public bool ScheduleVehicleAvailability(int vehicleID, int scheduleID, DateTime startDate, DateTime endDate)
        {
            var vehicle = Vehicles.Find(v => v.VehicleID == vehicleID);
            if (vehicle != null)
            {
                return vehicle.ScheduleAvailability(scheduleID, startDate, endDate);
            }
            else
            {
                Console.WriteLine("Error: Vehicle not found.");
                return false;
            }
        }

        public Vehicle SelectVehicle()
        {
            while (true)
            {
                Console.WriteLine("\nChoose a vehicle to schedule availability for:");
                for (int i = 0; i < Vehicles.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {Vehicles[i].Make} {Vehicles[i].Model} (ID: {Vehicles[i].VehicleID})");
                }
                Console.WriteLine($"{Vehicles.Count + 1}. Go back");

                Console.Write("Enter the number of the vehicle: ");
                if (int.TryParse(Console.ReadLine(), out int vehicleChoice) && vehicleChoice > 0 && vehicleChoice <= Vehicles.Count)
                {
                    var selectedVehicle = Vehicles[vehicleChoice - 1];
                    selectedVehicle.DisplayAvailabilities();
                    return selectedVehicle;
                }
                else if (vehicleChoice == Vehicles.Count + 1)
                {
                    return null;
                }
                else
                {
                    Console.WriteLine("Invalid vehicle choice. Please try again.");
                }
            }
        }
    }
}
