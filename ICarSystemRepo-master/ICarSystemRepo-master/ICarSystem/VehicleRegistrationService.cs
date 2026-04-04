using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ICarSystem
{
    public class VehicleRegistrationService
    {
        private List<CarOwner> carOwners = new List<CarOwner>();
        private List<string> validMakes = new List<string> { "Toyota", "Honda", "Tesla" };
        private Dictionary<string, List<string>> validModels = new Dictionary<string, List<string>>
        {
            { "Toyota", new List<string> { "Camry", "Corolla" } },
            { "Honda", new List<string> { "Civic", "Accord" } },
            { "Tesla", new List<string> { "Model S", "Model 3" } }
        };

        public VehicleRegistrationService()
        {
            // Prepopulate with some car owners
            carOwners.Add(new CarOwner("user1", "password1"));
            carOwners.Add(new CarOwner("user2", "password2"));
            carOwners.Add(new CarOwner("user3", "password3"));
        }

        public CarOwner Login(string username, string password)
        {
            return carOwners.FirstOrDefault(co => co.Username == username && co.Password == password);
        }

        public string RegisterVehicle(CarOwner owner, Vehicle vehicle, string imagePath)
        {
            if (owner == null)
            {
                return "Error: Invalid car owner.";
            }

            if (IsDuplicateVehicle(owner, vehicle))
            {
                return "Error: This vehicle has already been registered.";
            }

            string validationResult = ValidateVehicleDetails(vehicle);
            if (!string.IsNullOrEmpty(validationResult))
            {
                return validationResult;
            }

            string imageUploadResult = UploadImage(imagePath);
            if (!string.IsNullOrEmpty(imageUploadResult))
            {
                return imageUploadResult;
            }

            vehicle.Photos = Path.GetFileName(imagePath); // Store the name of the image file
            AssignInsuranceDetails(vehicle);
            owner.Vehicles.Add(vehicle);
            return "Vehicle registration successful!";
        }

        private bool IsDuplicateVehicle(CarOwner owner, Vehicle vehicle)
        {
            return owner.Vehicles.Any(v =>
                v.Make.Equals(vehicle.Make, StringComparison.OrdinalIgnoreCase) &&
                v.Model.Equals(vehicle.Model, StringComparison.OrdinalIgnoreCase) &&
                v.Year == vehicle.Year);
        }

        private string ValidateVehicleDetails(Vehicle vehicle)
        {
            if (string.IsNullOrWhiteSpace(vehicle.Make))
                return "Error: Make is required.";

            if (!validMakes.Contains(vehicle.Make, StringComparer.OrdinalIgnoreCase))
                return "Error: Invalid make.";

            if (string.IsNullOrWhiteSpace(vehicle.Model))
                return "Error: Model is required.";

            if (!validModels.TryGetValue(vehicle.Make, out var models) || !models.Contains(vehicle.Model, StringComparer.OrdinalIgnoreCase))
                return "Error: Invalid model.";

            if (vehicle.Year < 1900 || vehicle.Year > DateTime.Now.Year + 1)
                return "Error: Invalid year.";

            if (vehicle.Mileage < 60)
                return "Error: Minimum mileage is 60.";

            return string.Empty;
        }

        private string UploadImage(string imagePath)
        {
            if (string.IsNullOrWhiteSpace(imagePath))
            {
                return "Error: Image path is required.";
            }

            string absolutePath = Path.GetFullPath(imagePath);

            if (!File.Exists(absolutePath))
            {
                return $"Error: Image file not found at path: {absolutePath}. Please ensure the file path is correct.";
            }

            string extension = Path.GetExtension(absolutePath).ToLower();
            if (extension != ".jpg" && extension != ".png")
            {
                return "Error: Image must be in JPG or PNG format.";
            }

            string uploadsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Uploads");
            string destinationPath = Path.Combine(uploadsDirectory, Path.GetFileName(absolutePath));

            try
            {
                if (!Directory.Exists(uploadsDirectory))
                {
                    Directory.CreateDirectory(uploadsDirectory);
                }

                File.Copy(absolutePath, destinationPath, overwrite: true);
            }
            catch (Exception ex)
            {
                return $"Error uploading image: {ex.Message}";
            }

            return string.Empty;
        }

        private void AssignInsuranceDetails(Vehicle vehicle)
        {
            vehicle.InsuranceNo = Guid.NewGuid().ToString();
            vehicle.InsuranceCoverage = "Standard Coverage";
        }

        public List<Vehicle> GetRegisteredVehicles(CarOwner owner)
        {
            return owner?.Vehicles ?? new List<Vehicle>();
        }

        public Vehicle SelectVehicle()
        {
            var owner = carOwners.FirstOrDefault();
            if (owner == null)
            {
                Console.WriteLine("No car owner found.");
                return null;
            }

            Console.WriteLine("Select a vehicle:");
            for (int i = 0; i < owner.Vehicles.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {owner.Vehicles[i].Make} {owner.Vehicles[i].Model} (ID: {owner.Vehicles[i].VehicleID})");
            }

            int choice;
            if (int.TryParse(Console.ReadLine(), out choice) && choice > 0 && choice <= owner.Vehicles.Count)
            {
                var selectedVehicle = owner.Vehicles[choice - 1];
                selectedVehicle.DisplayAvailabilities();
                return selectedVehicle;
            }

            Console.WriteLine("Invalid choice.");
            return null;
        }

        public void AddVehicle(Vehicle vehicle)
        {
            var owner = carOwners.FirstOrDefault();
            if (owner != null)
            {
                owner.Vehicles.Add(vehicle);
            }
        }
    }
}