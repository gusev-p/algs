using System;
using System.Collections.Generic;
using System.Linq;
using Algs.TestUtilities;

namespace Algs.Tasks.DisjointSets
{
    public static class KunduAndTree
    {
        public static void TaskMain()
        {
            var n = Input.ReadInt();
            var unionFind = new UnionFind(n);
            var redEdges = new List<Edge>();
            for (var i = 0; i < n - 1; i++)
            {
                var line = Console.ReadLine().Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries);
                var v1 = int.Parse(line[0]) - 1;
                var v2 = int.Parse(line[1]) - 1;
                if (line[2] == "b")
                    unionFind.Union(v1, v2);
                else
                    redEdges.Add(new Edge
                    {
                        v1 = v1,
                        v2 = v2
                    });
            }
            var outgoing = new Dictionary<int, List<int>>();
            foreach (var e in redEdges)
            {
                var v1 = unionFind.Find(e.v1);
                var v2 = unionFind.Find(e.v2);
                Include(outgoing, v1, v2);
                Include(outgoing, v2, v1);
            }
            ulong c1 = 0;
            ulong c2 = 0;
            ulong c3 = 0;
            var processed = new HashSet<int>();
            var queue = new Queue<int>();
            queue.Enqueue(outgoing.First().Key);
            processed.Add(outgoing.First().Key);
            while (queue.Count > 0)
            {
                var v = queue.Dequeue();
                var size = unionFind.GetSize(v);
                var newC1 = c1 + size;
                var newC2 = c1*size + c2;
                var newC3 = c2*size + c3;
                c1 = newC1;
                c2 = newC2;
                c3 = newC3;
                foreach (var u in outgoing[v])
                    if (processed.Add(u))
                        queue.Enqueue(u);
            }
            var result = c3%((ulong) Math.Pow(10, 9) + 7);
            Console.WriteLine(result);
        }

        private class Edge
        {
            public int v1;
            public int v2;
        }

        private static void Include(Dictionary<int, List<int>> dictionary, int v1, int v2)
        {
            List<int> adjucent;
            if (!dictionary.TryGetValue(v1, out adjucent))
                dictionary.Add(v1, adjucent = new List<int>(1));
            adjucent.Add(v2);
        }

        private class UnionFind
        {
            private readonly int[] parent;
            private readonly int[] rank;
            private readonly uint[] size;

            public UnionFind(int count)
            {
                parent = new int[count];
                rank = new int[count];
                size = new uint[count];
                for (var i = 0; i < count; i++)
                {
                    parent[i] = i;
                    rank[i] = 0;
                    size[i] = 1;
                }
            }

            public uint GetSize(int i)
            {
                return size[i];
            }

            public void Union(int i, int j)
            {
                var r1 = Find(i);
                var r2 = Find(j);
                if (r1 == r2)
                    return;
                if (rank[r1] < rank[r2])
                {
                    parent[r1] = r2;
                    size[r2] += size[r1];
                }
                else if (rank[r2] < rank[r1])
                {
                    parent[r2] = r1;
                    size[r1] += size[r2];
                }
                else
                {
                    parent[r2] = r1;
                    rank[r1]++;
                    size[r1] += size[r2];
                }
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