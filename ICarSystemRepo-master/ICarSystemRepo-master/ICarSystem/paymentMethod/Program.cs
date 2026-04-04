using System;
using System.Collections.Generic;

namespace ICarSystem.paymentMethod
{
    public class Program
    {
        public static void Main(string[] args)
        {
            List<Renter> renters = new List<Renter>
            {
                new Renter(1, "John Doe", "john.doe@example.com", "1234567890", 1, "DL123456789", new DateTime(1985, 1, 1), 50m),
                new Renter(2, "Jane Smith", "jane.smith@example.com", "0987654321", 2, "DL987654321", new DateTime(1990, 2, 2), 0m),
                new Renter(3, "Alice Johnson", "alice.johnson@example.com", "1122334455", 3, "DL567890123", new DateTime(1992, 3, 3), 0m)
            };

            List<Booking> bookings = new List<Booking>
            {
                new Booking("B001", DateTime.Now, DateTime.Now.AddDays(7), 500, "Station", "Station"),
                new Booking("B002", DateTime.Now, DateTime.Now.AddDays(7), 600, "Station", "Station"),
                new Booking("B003", DateTime.Now, DateTime.Now.AddDays(7), 200, "Station", "Station"),
                new Booking("B004", DateTime.Now, DateTime.Now.AddDays(7), 200, "Station", "Station"),
                new Booking("B005", DateTime.Now, DateTime.Now.AddDays(7), 100, "Station", "Station"),
                new Booking("B006", DateTime.Now, DateTime.Now.AddDays(7), 100, "Station", "Station")
            };

            List<CreditCard> creditCards = new List<CreditCard>
            {
                new CreditCard(1, "1111222233334444", "John Doe", "12/24", "123", 1000),
                new CreditCard(2, "5555666677778888", "Jane Smith", "11/23", "321", 100),
                new CreditCard(3, "9999000011112222", "Alice Johnson", "10/22", "231", 5000)
            };

            List<DebitCard> debitCards = new List<DebitCard>
            {
                new DebitCard(1, "4444333322221111", "John Doe", "12/24", "123", 1000),
                new DebitCard(2, "8888777766665555", "Jane Smith", "11/23", "321", 100),
                new DebitCard(3, "2222111100009999", "Alice Johnson", "10/22", "231", 5000)
            };

            List<DigitalWallet> digitalWallets = new List<DigitalWallet>
            {
                new DigitalWallet(1, "DW001", "John's Wallet", "1234", 1000),
                new DigitalWallet(2, "DW002", "Jane's Wallet", "5678", 100),
                new DigitalWallet(3, "DW003", "Alice's Wallet", "9101", 5000)
            };

            bool continueRunning = true;

            while (continueRunning)
            {
                int renterIndex = SelectRenter(renters);
                if (renterIndex == -1)
                {
                    break;
                }

                if (renterIndex < 0 || renterIndex >= renters.Count)
                {
                    Console.WriteLine("Invalid renter selection.");
                    continue;
                }

                Renter selectedRenter = renters[renterIndex];
                List<Booking> renterBookings = bookings.GetRange(renterIndex * 2, Math.Min(2, bookings.Count - renterIndex * 2));

                if (renterBookings.Count == 0)
                {
                    Console.WriteLine("No bookings available for this renter.");
                    if (!AskToExitOrReturnToMain())
                    {
                        continueRunning = false;
                        break;
                    }
                    continue;
                }

                bool continueWithBooking = true;

                while (continueWithBooking)
                {
                    int bookingIndex = SelectBooking(renterBookings);
                    if (bookingIndex == -1)
                    {
                        break;
                    }

                    if (bookingIndex < 0 || bookingIndex >= renterBookings.Count)
                    {
                        Console.WriteLine("Invalid booking selection.");
                        continue;
                    }

                    Booking selectedBooking = renterBookings[bookingIndex];
                    decimal totalPriceWithPenalty = (decimal)selectedBooking.TotalPrice + selectedRenter.PenaltyAmount;

                    Console.WriteLine($"Booking Price: {selectedBooking.TotalPrice}");
                    Console.WriteLine($"Penalty: {selectedRenter.PenaltyAmount}");
                    Console.WriteLine($"Total Price with Penalty: {totalPriceWithPenalty}");

                    int paymentMethodIndex = SelectPaymentMethod();
                    Payment payment = new Payment("pay" + selectedBooking.BookingID, totalPriceWithPenalty, PaymentMethod(paymentMethodIndex));

                    bool paymentResult = ProcessPayment(payment, paymentMethodIndex, creditCards, debitCards, digitalWallets, renterIndex, totalPriceWithPenalty);

                    if (paymentResult)
                    {
                        Console.WriteLine("Payment successful.");
                        renterBookings.RemoveAt(bookingIndex); // Remove the booking from the list

                        // Check if more bookings are available for this renter
                        if (renterBookings.Count == 0)
                        {
                            renters.RemoveAt(renterIndex);
                            if (!AskToExitOrReturnToMain())
                            {
                                continueRunning = false;
                                break;
                            }
                            break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Payment failed or balance is insufficient.");
                    }

                    continueWithBooking = AskToContinueOrChangeRenter();
                }
            }
        }

        public static int SelectRenter(List<Renter> renters)
        {
            Console.WriteLine("Select a Renter:");
            for (int i = 0; i < renters.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {renters[i].FullName}");
            }
            Console.WriteLine($"{renters.Count + 1}. Exit");

            if (int.TryParse(Console.ReadLine(), out int selection) && selection > 0 && selection <= renters.Count + 1)
            {
                if (selection == renters.Count + 1) return -1;
                return selection - 1;
            }
            else
            {
                Console.WriteLine("Invalid selection. Please try again.");
                return -1;
            }
        }

        public static int SelectBooking(List<Booking> bookings)
        {
            Console.WriteLine("Select a Booking:");
            for (int i = 0; i < bookings.Count; i++)
            {
                Console.WriteLine($"{i + 1}. Booking ID: {bookings[i].BookingID}, Price: {bookings[i].TotalPrice}");
            }
            Console.WriteLine($"{bookings.Count + 1}. Exit");

            if (int.TryParse(Console.ReadLine(), out int selection) && selection > 0 && selection <= bookings.Count + 1)
            {
                if (selection == bookings.Count + 1) return -1;
                return selection - 1;
            }
            else
            {
                Console.WriteLine("Invalid selection. Please try again.");
                return -1;
            }
        }

        public static int SelectPaymentMethod()
        {
            Console.WriteLine("Select Payment Method:");
            Console.WriteLine("1. Credit Card");
            Console.WriteLine("2. Debit Card");
            Console.WriteLine("3. Digital Wallet");

            if (int.TryParse(Console.ReadLine(), out int selection) && selection >= 1 && selection <= 3)
            {
                return selection;
            }
            else
            {
                Console.WriteLine("Invalid selection. Defaulting to Credit Card.");
                return 1;
            }
        }

        public static string PaymentMethod(int index)
        {
            return index switch
            {
                1 => "CreditCard",
                2 => "DebitCard",
                3 => "DigitalWallet",
                _ => "Unknown"
            };
        }

        public static bool ProcessPayment(Payment payment, int paymentMethodIndex, List<CreditCard> creditCards, List<DebitCard> debitCards, List<DigitalWallet> digitalWallets, int renterIndex, decimal totalPriceWithPenalty)
        {
            bool paymentResult = false;

            switch (paymentMethodIndex)
            {
                case 1: // Credit Card
                    var creditCard = creditCards[renterIndex];
                    Console.WriteLine($"Current Credit Card Balance: {creditCard.Balance}");
                    if (totalPriceWithPenalty <= creditCard.Balance)
                    {
                        Console.WriteLine($"Balance after deduction: {creditCard.Balance - totalPriceWithPenalty}");
                        paymentResult = payment.CompletePayment(creditCard);
                    }
                    else
                    {
                        Console.WriteLine("Credit Card balance is insufficient.");
                    }
                    break;
                case 2: // Debit Card
                    var debitCard = debitCards[renterIndex];
                    Console.WriteLine($"Current Debit Card Balance: {debitCard.Balance}");
                    if (totalPriceWithPenalty <= debitCard.Balance)
                    {
                        Console.WriteLine($"Balance after deduction: {debitCard.Balance - totalPriceWithPenalty}");
                        paymentResult = payment.CompletePayment(debitCard);
                    }
                    else
                    {
                        Console.WriteLine("Debit Card balance is insufficient.");
                    }
                    break;
                case 3: // Digital Wallet
                    var digitalWallet = digitalWallets[renterIndex];
                    int attempts = 5;
                    bool pinCorrect = false;

                    while (attempts > 0 && !pinCorrect)
                    {
                        Console.WriteLine($"Current Digital Wallet Balance: {digitalWallet.Balance}");
                        Console.WriteLine($"Balance after deduction: {digitalWallet.Balance - totalPriceWithPenalty}");
                        Console.Write("Enter PIN:");
                        string enteredPin = Console.ReadLine();

                        if (enteredPin == digitalWallet.Pin)
                        {
                            pinCorrect = true;
                            paymentResult = payment.CompletePayment(digitalWallet, enteredPin);
                        }
                        else
                        {
                            attempts--;
                            Console.WriteLine($"Incorrect PIN. You have {attempts} attempt(s) left.");
                        }
                    }

                    if (!pinCorrect)
                    {
                        Console.WriteLine("Maximum PIN attempts exceeded. Payment failed.");
                    }
                    break;
            }

            return paymentResult;
        }

        public static bool AskToContinueOrChangeRenter()
        {
            Console.Write("Do you want to continue with another booking for the same renter? (y/n): ");
            var response = Console.ReadLine()?.ToLower();
            return response == "y";
        }

        public static bool AskToExitOrReturnToMain()
        {
            Console.Write("Do you want to return to the main menu or exit? (m/e): ");
            var response = Console.ReadLine()?.ToLower();
            return response == "m";
        }
    }
}
