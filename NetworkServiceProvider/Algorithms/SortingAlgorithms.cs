using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NetworkServiceProvider.Algorithms
{
    public class SortingAlgorithms<T> where T : IComparable<T>
    {
        public TimeSpan BubbleSort(T[] arr)
        {
            var stopwatch = Stopwatch.StartNew();

            int n = arr.Length;
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    if (arr[j].CompareTo(arr[j + 1]) > 0)
                    {
                        // Swap
                        T temp = arr[j];
                        arr[j] = arr[j + 1];
                        arr[j + 1] = temp;
                    }
                }
            }

            stopwatch.Stop();
            return stopwatch.Elapsed;
        }

        public TimeSpan QuickSort(T[] arr)
        {
            var stopwatch = Stopwatch.StartNew();

            QuickSortRecursive(arr, 0, arr.Length - 1);

            stopwatch.Stop();
            return stopwatch.Elapsed;
        }

        private void QuickSortRecursive(T[] arr, int low, int high)
        {
            if (low < high)
            {
                int pivotIndex = Partition(arr, low, high);
                QuickSortRecursive(arr, low, pivotIndex - 1);
                QuickSortRecursive(arr, pivotIndex + 1, high);
            }
        }

        private int Partition(T[] arr, int low, int high)
        {
            T pivot = arr[high];
            int i = low - 1;

            for (int j = low; j < high; j++)
            {
                if (arr[j].CompareTo(pivot) <= 0)
                {
                    i++;
                    T temp = arr[i];
                    arr[i] = arr[j];
                    arr[j] = temp;
                }
            }

            T temp1 = arr[i + 1];
            arr[i + 1] = arr[high];
            arr[high] = temp1;

            return i + 1;
        }

        public TimeSpan MergeSort(T[] arr)
        {
            var stopwatch = Stopwatch.StartNew();

            MergeSortRecursive(arr, 0, arr.Length - 1);

            stopwatch.Stop();
            return stopwatch.Elapsed;
        }

        private void MergeSortRecursive(T[] arr, int left, int right)
        {
            if (left < right)
            {
                int middle = (left + right) / 2;
                MergeSortRecursive(arr, left, middle);
                MergeSortRecursive(arr, middle + 1, right);
                Merge(arr, left, middle, right);
            }
        }

        private void Merge(T[] arr, int left, int middle, int right)
        {
            int n1 = middle - left + 1;
            int n2 = right - middle;

            T[] leftArray = new T[n1];
            T[] rightArray = new T[n2];

            Array.Copy(arr, left, leftArray, 0, n1);
            Array.Copy(arr, middle + 1, rightArray, 0, n2);

            int i = 0, j = 0, k = left;

            while (i < n1 && j < n2)
            {
                if (leftArray[i].CompareTo(rightArray[j]) <= 0)
                {
                    arr[k] = leftArray[i];
                    i++;
                }
                else
                {
                    arr[k] = rightArray[j];
                    j++;
                }
                k++;
            }

            while (i < n1)
            {
                arr[k] = leftArray[i];
                i++;
                k++;
            }

            while (j < n2)
            {
                arr[k] = rightArray[j];
                j++;
                k++;
            }
        }
    }
}