using System;
using System.Collections.Generic;

namespace NetworkServiceProvider.DataStructures
{
    public class Graph<T>
    {
        private Dictionary<T, List<(T Destination, double Weight)>> adjacencyList;

        public Graph()
        {
            adjacencyList = new Dictionary<T, List<(T, double)>>();
        }

        public void AddVertex(T vertex)
        {
            if (!adjacencyList.ContainsKey(vertex))
            {
                adjacencyList[vertex] = new List<(T, double)>();
            }
        }

        public void AddEdge(T source, T destination, double weight = 1.0)
        {
            if (!adjacencyList.ContainsKey(source))
                AddVertex(source);
            if (!adjacencyList.ContainsKey(destination))
                AddVertex(destination);

            adjacencyList[source].Add((destination, weight));
            // For undirected graph, add the reverse edge
            adjacencyList[destination].Add((source, weight));
        }

        public Dictionary<T, double> Dijkstra(T startVertex)
        {
            var distances = new Dictionary<T, double>();
            var pq = new PriorityQueue<(T Vertex, double Distance)>();
            var visited = new HashSet<T>();

            foreach (var vertex in adjacencyList.Keys)
            {
                distances[vertex] = double.MaxValue;
            }
            distances[startVertex] = 0;
            pq.Enqueue((startVertex, 0));

            while (pq.Count > 0)
            {
                var current = pq.Dequeue();
                T currentVertex = current.Vertex;

                if (visited.Contains(currentVertex))
                    continue;

                visited.Add(currentVertex);

                foreach (var neighbor in adjacencyList[currentVertex])
                {
                    if (visited.Contains(neighbor.Destination))
                        continue;

                    double newDistance = distances[currentVertex] + neighbor.Weight;
                    if (newDistance < distances[neighbor.Destination])
                    {
                        distances[neighbor.Destination] = newDistance;
                        pq.Enqueue((neighbor.Destination, newDistance));
                    }
                }
            }

            return distances;
        }

        public List<T> GetNeighbors(T vertex)
        {
            if (!adjacencyList.ContainsKey(vertex))
                return new List<T>();

            var neighbors = new List<T>();
            foreach (var neighbor in adjacencyList[vertex])
            {
                neighbors.Add(neighbor.Destination);
            }
            return neighbors;
        }
    }
}