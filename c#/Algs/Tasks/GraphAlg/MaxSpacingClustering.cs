using System;
using System.Collections.Generic;
using Algs.TestUtilities;

namespace Algs.Tasks.GraphAlg
{
    public static class MaxSpacingClustering
    {
        public static void TaskMain()
        {
            var nodesCount = Input.ReadInt();
            var distances = new List<Distance>();
            for (var i = 0; i < nodesCount; i++)
                for (var j = i + 1; j < nodesCount; j++)
                {
                    var ints = Input.ReadInts();
                    if (ints[0] != i + 1)
                        throw new InvalidOperationException("unexpected node");
                    if (ints[1] != j + 1)
                        throw new InvalidOperationException("unexpected node");
                    distances.Add(new Distance
                    {
                        value = ints[2],
                        node1 = i,
                        node2 = j
                    });
                }
            distances.Sort((d1, d2) => d1.value.CompareTo(d2.value));
            var clusters = new UnionFind(nodesCount);
            var lastDistanceIndex = 0;
            while (true)
            {
                var d = distances[lastDistanceIndex++];
                if (clusters.Find(d.node1) == clusters.Find(d.node2))
                    continue;
                if (clusters.Count == 4)
                    break;
                clusters.Union(d.node1, d.node2);
            }

            Console.WriteLine(distances[lastDistanceIndex].value);
        }

        private class Distance
        {
            public int node1;
            public int node2;
            public int value;
        }

        private class UnionFind
        {
            private readonly int[] parent;
            private readonly int[] rank;

            public UnionFind(int count)
            {
                parent = new int[count];
                rank = new int[count];
                for (var i = 0; i < count; i++)
                {
                    parent[i] = i;
                    rank[i] = 0;
                }
                Count = count;
            }

            public int Count { get; private set; }

            public void Union(int i, int j)
            {
                var r1 = Find(i);
                var r2 = Find(j);
                if (r1 == r2)
                    return;
                if (rank[r1] < rank[r2])
                    parent[r1] = r2;
                else if (rank[r2] < rank[r1])
                    parent[r2] = r1;
                else
                {
                    parent[r2] = r1;
                    rank[r1]++;
                }
                Count--;
            }

            public int Find(int i)
            {
                var root = i;
                while (parent[root] != root)
                    root = parent[root];
                while (parent[i] != root)
                {
                    var next = parent[i];
                    parent[i] = root;
                    i = next;
                }
                return root;
            }
        }
    }
}