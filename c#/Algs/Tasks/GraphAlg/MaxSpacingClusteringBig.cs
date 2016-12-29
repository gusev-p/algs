using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Algs.TestUtilities;

namespace Algs.Tasks.GraphAlg
{
    public static class MaxSpacingClusteringBig
    {
        public static void TaskMain()
        {
            var line0 = Input.ReadInts();
            var maxPointsCount = line0[0];
            var bitsCount = line0[1];
            var points = new List<int>();
            var pointToIndexMap = new Dictionary<int, int>();
            for (var i = 0; i < maxPointsCount; i++)
            {
                var bitsString = Console.ReadLine();
                var point = ParseBits(bitsString);
                if (pointToIndexMap.ContainsKey(point))
                    continue;
                pointToIndexMap.Add(point, points.Count);
                points.Add(point);
            }
            var unionFind = new UnionFind(points.Count);
            for (var i = 0; i < points.Count; i++)
            {
                var point = points[i];
                for (var j = 0; j < bitsCount; j++)
                {
                    var otherPoint = SwitchBit(point, j);
                    int otherIndex;
                    if (pointToIndexMap.TryGetValue(otherPoint, out otherIndex))
                        unionFind.Union(i, otherIndex);
                }
            }
            for (var i = 0; i < points.Count; i++)
            {
                var point = points[i];
                for (var j = 0; j < bitsCount; j++)
                {
                    var p = SwitchBit(point, j);
                    for (var l = j + 1; l < bitsCount; l++)
                    {
                        var otherPoint = SwitchBit(p, l);
                        int otherIndex;
                        if (pointToIndexMap.TryGetValue(otherPoint, out otherIndex))
                            unionFind.Union(i, otherIndex);
                    }
                }
            }
            Console.WriteLine(unionFind.Count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int SwitchBit(int number, int bit)
        {
            return number ^ (1 << bit);
        }

        private static int ParseBits(string bits)
        {
            var result = 0;
            for (var i = 0; i < bits.Length; i += 2)
            {
                result <<= 1;
                if (bits[i] == '1')
                    result |= 1;
            }
            return result;
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

            private int Find(int i)
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