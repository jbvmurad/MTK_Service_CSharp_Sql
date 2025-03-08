using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Text.RegularExpressions;

namespace MTK
{
    public class Car_Model
    {
        public string Person { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public int Motors { get; set; }
        public decimal Value { get; set; }
        public string Country { get; set; }

        private List<string> maleNamesAZ = new List<string>
        {
            "Ali", "Veli", "Cemil", "Elvin", "Ruslan", "Tural", "Samir", "Orxan", "Elnur", "Ramin",
            "Nurlan", "Farid", "Anar", "Rövşen", "Elşən", "İlqar", "Vüqar", "Şahin", "Rəşad", "Natiq",
            "Emin", "Rəhman", "Teymur", "Rəşid", "Fərid", "Nizami", "Şamil", "Rüstəm", "Vidadi", "Zaur",
            "Elbrus", "Rövşən", "Nadir", "Elşad", "Rəcəb", "Sərvər", "Rəvan", "Tərlan", "Eldar", "Rəşat",
            "Nurlan", "Faiq", "Rəşid", "Elmar", "Rəşad", "Natiq", "Emin", "Rəhman", "Teymur", "Rəşid","Murad"
        };

        private List<string> femaleNamesAZ = new List<string>
        {
            "Aysel", "Gülşen", "Leyla", "Nergiz", "Sevda", "Aytac", "Günel", "Narmin", "Sevil", "Zümrüd",
            "Lala", "Nazlı", "Səbinə", "Türkan", "Zərifə", "Aynur", "Gülay", "Nərmin", "Səadət", "Zəhra",
            "Leyla", "Nigar", "Səbinə", "Türkan", "Zərifə", "Aynur", "Gülay", "Nərmin", "Səadət", "Zəhra",
            "Leyla", "Nigar", "Səbinə", "Türkan", "Zərifə", "Aynur", "Gülay", "Nərmin", "Səadət", "Zəhra",
            "Leyla", "Nigar", "Səbinə", "Türkan", "Zərifə", "Aynur", "Gülay", "Nərmin", "Səadət", "Zəhra","Fatime"
        };

        private List<string> maleNamesEN = new List<string>
        {
            "John", "Michael", "David", "James", "Robert", "William", "Joseph", "Charles", "Thomas", "Daniel",
            "Matthew", "Anthony", "Mark", "Paul", "Steven", "Andrew", "Kenneth", "Joshua", "Kevin", "Brian",
            "George", "Edward", "Ronald", "Timothy", "Jason", "Jeffrey", "Ryan", "Jacob", "Gary", "Nicholas",
            "Eric", "Jonathan", "Stephen", "Larry", "Justin", "Scott", "Brandon", "Benjamin", "Samuel", "Frank",
            "Gregory", "Raymond", "Alexander", "Patrick", "Jack", "Dennis", "Jerry", "Tyler", "Aaron", "Jose"
        };

        private List<string> femaleNamesEN = new List<string>
        {
            "Emma", "Olivia", "Ava", "Sophia", "Isabella", "Mia", "Amelia", "Harper", "Evelyn", "Abigail",
            "Emily", "Elizabeth", "Mila", "Ella", "Avery", "Sofia", "Camila", "Aria", "Scarlett", "Victoria",
            "Madison", "Luna", "Grace", "Chloe", "Penelope", "Layla", "Riley", "Zoey", "Nora", "Lily",
            "Eleanor", "Hannah", "Lillian", "Addison", "Aubrey", "Ellie", "Stella", "Natalie", "Zoe", "Leah",
            "Hazel", "Violet", "Aurora", "Savannah", "Audrey", "Brooklyn", "Bella", "Claire", "Skylar", "Lucy"
        };

        private List<Car_Model> CarsList = new List<Car_Model>();

        public void Cars()
        {
            Console.WriteLine("\n--- CarsModel ---");

            Console.WriteLine("Enter your name: ");
            string personName = Console.ReadLine();
            Person = GenerateName(personName);

            Console.WriteLine("Enter your email: ");
            string email = Console.ReadLine();
            if (ValidateEmail(email))
            {
                Email = email;
            }
            else
            {
                Console.WriteLine("Invalid email format.");
                return;
            }

            FillCarData();

            Console.WriteLine("Enter Car name which you want to get (Toyota, Mercedes, BMW): ");
            Name = Console.ReadLine();

            ShowCarModels(Name);

            Console.WriteLine("Enter the model you want to buy: ");
            string selectedModel = Console.ReadLine();

            var selectedCar = CarsList.Find(c => c.Model == selectedModel);
            if (selectedCar != null)
            {
                selectedCar.Person = Person;  
                selectedCar.Email = Email;    

                
                string message = GeneratePurchaseMessage(personName);
                Console.WriteLine(message);

                Console.WriteLine($"Selected Car: {selectedCar.Model}, Year: {selectedCar.Year}, Motors: {selectedCar.Motors}, Value: {selectedCar.Value}, Country: {selectedCar.Country}");
                AddToDatabase(selectedCar);
            }
            else
            {
                Console.WriteLine("Invalid model selection.");
            }
        }

        private void FillCarData()
        {
            CarsList.Add(new Car_Model { Name = "Toyota", Model = "Corolla", Year = 2020, Motors = 1600, Value = 30000, Country = "Japan" });
            CarsList.Add(new Car_Model { Name = "Toyota", Model = "Camry", Year = 2021, Motors = 2000, Value = 40000, Country = "Japan" });
            CarsList.Add(new Car_Model { Name = "Mercedes", Model = "C200", Year = 2019, Motors = 1800, Value = 50000, Country = "Germany" });
            CarsList.Add(new Car_Model { Name = "Mercedes", Model = "E250", Year = 2022, Motors = 2200, Value = 60000, Country = "Germany" });
            CarsList.Add(new Car_Model { Name = "BMW", Model = "320i", Year = 2020, Motors = 2000, Value = 45000, Country = "Germany" });
            CarsList.Add(new Car_Model { Name = "BMW", Model = "X5", Year = 2021, Motors = 3000, Value = 70000, Country = "Germany" });
        }

        private void ShowCarModels(string carName)
        {
            Console.WriteLine($"Available models for {carName}:");
            foreach (var car in CarsList)
            {
                if (car.Name.Equals(carName, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"Model: {car.Model}, Year: {car.Year}, Motors: {car.Motors}, Value: {car.Value}, Country: {car.Country}");
                }
            }
        }

        private void AddToDatabase(Car_Model car)
        {
            string connectionString = "Server=DESKTOP-738P60L\\SQLEXPRESS;Database=MTK;Integrated Security=True;TrustServerCertificate=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Car_Model (Person, Email, Name, Model, Year, Motors, Value, Country) VALUES (@Person, @Email, @Name, @Model, @Year, @Motors, @Value, @Country)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Person", car.Person);
                    command.Parameters.AddWithValue("@Email", car.Email);
                    command.Parameters.AddWithValue("@Name", car.Name);
                    command.Parameters.AddWithValue("@Model", car.Model);
                    command.Parameters.AddWithValue("@Year", car.Year);
                    command.Parameters.AddWithValue("@Motors", car.Motors);
                    command.Parameters.AddWithValue("@Value", car.Value);
                    command.Parameters.AddWithValue("@Country", car.Country);
                    command.ExecuteNonQuery();
                }
            }
            Console.WriteLine("Car added to database successfully!");
        }

        private bool ValidateEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, pattern);
        }

        private string GenerateName(string name)
        {
            if (maleNamesAZ.Contains(name) || maleNamesEN.Contains(name))
            {
                return "Mr. " + name;
            }
            else if (femaleNamesAZ.Contains(name) || femaleNamesEN.Contains(name))
            {
                return "Ms. " + name;
            }
            else
            {
                return "Invalid name.";
            }
        }

        private string GeneratePurchaseMessage(string name)
        {
            if (maleNamesAZ.Contains(name) || maleNamesEN.Contains(name))
            {
                return $"Mr. {name}, your car has been successfully purchased!";
            }
            else if (femaleNamesAZ.Contains(name) || femaleNamesEN.Contains(name))
            {
                return $"Ms. {name}, your car has been successfully purchased!";
            }
            else
            {
                return "Invalid name.";
            }
        }
    }
}