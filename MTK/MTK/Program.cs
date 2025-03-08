using System;

namespace MTK
{
    class Program
    {
        static void Main()
        {
            bool continueRunning = true;

            while (continueRunning)
            {
                Console.WriteLine("Hello. Welcome to MTK Company. Can we help you?");
                Console.WriteLine("1. Buy car");
                Console.WriteLine("2. Buy home");
                Console.Write("Please choose a service (1 or 2): ");
                string serviceChoice = Console.ReadLine();

                if (serviceChoice == "1")
                {
                    Car_Model cars = new Car_Model();
                    cars.Cars();
                }
                else if (serviceChoice == "2")
                {
                    Home homes = new Home();
                    homes.Homes();
                }
                else
                {
                    Console.WriteLine("Invalid selection. Please restart the program and choose 1 or 2.");
                }

                Console.WriteLine("\nWould you like to do another task? (yes/no): ");
                string userChoice = Console.ReadLine().ToLower();
                if (userChoice == "no")
                {
                    continueRunning = false;
                    Console.WriteLine("Thank you for using MTK Company services! Goodbye.");
                }
                else if (userChoice != "yes")
                {
                    Console.WriteLine("Invalid input. Exiting the program.");
                    continueRunning = false;
                }
            }
        }
    }
}