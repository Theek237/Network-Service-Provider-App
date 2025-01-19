using System;
using System.Linq;
using NetworkServiceProvider.Utils;

namespace NetworkServiceProvider.UI
{
    public class PerformanceAnalysisUI
    {
        public void ShowPerformanceAnalysis()
        {
            Console.Clear();
            Console.WriteLine("Performance Analysis");

            // Sample data for testing
            int[] testData = Enumerable.Range(1, 10000)
                .Select(_ => new Random().Next(1, 10000))
                .ToArray();

            var results = PerformanceAnalyzer.CompareSortingAlgorithms(testData);

            foreach (var result in results)
            {
                Console.WriteLine($"Algorithm: {result.AlgorithmName}");
                Console.WriteLine($"Execution Time: {result.ExecutionTime.TotalMilliseconds} ms");
                Console.WriteLine($"Memory Used: {result.MemoryUsed} bytes");
                Console.WriteLine();
            }

            Console.WriteLine("Press any key to return to the main menu...");
            Console.ReadKey();
        }
    }
}