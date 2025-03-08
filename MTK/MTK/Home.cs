using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Text.RegularExpressions;

namespace MTK
{
    public class Home
    {
        public string Person { get; set; }
        public string Email { get; set; }
        public string Adress { get; set; }
        public int Room { get; set; }
        public string Model { get; set; }
        public decimal Value { get; set; }

        private List<string> maleNamesAZ = new List<string>
        {
            "Ali", "Veli", "Cemil", "Elvin", "Ruslan", "Tural", "Samir", "Orxan", "Elnur", "Ramin",
            "Nurlan", "Farid", "Anar", "Rövşen", "Elşən", "İlqar", "Vüqar", "Şahin", "Reşad", "Natiq",
            "Emin", "Rehman", "Teymur", "Rəşid", "Fərid", "Nizami", "Şamil", "Rüstem", "Vidadi", "Zaur",
            "Elbrus", "Rövşən", "Nadir", "Elşad", "Receb", "Sərvər", "Revan", "Terlan", "Eldar", "Reşat",
            "Nurlan", "Faiq", "Reşid", "Elmar", "Reşad", "Natiq", "Emin", "Rehman", "Teymur", "Reşid","Murad"
        };

        private List<string> femaleNamesAZ = new List<string>
        {
            "Aysel", "Gülşen", "Leyla", "Nergiz", "Sevda", "Aytac", "Günel", "Narmin", "Sevil", "Zümrüd",
            "Lala", "Nazlı", "Səbinə", "Türkan", "Zərifə", "Aynur", "Gülay", "Nermin", "Səadət", "Zehra",
            "Leyla", "Nigar", "Səbinə", "Türkan", "Zeyneb", "Aynurə", "Gülnar", "Nermine", "Sevda", "Zerengül",
            "Leyli", "Narmina", "Səbinə", "Türkan", "Zenfira", "Aytac", "Gül", "Nezrin", "Seide", "Gülgez",
            "Elnure", "Nergiz", "Səbinə", "Türkan", "Zernişan", "Aysu", "Gülnare", "Narin", "Seadet", "Mehbube","Fatime"
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

        private List<Home> HomesList = new List<Home>();

        public void Homes()
        {
            Console.WriteLine("\n--- Home ---");

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

            FillHomeData();

            Console.WriteLine("Enter Home model which you want to get (Kohne Tikili, Yeni Bina): ");
            Model = Console.ReadLine();

            
            var availableHomes = HomesList.FindAll(h => h.Model.Equals(Model, StringComparison.OrdinalIgnoreCase));
            if (availableHomes.Count == 0)
            {
                Console.WriteLine("No homes available for the selected model.");
                return;
            }

            Console.WriteLine($"Available homes for {Model}:");
            for (int i = 0; i < availableHomes.Count; i++)
            {
                Console.WriteLine($"{i + 1}. Address: {availableHomes[i].Adress}, Rooms: {availableHomes[i].Room}, Value: {availableHomes[i].Value}");
            }

            Console.WriteLine("Enter the number of the home you want to buy: ");
            if (int.TryParse(Console.ReadLine(), out int selectedIndex) && selectedIndex > 0 && selectedIndex <= availableHomes.Count)
            {
                var selectedHome = availableHomes[selectedIndex - 1];
                selectedHome.Person = Person;
                selectedHome.Email = Email;

                // Cinsiyete uygun mesaj ver
                string message = GeneratePurchaseMessage(personName);
                Console.WriteLine(message);

                // Veritabanına ekle
                AddToDatabase(selectedHome);
            }
            else
            {
                Console.WriteLine("Invalid selection.");
            }
        }

        private void FillHomeData()
        {
            HomesList.Add(new Home { Adress = "Baku, Sovetski 1", Room = 3, Model = "Kohne Tikili", Value = 50000 });
            HomesList.Add(new Home { Adress = "Baku, Sovetski 2", Room = 2, Model = "Kohne Tikili", Value = 40000 });
            HomesList.Add(new Home { Adress = "Baku, Yasamal 1", Room = 4, Model = "Yeni Bina", Value = 100000 });
            HomesList.Add(new Home { Adress = "Baku, Yasamal 2", Room = 3, Model = "Yeni Bina", Value = 90000 });
        }

        private void AddToDatabase(Home home)
        {
            string connectionString = "Server=Name;Database=Table;Integrated Security=True;TrustServerCertificate=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Home (Person, Email, Adress, Room, Model, Value) VALUES (@Person, @Email, @Adress, @Room, @Model, @Value)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Person", home.Person);
                    command.Parameters.AddWithValue("@Email", home.Email);
                    command.Parameters.AddWithValue("@Adress", home.Adress);
                    command.Parameters.AddWithValue("@Room", home.Room);
                    command.Parameters.AddWithValue("@Model", home.Model);
                    command.Parameters.AddWithValue("@Value", home.Value);
                    command.ExecuteNonQuery();
                }
            }
            Console.WriteLine("Home added to database successfully!");
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
                return $"Mr. {name}, your home has been successfully purchased!";
            }
            else if (femaleNamesAZ.Contains(name) || femaleNamesEN.Contains(name))
            {
                return $"Ms. {name}, your home has been successfully purchased!";
            }
            else
            {
                return "Invalid name.";
            }
        }
    }
}
