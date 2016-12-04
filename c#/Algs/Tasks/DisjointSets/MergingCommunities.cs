using System;
using Algs.TestUtilities;

namespace Algs.Tasks.DisjointSets
{
    public static class MergingCommunities
    {
        public static void TaskMain()
        {
            var line0 = Input.ReadInts();
            var n = line0[0];
            var q = line0[1];
            var disjointSet = new DisjointSet(n);
            for (var i = 0; i < q; i++)
            {
                var query = Console.ReadLine().Split(' ');
                if (query[0] == "M")
                    disjointSet.Merge(int.Parse(query[1]) - 1, int.Parse(query[2]) - 1);
                else if (query[0] == "Q")
                    Console.WriteLine(disjointSet.GetSize(int.Parse(query[1]) - 1));
                else
                {
                    const string messageFormat = "unexpected command [{0}]";
                    throw new InvalidOperationException(string.Format(messageFormat, query[0]));
                }
            }
        }

        private class DisjointSet
        {
            private readonly int[] parent;
            private readonly int[] rank;
            private readonly int[] size;

            public DisjointSet(int count)
            {
                parent = new int[count];
                rank = new int[count];
                size = new int[count];
                for (var i = 0; i < count; i++)
                {
                    parent[i] = i;
                    rank[i] = 0;
                    size[i] = 1;
                }
            }

            public void Merge(int i, int j)
            {
                var r1 = GetRoot(i);
                var r2 = GetRoot(j);
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

            public int GetSize(int i)
            {
                return size[GetRoot(i)];
            }

            private int GetRoot(int i)
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