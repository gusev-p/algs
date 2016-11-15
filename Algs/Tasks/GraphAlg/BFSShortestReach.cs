using System;
using System.Collections.Generic;

namespace Algs.Tasks.GraphAlg
{
    public static class BFSShortestReach
    {
        public static void TaskMain()
        {
            var queriesCount = Convert.ToInt32(Console.ReadLine());
            for (var i = 0; i < queriesCount; i++)
            {
                var line1 = Console.ReadLine().Split(' ');
                var nodesCount = int.Parse(line1[0]);
                var edgesCount = int.Parse(line1[1]);
                var graph = new Graph(nodesCount);
                for (var j = 0; j < edgesCount; j++)
                {
                    var line2 = Console.ReadLine().Split(' ');
                    var u = int.Parse(line2[0]);
                    var v = int.Parse(line2[1]);
                    graph.AddEdge(u - 1, v - 1);
                }
                var startingNode = int.Parse(Console.ReadLine());
                var distances = graph.CalculateDistancesFrom(startingNode - 1);
                var result = new List<int>();
                for (var n = 0; n < distances.Length; n++)
                {
                    if (n == startingNode - 1)
                        continue;
                    var d = distances[n];
                    result.Add(d == -1 ? -1 : d*6);
                }
                Console.Out.WriteLine(string.Join(" ", result));
            }
        }

        public class Graph
        {
            private readonly List<int>[] outgoing;

            public Graph(int nodesCount)
            {
                outgoing = new List<int>[nodesCount];
                for (var i = 0; i < outgoing.Length; i++)
                    outgoing[i] = new List<int>();
            }

            public void AddEdge(int u, int v)
            {
                outgoing[u].Add(v);
                outgoing[v].Add(u);
            }

            public int[] CalculateDistancesFrom(int s)
            {
                var queue = new Queue<int>();
                var distances = new int[outgoing.Length];
                for (var i = 0; i < distances.Length; i++)
                    distances[i] = -1;
                queue.Enqueue(s);
                var distance = 0;
                var frontSize = 1;
                while (queue.Count > 0)
                {
                    var currentNode = queue.Dequeue();
                    if (distances[currentNode] == -1)
                    {
                        distances[currentNode] = distance;
                        var neighbours = outgoing[currentNode];
                        foreach (var n in neighbours)
                            if (distances[n] == -1)
                                queue.Enqueue(n);
                    }
                    if (--frontSize == 0)
                    {
                        frontSize = queue.Count;
                        distance++;
                    }
                }
                return distances;
            }
        }
    }
}