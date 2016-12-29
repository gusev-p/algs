using System;
using System.Collections.Generic;
using Algs.Core;

namespace Algs.Tasks.GraphAlg
{
    public static class DijkstraShortestPath
    {
        private const int verticiesCount = 200;
        private const int infiniteWeight = 1000000;
        private static readonly int[] targetVerticies = {7, 37, 59, 82, 99, 115, 133, 165, 188, 197};

        public static void TaskMain()
        {
            var outgoing = new List<Edge>[verticiesCount];
            for (var i = 0; i < outgoing.Length; i++)
                outgoing[i] = new List<Edge>();
            for (var i = 0; i < outgoing.Length; i++)
            {
                var line = Console.ReadLine().Split(new[] {'\t'}, StringSplitOptions.RemoveEmptyEntries);
                for (var j = 1; j < line.Length; j++)
                {
                    var forwardEdge = Edge.Parse(line[j]);
                    outgoing[i].Add(forwardEdge);
                    outgoing[forwardEdge.v].Add(new Edge
                    {
                        v = i,
                        weight = forwardEdge.weight
                    });
                }
            }
            var distances = DijkstraFrom(outgoing, 0);
            var result = new int[targetVerticies.Length];
            for (var i = 0; i < result.Length; i++)
                result[i] = distances[targetVerticies[i] - 1];
            Console.WriteLine(string.Join(",", result));
        }

        private static int[] DijkstraFrom(List<Edge>[] outgoing, int start)
        {
            var distances = new int[outgoing.Length];
            var heapItems = new HeapItem[outgoing.Length];
            for (var i = 0; i < outgoing.Length; i++)
            {
                distances[i] = i == start ? 0 : infiniteWeight;
                heapItems[i] = new HeapItem
                {
                    v = i,
                    key = int.MaxValue
                };
            }
            var startAdjacent = outgoing[start];
            foreach (var t in startAdjacent)
                heapItems[t.v].key = t.weight;
            var heap = PriorityQueue<HeapItem>.Create(heapItems,
                delegate(HeapItem i1, HeapItem i2)
                {
                    if (i1.key < i2.key)
                        return MostPriority.First;
                    if (i1.key > i2.key)
                        return MostPriority.Second;
                    return MostPriority.Both;
                },
                (n, handle) => n.handle = handle);
            while (heap.Count > 0)
            {
                var minItem = heap.ExtractTop();
                if (minItem.key == int.MaxValue)
                    break;
                distances[minItem.v] = minItem.key;
                var adjacent = outgoing[minItem.v];
                foreach (var edge in adjacent)
                {
                    if (distances[edge.v] != infiniteWeight)
                        continue;
                    var newKey = minItem.key + edge.weight;
                    if (newKey < heapItems[edge.v].key)
                    {
                        heapItems[edge.v].key = newKey;
                        heap.HeapifyUp(heapItems[edge.v].handle);
                    }
                }
            }
            return distances;
        }

        private class HeapItem
        {
            public int key;
            public int v;
            public int handle;
        }

        private class Edge
        {
            public int v;
            public int weight;

            public static Edge Parse(string s)
            {
                var splitted = s.Split(',');
                return new Edge
                {
                    v = int.Parse(splitted[0]) - 1,
                    weight = int.Parse(splitted[1])
                };
            }
        }
    }
}