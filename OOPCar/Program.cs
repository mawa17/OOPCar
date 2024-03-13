using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPCar
{
    internal class Program
    {
        private const string windowTitle = "Bil forhandler";
        private static readonly int windowWidth = Console.WindowWidth;
        private static readonly int windowHalf = Console.WindowWidth / 2;
        private static string[] options = { "Intast biloplysninger", "Ændre biloplysninger", "Se biloplysninger" };

        static void Main(string[] args)
        {
            var CarDealers = new Dictionary<string, List<Car>>();



            while (true)
            {
                WriteLineOptions("Bil forhandler", options);
                ConsoleKey inputKey = Console.ReadKey(true).Key;
                ResetConsole();
                switch (inputKey)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1: {
                        var result = AskOptions(options[0], "Skriv sælgers navn", "Skriv bilens model", "Skriv bilnes topfart");
                        if (result is null || result.Length < 3)
                        {
                            Console.WriteLine("Kunne ikke tilføje bilen!");
                            break;
                        }
                        if(String.IsNullOrEmpty(result[0]))
                        {
                            Console.WriteLine("Kan ikke tilføje en sælger uden!");
                            break;
                        }  
                        if(String.IsNullOrEmpty(result[1]))
                        {
                            Console.WriteLine("Kan ikke tilføje en bil uden navn og model!");
                            break;
                        }        
                        if(!ushort.TryParse(result[2], out var val))
                        {
                            Console.WriteLine($"Den andgivede topfart må fra {ushort.MinValue} til {ushort.MaxValue} KPH!");
                            break;
                        }
                        CarDealers[result[0]].Add(new Car(result[1], val));
                    } break;

                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2: {

                        } break;

                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3: {

                        } break;
                    default: {
                        } break;
                }
            }



        }

        private static void ResetConsole()
        {
            Console.SetCursorPosition(0, 0);
            Console.Clear();
        }
        private static void WriteLineOptions(string title, params string[] options)
        {
            if (options.Length == default || title == default) return;
            ResetConsole();
            WriteLineCenter(title);
            for (int i = 0; i < options.Length; i++)
            {
                Console.WriteLine($"{options[i]} (Tryk {i + 1})");
            }
            WriteLineFull();
        }

        private static string[] AskOptions(string title, params string[] questions)
        {
            if (options.Length == default || title == default) return null;
            var result = new string[questions.Length];
            ResetConsole();
            WriteLineCenter(title);
            for (int i = 0; i < questions.Length; i++)
            {
                Console.Write($"\r{questions[i]}: ");
                result[i] = Console.ReadLine().Trim();
            }
            return result;
        }

        private static void WriteLineCenter(string text) => Console.WriteLine(new string('=', (windowHalf-(text.Length/2))) + $"{text}" + new string('=', (windowHalf - (text.Length/2))-1));
        private static void WriteLineFull() => Console.WriteLine(new string('=', (windowWidth)-1));
    }
}
