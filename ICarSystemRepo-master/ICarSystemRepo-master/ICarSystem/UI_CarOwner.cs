using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICarSystem
{
    public class UI_CarOwner
    {
        private CTL_Schedule ctlSchedule; // Add this line

        public UI_CarOwner(CTL_Schedule ctlSchedule) // Modify the constructor to accept CTL_Schedule
        {
            this.ctlSchedule = ctlSchedule;
        }
        public void CarOwnerMenu(CarOwner carOwner)
        {
            UI_RegisterCar uiRegisterCar = new UI_RegisterCar();
            UI_Schedule uiSchedule = new UI_Schedule(ctlSchedule);

            while (true)
            {
                Console.WriteLine("\n===============================================");
                Console.WriteLine("                CarOwner Menu");
                Console.WriteLine("===============================================");
                Console.WriteLine("1. View All Cars");
                Console.WriteLine("2. Register Car");
                Console.WriteLine("3. Schedule Availability");
                Console.WriteLine("0. Logout");
                Console.WriteLine("===============================================\n");
                Console.Write("Please select an option: ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        ViewAllCars(carOwner);
                        continue;
                    case "2":
                        //uiRegisterCar.RegisterCar(carOwner);
                        continue;
                    case "3":
                        uiSchedule.Start(carOwner);
                        break; // Add break here to prevent fall-through
                    case "0":
                        Console.WriteLine("\nLog out successful. You have been securely signed out.\n");
                        return;
                    default:
                        Console.WriteLine("\nInvalid option. Please try again.\n");
                        continue;
                }
            }
        }

        private void ViewAllCars(CarOwner carOwner)
        {
            if (carOwner.Vehicles.Count == 0)
            {
                Console.WriteLine("You have no cars registered.\n");
                return;
            }

            Console.WriteLine("===============================================");
            Console.WriteLine("                Your Cars");
            Console.WriteLine("===============================================");

            foreach (var car in carOwner.Vehicles)
            {
                Console.WriteLine($"ID:                            {car.VehicleID}");
                Console.WriteLine($"Make:                          {car.Make}");
                Console.WriteLine($"Model:                         {car.Model}");
                Console.WriteLine($"Year:                          {car.Year}");
                Console.WriteLine($"Mileage:                       {car.Mileage}");
                Console.WriteLine($"Rental Price Per Day (SGD):    {car.RentalRate:C}");
                Console.WriteLine($"Photos:                        {car.Photos}");
                Console.WriteLine($"Insurance Number:              {car.InsuranceNo}");
                Console.WriteLine($"Insurance Coverage:            {car.InsuranceCoverage}");

                Console.WriteLine("-----------------------------------------------");
            }

            Console.WriteLine("===============================================\n");
        }
    }
}
