using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICarSystem
{
    public class UI_MainMenu
    {
        private CTL_Schedule ctlSchedule;  // Declare the CTL_Schedule object

        public UI_MainMenu(VehicleRegistrationService registrationService) // Pass the service needed by CTL_Schedule
        {
            ctlSchedule = new CTL_Schedule(registrationService);  // Initialize the CTL_Schedule object
        }
        public void MainMenu(Renter testRenter, CarOwner testCarOwner, List<Vehicle> availableVehicles)
        {
            while (true)
            {
                Console.WriteLine("+---------------------------------------------+");
                Console.WriteLine("|                 MAIN MENU                   |");
                Console.WriteLine("+---------------------------------------------+");
                Console.WriteLine("| 1. Login as Renter                          |");
                Console.WriteLine("| 2. Login as CarOwner                        |");
                Console.WriteLine("| 0. Exit                                     |");
                Console.WriteLine("+---------------------------------------------+");
                Console.Write("\nPlease enter your choice: ");

                string choice = Console.ReadLine();
                Console.WriteLine();
                switch (choice)
                {
                    case "1":
                        LoginAsRenter(testRenter, availableVehicles);
                        break;
                    case "2":
                        LoginAsCarOwner(testCarOwner);
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("\nInvalid option. Please try again.\n");
                        break;
                }
            }
        }

        public void LoginAsRenter(Renter testRenter, List<Vehicle> availableVehicles)
        {
            Console.WriteLine($"Logged in as Renter: {testRenter.Username}");
            UI_Renter uiRenter = new UI_Renter();
            uiRenter.RenterMenu(testRenter, availableVehicles);
        }

        public void LoginAsCarOwner(CarOwner testCarOwner)
        {
            Console.WriteLine($"Logged in as Car Owner: {testCarOwner.Username}");
            UI_CarOwner uiCarOwner = new UI_CarOwner(ctlSchedule);
            uiCarOwner.CarOwnerMenu(testCarOwner);
        }
    }
}
