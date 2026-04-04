using System;
using System.Collections.Generic;

namespace ICarSystem
{
    public class Vehicle
    {
        public int VehicleID { get; set; }

        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public int Mileage { get; set; }
        public string Photos { get; set; }
        public string InsuranceNo { get; set; }
        public string InsuranceCoverage { get; set; }
        public List<ScheduleAvailability> Availabilities { get; set; }

        private static List<Vehicle> vehicleList = new List<Vehicle>();

        public List<Booking> Bookings { get; set; }

        private decimal rentalRate;

        public decimal RentalRate
        {
            get { return rentalRate; }
            set { rentalRate = value; }
        }

        public Vehicle()
        {
            Availabilities = new List<ScheduleAvailability>();
            Bookings = new List<Booking>();
        }

        public Vehicle(int vehicleID, string make, string model, int year, int mileage, string photos, string insuranceNo, string insuranceCoverage, decimal rentalRate)
        {
            VehicleID = vehicleID;
            Make = make;
            Model = model;
            Year = year;
            Mileage = mileage;
            Photos = photos;
            InsuranceNo = insuranceNo;
            InsuranceCoverage = insuranceCoverage;
            RentalRate = rentalRate;
            Availabilities = new List<ScheduleAvailability>();
            Bookings = new List<Booking>();
        }

        public bool ScheduleAvailability(int id, DateTime startDate, DateTime endDate)
        {
            if (endDate <= startDate)
            {
                DisplayErrorMessage("Error: End date and time must be after start date and time.");
                return false;
            }

            if (Availabilities.Any(a => (startDate < a.EndDate && endDate > a.StartDate)))
            {
                DisplayErrorMessage("Error: The entered dates and times conflict with existing bookings.");
                return false;
            }

            var newAvailability = new ScheduleAvailability(id, startDate, endDate);
            Availabilities.Add(newAvailability);

            DisplayConfirmation("Availability successfully scheduled.");
            return true;
        }

        public bool DeleteAvailability(int dateId)
        {
            var availability = Availabilities.FirstOrDefault(a => a.Id == dateId);
            if (availability != null)
            {
                Availabilities.Remove(availability);
                DisplayConfirmation("Availability successfully deleted.");
                return true;
            }
            else
            {
                DisplayErrorMessage("Error: Availability ID not found.");
                return false;
            }
        }

        private void DisplayErrorMessage(string message)
        {
            Console.WriteLine(message);
        }


        private void DisplayConfirmation(string message)
        {
            Console.WriteLine(message);

        }

        public List<DateTime> GetAvailableDates()
        {
            List<DateTime> dates = new List<DateTime>();
            foreach (var availability in Availabilities)
            {
                for (DateTime date = availability.StartDate; date <= availability.EndDate; date = date.AddDays(1))
                {
                    dates.Add(date);
                }
            }
            return dates;
        }

        public bool CheckCarAvailability(DateTime newStartDateTime, DateTime newEndDateTime)
        {
            foreach (var schedule in Availabilities)
            {
                if (newStartDateTime >= schedule.StartDate && newEndDateTime <= schedule.EndDate)
                {
                    return true;
                }
            }
            return false;
        }

        public void DisplayAvailabilities()
        {
            Console.WriteLine($"\nCurrent Availabilities for {Make} {Model} (ID: {VehicleID}):");
            if (Availabilities.Count == 0)
            {
                Console.WriteLine("No availabilities scheduled.");
            }
            else
            {
                foreach (var availability in Availabilities)
                {
                    Console.WriteLine($"ID: {availability.Id}, Start Date: {availability.StartDate}, End Date: {availability.EndDate}");
                }
            }
        }

        public void AddBooking(Booking booking)
        {
            Bookings.Add(booking);
        }

        public static Vehicle GetVehicleById(int vehicleId)
        {
            return vehicleList.FirstOrDefault(v => v.VehicleID == vehicleId);
        }

        public static bool CheckAvailability(int vehicleId, DateTime startDate, DateTime endDate)
        {
            return true ;
        }
    }
}
