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
            private readonly List<int>[] outgoing;
            private readonly Edge[] edges;
            private readonly TopSortItem[] topSort;
            private readonly int root;

            public TreePathsSearcher(Edge[] edges)
            {
                this.edges = edges;
                outgoing = new List<int>[edges.Length + 1];
                for (var i = 0; i < outgoing.Length; i++)
                    outgoing[i] = new List<int>();
                for (var i = 0; i < edges.Length; i++)
                {
                    var edge = edges[i];
                    outgoing[edge.v1].Add(i);
                    outgoing[edge.v2].Add(i);
                }
                topSort = new TopSortItem[edges.Length + 1];
                root = new CenterFinder(outgoing, edges).Find();
                new TopSorter(topSort, outgoing, edges).TopSort(root);
            }

            public ulong CountPathsWithCostBetween(int l, int r)
            {
                var builder = new RedBlackTreeBuilder(l, r, outgoing, edges, topSort, root);
                return builder.Build().CalculatePathsCount();
            }
        }

        private class UFNode
        {
            public Color color;
            private UFNode parent;
            public uint size;
            private int rank;

            public static UFNode CreateNew(Color color, uint size)
            {
                var result = new UFNode
                {
                    size = size,
                    rank = 0,
                    color = color
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
            private readonly List<UFNode> roots;

            private readonly Dictionary<UFNode, Dictionary<UFNode, uint>> outgoing =
                new Dictionary<UFNode, Dictionary<UFNode, uint>>();

            public RedBlackTree(List<UFNode> roots)
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

            public ulong CalculatePathsCount()
            {
                ulong result = 0;
                var stack = new Stack<UFNode>();
                var processed = new HashSet<UFNode>();
                foreach (var root in roots)
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
                        if (!outgoing.TryGetValue(e, out adjacent))
                            continue;
                        foreach (var x in adjacent)
                        {
                            if (!processed.Add(x.Key))
                                continue;
                            stack.Push(x.Key);
                            var xIsRed = x.Key.color == Color.Red;
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
        }

        private enum Color
        {
            Black,
            Red
        }

        private class Link
        {
            public UFNode n1;
            public UFNode n2;
        }

        private class Edge
        {
            public int v1;
            public int v2;
            public int weight;
        }

        private class RedBlackTreeBuilder
        {
            private readonly List<Link> links = new List<Link>();
            private readonly List<UFNode> roots = new List<UFNode>();
            private readonly int l;
            private readonly int r;
            private readonly List<int>[] outgoing;
            private readonly Edge[] edges;
            private readonly TopSortItem[] topSort;
            private readonly int root;

            public RedBlackTreeBuilder(int l, int r,
                List<int>[] outgoing, Edge[] edges,
                TopSortItem[] topSort, int root)
            {
                this.l = l;
                this.r = r;
                this.outgoing = outgoing;
                this.edges = edges;
                this.topSort = topSort;
                this.root = root;
            }

            public RedBlackTree Build()
            {
                Visit(root, -1, null);
                for (var i = 0; i < roots.Count; i++)
                    roots[i] = roots[i].Find();
                var result = new RedBlackTree(roots);
                foreach (var link in links)
                {
                    var r1 = link.n1.Find();
                    var r2 = link.n2.Find();
                    result.Include(r1, r2);
                    result.Include(r2, r1);
                }
                return result;
            }

            private void Visit(int parent, int parentEdge, UFNode parentNode)
            {
                var topSortItem = topSort[parent];
                if (topSortItem.count == 0)
                    return;
                var match = topSortItem.Match(l, r);
                if (match != MatchResult.None)
                {
                    if (match == MatchResult.Brown)
                        return;
                    var node = UFNode.CreateNew(match == MatchResult.Black ? Color.Black : Color.Red,
                        topSortItem.count);
                    if (parentNode != null && parentNode.color == node.color)
                        parentNode.Union(node);
                    else if (parentNode != null)
                        links.Add(new Link {n1 = parentNode, n2 = node});
                    else if (node.color == Color.Red)
                        roots.Add(node);
                    return;
                }
                var adjacent = outgoing[parent];
                UFNode blackNode = null;
                UFNode redNode = null;
                foreach (var e in adjacent)
                {
                    var edge = edges[e];
                    UFNode childNode;
                    if (edge.weight > r)
                        childNode = null;
                    else
                    {
                        if (e == parentEdge)
                        {
                            if (parentNode == null)
                                throw new InvalidOperationException("assertion failure");
                            childNode = parentNode;
                        }
                        else
                            childNode = UFNode.CreateNew(edge.weight < l ? Color.Black : Color.Red, 1);
                        if (childNode.color == Color.Red)
                        {
                            if (redNode == null)
                                redNode = childNode;
                            else
                                redNode.Union(childNode);
                        }
                        else if (blackNode == null)
                            blackNode = childNode;
                        else
                            blackNode.Union(childNode);
                    }
                    if (e != parentEdge)
                        Visit(parent == edge.v1 ? edge.v2 : edge.v1, e, childNode);
                }
                if (blackNode != null && redNode != null)
                    links.Add(new Link
                    {
                        n1 = blackNode,
                        n2 = redNode
                    });
                if (parentNode == null && redNode != null)
                    roots.Add(redNode);
            }
        }

        private class CenterFinder
        {
            private readonly List<int>[] outgoing;
            private readonly Edge[] edges;
            private readonly int[] distances;

            public CenterFinder(List<int>[] outgoing, Edge[] edges)
            {
                this.outgoing = outgoing;
                this.edges = edges;
                distances = new int[outgoing.Length];
            }

            public int Find()
            {
                DistanceDFS(0, -1);
                var currentNode = 0;
                var currentDistance = distances[currentNode];
                while (true)
                {
                    var adjacent = outgoing[currentNode];
                    var max = -1;
                    var maxNode = -1;
                    foreach (var t in adjacent)
                    {
                        var e = edges[t];
                        var v = e.v1 == currentNode ? e.v2 : e.v1;
                        if (distances[v] > max)
                        {
                            max = distances[v];
                            maxNode = v;
                        }
                    }
                    if (max < 0)
                        break;
                    var newResult = Math.Max(1 + currentDistance, max);
                    if (newResult >= currentDistance)
                        break;
                    currentDistance = newResult;
                    currentNode = maxNode;
                }
                return currentNode;
            }

            private int DistanceDFS(int parent, int parentEdge)
            {
                var adjacent = outgoing[parent];
                var result = 0;
                foreach (var t in adjacent)
                {
                    if (t == parentEdge)
                        continue;
                    var edge = edges[t];
                    var childDistance = DistanceDFS(edge.v1 == parent ? edge.v2 : edge.v1, t) + 1;
                    if (childDistance > result)
                        result = childDistance;
                }
                return distances[parent] = result;
            }
        }

        private class TopSorter
        {
            private readonly TopSortItem[] items;
            private readonly List<int>[] outgoing;
            private readonly Edge[] edges;

            public TopSorter(TopSortItem[] items, List<int>[] outgoing, Edge[] edges)
            {
                this.items = items;
                this.outgoing = outgoing;
                this.edges = edges;
            }

            public void TopSort(int root)
            {
                Visit(root, -1);
            }

            private void Visit(int parent, int grandParent)
            {
                var adjacent = outgoing[parent];
                var parentDescriptor = items[parent] = new TopSortItem
                {
                    max = -1,
                    min = int.MaxValue,
                    count = 0
                };
                foreach (var x in adjacent)
                {
                    var edge = edges[x];
                    var child = edge.v1 == parent ? edge.v2 : edge.v1;
                    if (child == grandParent)
                        continue;
                    Visit(child, parent);
                    var childDescriptor = items[child];
                    var childMax = edge.weight > childDescriptor.max
                        ? edge.weight
                        : childDescriptor.max;
                    if (childMax > parentDescriptor.max)
                        parentDescriptor.max = childMax;
                    var childMin = edge.weight < childDescriptor.min
                        ? edge.weight
                        : childDescriptor.min;
                    if (childMin < parentDescriptor.min)
                        parentDescriptor.min = childMin;
                    parentDescriptor.count += childDescriptor.count + 1;
                }
            }
        }

        private class TopSortItem
        {
            public int max;
            public int min;
            public uint count;

            public MatchResult Match(int l, int r)
            {
                if (l > max)
                    return MatchResult.Black;
                if (l > min)
                    return MatchResult.None;
                if (r < min)
                    return MatchResult.Brown;
                if (r < max)
                    return MatchResult.None;
                return MatchResult.Red;
            }
        }

        public enum MatchResult
        {
            None,
            Black,
            Red,
            Brown
        }
    }
}