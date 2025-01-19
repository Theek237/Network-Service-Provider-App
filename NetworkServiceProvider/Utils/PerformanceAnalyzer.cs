using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NetworkServiceProvider.Algorithms;

namespace NetworkServiceProvider.Utils
{
    public class PerformanceAnalyzer
    {
        public class PerformanceResult
        {
            public string AlgorithmName { get; set; }
            public TimeSpan ExecutionTime { get; set; }
            public long MemoryUsed { get; set; }
        }

        public static List<PerformanceResult> CompareSortingAlgorithms<T>(T[] data) where T : IComparable<T>
        {
            var results = new List<PerformanceResult>();
            var algorithms = new SortingAlgorithms<T>();

            // Test Bubble Sort
            var bubbleSortData = (T[])data.Clone();
            GC.Collect();
            var memoryBefore = GC.GetTotalMemory(true);
            var bubbleSortTime = algorithms.BubbleSort(bubbleSortData);
            var memoryAfter = GC.GetTotalMemory(true);

            results.Add(new PerformanceResult
            {
                AlgorithmName = "Bubble Sort",
                ExecutionTime = bubbleSortTime,
                MemoryUsed = memoryAfter - memoryBefore
            });

            // Test Quick Sort
            var quickSortData = (T[])data.Clone();
            GC.Collect();
            memoryBefore = GC.GetTotalMemory(true);
            var quickSortTime = algorithms.QuickSort(quickSortData);
            memoryAfter = GC.GetTotalMemory(true);

            results.Add(new PerformanceResult
            {
                AlgorithmName = "Quick Sort",
                ExecutionTime = quickSortTime,
                MemoryUsed = memoryAfter - memoryBefore
            });

            // Test Merge Sort
            var mergeSortData = (T[])data.Clone();
            GC.Collect();
            memoryBefore = GC.GetTotalMemory(true);
            var mergeSortTime = algorithms.MergeSort(mergeSortData);
            memoryAfter = GC.GetTotalMemory(true);

            results.Add(new PerformanceResult
            {
                AlgorithmName = "Merge Sort",
                ExecutionTime = mergeSortTime,
                MemoryUsed = memoryAfter - memoryBefore
            });

            return results;
        }
    }
}