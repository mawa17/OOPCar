using System;
using System.Collections.Generic;
using System.Linq;

namespace OOPCar
{
    internal class Program
    {
        private const string windowTitle = "Bil forhandler";
        private static readonly int windowWidth = Console.WindowWidth - 1;
        private static readonly int windowHalf = (Console.WindowWidth / 2);
        private static string[] options = { "Intast biloplysninger", "Ændre biloplysninger", "Se biloplysninger" };
        private static Dictionary<string, List<Car>> CarDealers = new Dictionary<string, List<Car>>();
        static void Main(string[] args)
        {
            while (true)
            {
                PrintTitle(windowTitle);
                PrintOptions(options);
                PrintLine();

                ConsoleKey inputKey = Console.ReadKey(true).Key;
                switch (inputKey)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1: RegisterCar(); break;

                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2: ChangeCarInfo(); break;

                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3: ViewCarInfo(); break;
                }
            }
        }
        #region Main Logic
        public static void RegisterCar()
        {
            PrintTitle($"{windowTitle} {options[0]}");
            var data = AskOptions("Skriv forhandleren navn", "Skriv bilens model", "Skriv bilnes topfart");
            PrintLine();

            if (data is null || data.Length < 3) { Console.WriteLine("Mangler data for at kunne tilføje bilen"); return; }
            if (String.IsNullOrEmpty(data[0])) { Console.WriteLine("Kan ikke tilføje en sælger uden!"); return; }
            if (String.IsNullOrEmpty(data[1])) { Console.WriteLine("Kan ikke tilføje en bil uden navn og model!"); return; }
            if (!ushort.TryParse(data[2], out var val)) { Console.WriteLine($"Den andgivede topfart må fra {ushort.MinValue} til {ushort.MaxValue} KPH!"); return; }

            InitDealer(data[0]);
            AddCar(data[0], new Car(data[1], val));
            Console.WriteLine($"Bilen: {data[1]}");
            Console.WriteLine($"Med hastiheden: {val}");
            Console.WriteLine($"Er nu tilføjet under forhandleren: {data[0]}");
        }
        public static void ChangeCarInfo()
        {
            PrintTitle($"{windowTitle} - {options[1]}");
            PrintOptions("Ændre forhandleren", "Ændre modellen", "Ændre hastigheden");
            PrintLine();

            ConsoleKey optionKey = Console.ReadKey(true).Key;
            if (optionKey == ConsoleKey.Escape || optionKey == ConsoleKey.Backspace) { return; }

            var data = AskOptions("Skriv forhandleren", "Skriv modellen");
            if (data is null || data.Length < 2) { Console.WriteLine("Mangler data for at kunne udføre handlingen"); return; }
            InitDealer(data[0]);
            var car = GetCar(data[0], data[1]);
            if (car is null) { Console.WriteLine($"Forhandleren: {data[0]} har igen model: {data[1]} registreret"); return; }

            PrintTitle($"{windowTitle} - Ændre forhandleren");
            switch (optionKey)
            {
                case ConsoleKey.D1:
                case ConsoleKey.NumPad1:
                    {
                        var temp = AskOptions("Skift forhandler til");
                        if (temp is null || temp.Length < 1) { Console.WriteLine("Mangler data for at kunne ændre forhandler"); return; }
                        if (!TransferCar(data[0], temp[0], car)) { Console.WriteLine($"Kunne ikke overføre bil model: {data[1]} fra forhandler: {data[0]} til {temp[0]}"); break; }
                        Console.WriteLine($"Bil model overført model: {data[1]} fra forhandler: {data[0]} til {temp[0]}");
                    }
                    break;
                case ConsoleKey.D2:
                case ConsoleKey.NumPad2:
                    {
                        var temp = AskOptions("Skift modellen til");
                        if (temp is null || temp.Length < 1) { Console.WriteLine("Mangler data for at kunne ændre modellen"); return; }
                        if (!RemoveCar(data[0], car)) { Console.WriteLine($"Modellen {car.Model} er ikke registreret af forhandleren: {data[0]}"); return; }
                        AddCar(data[0], car.UpdateData(model: temp[0]));
                        Console.WriteLine($"Bil model ændret fra: {data[1]} til {temp[0]}");
                    }
                    break;
                case ConsoleKey.D3:
                case ConsoleKey.NumPad3:
                    {

                    }
                    break;
                default:  break;
            }
        }
        public static void ViewCarInfo()
        {

        }
        #endregion

        #region Car Logic
        private static void InitDealer(string dealer)
        {
            if (CarDealers.ContainsKey(dealer)) { return; }
            CarDealers[dealer] = new List<Car>();
        }
        private static void AddCar(string dealer, Car car)
        {
            InitDealer(dealer);
            CarDealers[dealer].Add(car);
        }
        private static bool RemoveCar(string dealer, Car car)
        {
            InitDealer(dealer);
            return CarDealers[dealer].RemoveAll(c => c.Model.Contains(car.Model)) > 0;
        }
        private static bool TransferCar(string fromDealer, string toDealer, Car car)
        {
            InitDealer(fromDealer);
            InitDealer(toDealer);
            if (!RemoveCar(fromDealer, car)) { return false; }
            AddCar(toDealer, car);
            return true;
        }
        private static Car GetCar(string dealer, string model)
        {
            InitDealer(dealer);
            if (!CarDealers[dealer].Any(c => c.Model.Contains(model))) return null;
            return CarDealers[dealer].FirstOrDefault(c => c.Model.Contains(model));
        }
        #endregion

        #region Helpers
        private static void ResetConsole()
        {
            Console.SetCursorPosition(0, 0);
            Console.Clear();
        }
        private static void PrintOptions(params string[] options)
        {
            if(options != null && options.Length > 0)
            {
                for (int i = 0; i < options.Length; i++)
                {
                    Console.WriteLine($"{options[i]} (Tryk {i + 1})");
                }
            }
        }
        private static string[] AskOptions(params string[] questions)
        {
            if(questions is null || questions.Length < 1) { return null; }
            var data = new string[questions.Length];
            for (int i = 0; i < questions.Length; i++)
            {
                Console.Write($"{questions[i]}: ");
                var input = Console.ReadLine().Trim();
                if (String.IsNullOrWhiteSpace(input)) { return null; }
                data[i] = input;
            }
            return data;
   
        }
        private static void PrintTitle(string text)
        {
            ResetConsole();
            int len = windowHalf - (text.Length / 2);
            Console.WriteLine((new string('=', len) + text + new string('=', len)).Substring(0, windowWidth));
        }
        private static void PrintLine() => Console.WriteLine((new string('=', windowWidth)).Substring(0, windowWidth));
        #endregion
    }
}