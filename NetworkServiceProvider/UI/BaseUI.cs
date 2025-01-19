using System;

namespace NetworkServiceProvider.UI
{
    public abstract class BaseUI
    {
        protected static void DisplayHeader(string title)
        {
            Console.Clear();
            Console.WriteLine($"=== {title} ===");
            Console.WriteLine($"Current Time (UTC): 2025-01-19 15:09:42");
            Console.WriteLine($"User: Theek237");
            Console.WriteLine(new string('=', title.Length + 8));
            Console.WriteLine();
        }

        protected static string GetUserInput(string prompt)
        {
            Console.Write($"{prompt}: ");
            return Console.ReadLine();
        }

        protected static void DisplayError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Error: {message}");
            Console.ResetColor();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        protected static void DisplaySuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Success: {message}");
            Console.ResetColor();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}