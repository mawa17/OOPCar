using System;
using System.Collections.Generic;
using System.Linq;

namespace OOPCar
{
    internal class Program
    {
        private const string windowTitle = "Bil forhandler";
        private static readonly int windowWidth = Console.WindowWidth;
        private static readonly int windowHalf = Console.WindowWidth / 2;
        private static string[] options = { "Intast biloplysninger", "Ændre biloplysninger", "Se biloplysninger" };
        private static Dictionary<string, List<Car>> CarDealers = new Dictionary<string, List<Car>>();

        static void Main(string[] args)
        {
            bool isLooped = false;
            while (true)
            {
                if(isLooped) Console.ReadKey(true);
                isLooped = true;

                ResetConsole();
                PrintTitle(windowTitle);
                PrintOptions(options);
                PrintLine();
                ConsoleKey inputKey = Console.ReadKey(true).Key;
                ResetConsole();

                switch (inputKey)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1: {
                        PrintTitle($"{windowTitle} - {options[0]}");
                        var data = AskOptions("Skriv forhandleren navn", "Skriv bilens model", "Skriv bilnes topfart");
                        PrintLine();

                        if (data is null || data.Length < 3)        { Console.WriteLine("Mangler data for at kunne tilføje bilen"); break; }
                        if (String.IsNullOrEmpty(data[0]))          { Console.WriteLine("Kan ikke tilføje en sælger uden!"); break; }
                        if (String.IsNullOrEmpty(data[1]))          { Console.WriteLine("Kan ikke tilføje en bil uden navn og model!"); break; }
                        if (!ushort.TryParse(data[2], out var val)) { Console.WriteLine($"Den andgivede topfart må fra {ushort.MinValue} til {ushort.MaxValue} KPH!"); break; }

                        InitDealer(data[0]);
                        AddCar(data[0], new Car(data[1], val));
                        Console.WriteLine($"Bilen: {data[1]}\r\nHastihed: {val}\r\nEr nu tilføjet under forhandleren: {data[0]}");
                    } break;

                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2: {
                            PrintTitle($"{windowTitle} - {options[1]}");
                            PrintOptions("Ændre forhandleren", "Ændre modellen", "Ændre hastigheden");
                            ConsoleKey optionKey = Console.ReadKey(true).Key;
                            var data = AskOptions("Skriv forhandleren", "Skriv modellen");
                            PrintLine();
                            if(data is null || data.Length < 2) { Console.WriteLine("Mangler data for at kunne udføre handlingen"); break; }
                            if(!CarDealers.ContainsKey(data[0])) { Console.WriteLine($"Forhandleren: {data[0]} findes ikke"); break; }
                            if(!CarDealers[data[0]].Any(c => c.Model.Contains(data[1]))) { Console.WriteLine($"Forhandleren: {data[0]} har igen model: {data[1]} registreret"); break; }

                            switch (optionKey)
                            {
                                case ConsoleKey.D1:
                                case ConsoleKey.NumPad1: {
                                    var temp = AskOptions("Skift forhandler til");
                                    if (temp is null || temp.Length < 1){ Console.WriteLine("Mangler data for at kunne ændre forhandler"); break; }
                                    if(!TransferCar(data[0], temp[0], GetCar(data[0], data[1]))) { Console.WriteLine($"Kunne ikke overføre bil model: {data[1]} fra forhandler: {data[0]} til {temp[0]}"); break; }
                                    Console.WriteLine($"Bil model overført model: {data[1]} fra forhandler: {data[0]} til {temp[0]}");
                                } break;
                                case ConsoleKey.D2:
                                case ConsoleKey.NumPad2: {

                                } break;
                                case ConsoleKey.D3:
                                case ConsoleKey.NumPad3: {

                                } break;
                                default: Console.WriteLine("Den indtastede mulighed er ikke gyldig"); break;
                            }
                        } break;

                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3: {
                        PrintTitle($"{windowTitle} - {options[2]}");

                    } break;
                    default: Console.WriteLine("Den indtastede mulighed er ikke gyldig"); break;
                }
            }



        }
        #region Car Logic
        private static void InitDealer(string dealer) => CarDealers[dealer] = !CarDealers.ContainsKey(dealer) ? new List<Car>() : CarDealers[dealer];
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
            if (options is null || options.Length == 0) return;
            for (int i = 0; i < options.Length; i++)
            {
                Console.WriteLine($"{options[i]} (Tryk {i + 1})");
            }
        }
        private static string[] AskOptions(params string[] questions)
        {
            if (questions is null || questions.Length == 0) return null;
            var data = new string[questions.Length];
            for (int i = 0; i < questions.Length; i++)
            {
                Console.Write($"\r{questions[i]}: ");
                data[i] = Console.ReadLine().ToLower().Trim();
            }
            return data;
        }
        private static void PrintTitle(string text) => Console.WriteLine(new string('=', (windowHalf-(text.Length/2))) + $"{text}" + new string('=', (windowHalf - (text.Length/2))-1));
        private static void PrintLine() => Console.WriteLine(new string('=', (windowWidth)-1));
        #endregion
    }
}