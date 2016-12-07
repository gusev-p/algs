using System;
using System.Collections.Generic;
using Algs.TestUtilities;

namespace Algs.Tasks.DisjointSets
{
    public static class SuperMaximumCostQueries
    {
        public static void TaskMain()
        {
            var line0 = Input.ReadInts();
            var n = line0[0];
            var q = line0[1];
            var edges = new Edge[n - 1];
            for (var i = 0; i < n - 1; i++)
            {
                var line = Input.ReadInts();
                edges[i] = new Edge
                {
                    v1 = line[0] - 1,
                    v2 = line[1] - 1,
                    weight = line[2]
                };
            }
            var searcher = new TreePathsSearcher(edges);
            for (var _ = 0; _ < q; _++)
            {
                var line = Input.ReadInts();
                var l = line[0];
                var r = line[1];
                var pathCount = searcher.CountPathsWithCostBetween(l, r);
                Console.WriteLine(pathCount);
            }
        }

        private class TreePathsSearcher
        {
            private readonly int[] weights;
            private readonly List<int>[] outgoing;
            private readonly Edge[] edges;

            public TreePathsSearcher(Edge[] edges)
            {
                this.edges = edges;
                outgoing = new List<int>[edges.Length + 1];
                for (var i = 0; i < outgoing.Length; i++)
                    outgoing[i] = new List<int>();
                Array.Sort(edges, (e1, e2) => e1.weight.CompareTo(e2.weight));
                for (var i = 0; i < edges.Length; i++)
                {
                    var edge = edges[i];
                    outgoing[edge.v1].Add(i);
                    outgoing[edge.v2].Add(i);
                }
                weights = new int[edges.Length];
                for (var i = 0; i < edges.Length; i++)
                    weights[i] = edges[i].weight;
            }

            public ulong CountPathsWithCostBetween(int l, int r)
            {
                var redBlackTree = BuildRedBlackTree(l, r);
                return redBlackTree == null
                    ? 0
                    : CalculatePathsCount(redBlackTree, l);
            }

            private ulong CalculatePathsCount(RedBlackTree redBlackTree, int l)
            {
                ulong result = 0;
                var stack = new Stack<UFNode>();
                var processed = new HashSet<UFNode>();
                foreach (var root in redBlackTree.roots)
                {
                    if (!processed.Add(root))
                        continue;
                    ulong processedSize = root.size;
                    ulong treeCount = (root.size + 1)*root.size/2;
                    stack.Push(root);
                    while (stack.Count > 0)
                    {
                        var e = stack.Pop();
                        Dictionary<UFNode, uint> adjacent;
                        if (!redBlackTree.outgoing.TryGetValue(e, out adjacent))
                            continue;
                        foreach (var x in adjacent)
                        {
                            if (!processed.Add(x.Key))
                                continue;
                            stack.Push(x.Key);
                            var xIsRed = weights[x.Key.edge] >= l;
                            var size = x.Key.size;
                            result += processedSize*size*x.Value;
                            if (xIsRed)
                                result += (size + 1)*size/2;
                            processedSize += size;
                        }
                    }
                    result += treeCount;
                }
                return result;
            }

            private RedBlackTree BuildRedBlackTree(int l, int r)
            {
                var first = FindFirstGreaterOrEqual(weights, l);
                var last = FindLastSmallerOrEqual(weights, r);
                if (last < first || last < 0)
                    return null;
                var stack = new Stack<int>();
                var processed = new Dictionary<int, UFNode>();
                var links = new List<Link>();
                var roots = new UFNode[last - first + 1];
                for (var i = first; i <= last; i++)
                {
                    UFNode n;
                    if (processed.TryGetValue(i, out n))
                    {
                        roots[i - first] = n;
                        continue;
                    }
                    n = UFNode.CreateNew(i);
                    roots[i - first] = n;
                    processed.Add(i, n);
                    var e = edges[i];
                    stack.Push(e.v1);
                    while (stack.Count > 0)
                    {
                        var v = stack.Pop();
                        var adjacent = outgoing[v];
                        UFNode redNode = null;
                        UFNode blackNode = null;
                        foreach (var t in adjacent)
                        {
                            if (weights[t] > r)
                                continue;
                            e = edges[t];
                            UFNode node;
                            if (!processed.TryGetValue(t, out node))
                            {
                                stack.Push(v == e.v1 ? e.v2 : e.v1);
                                processed.Add(t, node = UFNode.CreateNew(t));
                            }
                            var isRed = weights[t] >= l;
                            if (isRed)
                            {
                                if (redNode == null)
                                    redNode = node;
                                else
                                    redNode.Union(node);
                            }
                            else if (blackNode == null)
                                blackNode = node;
                            else
                                blackNode.Union(node);
                        }
                        if (blackNode != null && redNode != null)
                            links.Add(new Link
                            {
                                e1 = blackNode,
                                e2 = redNode
                            });
                    }
                }
                for (var i = 0; i < roots.Length; i++)
                    roots[i] = roots[i].Find();
                var result = new RedBlackTree(roots);
                foreach (var link in links)
                {
                    var r1 = link.e1.Find();
                    var r2 = link.e2.Find();
                    result.Include(r1, r2);
                    result.Include(r2, r1);
                }
                return result;
            }
        }

        public static int FindFirstGreaterOrEqual(int[] array, int target)
        {
            var left = 0;
            var right = array.Length - 1;
            var result = -1;
            while (left <= right)
            {
                var mid = left + (right - left)/2;
                var midValue = array[mid];
                if (target <= midValue)
                {
                    right = mid - 1;
                    result = mid;
                }
                else
                    left = mid + 1;
            }
            return result;
        }

        public static int FindLastSmallerOrEqual(int[] array, int target)
        {
            var left = 0;
            var right = array.Length - 1;
            var result = -1;
            while (left <= right)
            {
                var mid = left + (right - left)/2;
                var midValue = array[mid];
                if (target < midValue)
                    right = mid - 1;
                else
                {
                    result = mid;
                    left = mid + 1;
                }
            }
            return result;
        }

        private class UFNode
        {
            public int edge;
            private UFNode parent;
            public uint size;
            private int rank;

            public static UFNode CreateNew(int edge)
            {
                var result = new UFNode
                {
                    size = 1,
                    rank = 0,
                    edge = edge
                };
                result.parent = result;
                return result;
            }

            public UFNode Find()
            {
                var n = this;
                while (n.parent != n)
                {
                    n.parent = n.parent.parent;
                    n = n.parent;
                }
                return n;
            }

            public void Union(UFNode other)
            {
                var r1 = Find();
                var r2 = other.Find();
                if (r1 == r2)
                    return;
                if (r1.rank < r2.rank)
                {
                    r1.parent = r2;
                    r2.size += r1.size;
                }
                else if (r2.rank < r1.rank)
                {
                    r2.parent = r1;
                    r1.size += r2.size;
                }
                else
                {
                    r2.parent = r1;
                    r1.rank++;
                    r1.size += r2.size;
                }
            }
        }

        private class RedBlackTree
        {
            public readonly UFNode[] roots;

            public readonly Dictionary<UFNode, Dictionary<UFNode, uint>> outgoing =
                new Dictionary<UFNode, Dictionary<UFNode, uint>>();

            public RedBlackTree(UFNode[] roots)
            {
                this.roots = roots;
            }

            public void Include(UFNode e1, UFNode e2)
            {
                Dictionary<UFNode, uint> adjacent;
                if (!outgoing.TryGetValue(e1, out adjacent))
                    outgoing.Add(e1, adjacent = new Dictionary<UFNode, uint>());
                uint count;
                if (adjacent.TryGetValue(e2, out count))
                    adjacent[e2] = count + 1;
                else
                    adjacent.Add(e2, 1);
            }
        }

        private class Link
        {
            public UFNode e1;
            public UFNode e2;
        }

        private class TopSortDescriptor
        {
            public int low;
            public int high;
            public int count;
            public int number;
        }

        private class Edge
        {
            public int v1;
            public int v2;
            public int weight;
        }
    }
}