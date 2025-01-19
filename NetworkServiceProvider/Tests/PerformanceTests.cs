// Location: NetworkServiceProvider/Tests/PerformanceTests.cs

using System;
using System.Linq;
using NetworkServiceProvider.Utils;
using Xunit;

namespace NetworkServiceProvider.Tests
{
    public class PerformanceTests
    {
        [Fact]
        public void SortingAlgorithms_PerformanceComparison_ShouldReturnValidResults()
        {
            // Arrange
            int[] testData = Enumerable.Range(1, 10000)
                .Select(_ => new Random().Next(1, 10000))
                .ToArray();

            // Act
            var results = PerformanceAnalyzer.CompareSortingAlgorithms(testData);

            // Assert
            Assert.Equal(3, results.Count); // Should have results for all three sorting algorithms
            Assert.All(results, result => Assert.True(result.ExecutionTime.TotalMilliseconds > 0));
            Assert.All(results, result => Assert.True(result.MemoryUsed >= 0));
        }
    }
}