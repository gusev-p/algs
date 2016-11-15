using System;
using System.Collections.Generic;
using Algs.Utilities;

namespace Algs.Tasks.GraphAlg
{
    public class PrimMSTSpecialSubtree
    {
        public static void TaskMain()
        {
            var line0 = Console.ReadLine().Split(' ');
            var n = int.Parse(line0[0]);
            var m = int.Parse(line0[1]);
            var graph = new OutgoingWeightedGraph(n);
            for (var _ = 0; _ < m; _++)
            {
                var edgeLine = Console.ReadLine().Split(' ');
                var x = int.Parse(edgeLine[0]);
                var y = int.Parse(edgeLine[1]);
                var r = int.Parse(edgeLine[2]);
                graph.AddBilateralEdge(x - 1, y - 1, r);
            }
            var s = int.Parse(Console.ReadLine());
            var mstWeight = PrimMSTAlgorithm.GetMSTWeight(graph, s - 1);
            Console.WriteLine(mstWeight);
        }

        public static class PrimMSTAlgorithm
        {
            private const int maxWeight = 100000;

            public static long GetMSTWeight(OutgoingWeightedGraph graph, int startNode)
            {
                var distanceToMST = new int[graph.NodesCount];
                var queueHandles = new int[graph.NodesCount];
                var rest = new PriorityQueue<int>(graph.NodesCount,
                    (n1, n2) =>
                    {
                        if (distanceToMST[n1] < distanceToMST[n2])
                            return MostPriority.First;
                        if (distanceToMST[n1] > distanceToMST[n2])
                            return MostPriority.Second;
                        return MostPriority.Both;
                    });
                rest.OnHandleChanged += (node, i) => queueHandles[node] = i;
                for (var i = 0; i < graph.NodesCount; i++)
                {
                    distanceToMST[i] = i == startNode ? 0 : maxWeight + 1;
                    rest.Insert(i);
                }
                var mstWeight = 0;
                while (rest.Count > 0)
                {
                    var u = rest.ExtractTop();
                    mstWeight += distanceToMST[u];
                    var outgoing = graph.GetOutgoing(u);
                    foreach (var v in outgoing)
                    {
                        var queueHandle = queueHandles[v.node];
                        if (queueHandle < 0)
                            continue;
                        if (v.weight < distanceToMST[v.node])
                        {
                            distanceToMST[v.node] = v.weight;
                            rest.Promote(queueHandle);
                        }
                    }
                }
                return mstWeight;
            }
        }

        public class OutgoingWeightedGraph
        {
            private readonly List<Edge>[] outgoing;

            public OutgoingWeightedGraph(int nodesCount)
            {
                outgoing = new List<Edge>[nodesCount];
                for (var i = 0; i < outgoing.Length; i++)
                    outgoing[i] = new List<Edge>();
            }

            public int NodesCount
            {
                get { return outgoing.Length; }
            }

            public void AddBilateralEdge(int n1, int n2, int r)
            {
                AddEdge(n1, n2, r);
                AddEdge(n2, n1, r);
            }

            public List<Edge> GetOutgoing(int node)
            {
                return outgoing[node];
            }

            private void AddEdge(int from, int to, int weight)
            {
                outgoing[from].Add(new Edge {node = to, weight = weight});
            }

            public struct Edge
            {
                public int node;
                public int weight;
            }
        }

        public class PriorityQueue<T>
        {
            private readonly Func<T, T, MostPriority> prioritizer;
            public event Action<T, int> OnHandleChanged;
            private readonly T[] values;

            public PriorityQueue(int maxItemsCount, Func<T, T, MostPriority> prioritizer)
            {
                this.prioritizer = prioritizer;
                values = new T[maxItemsCount + 1];
            }

            public int Count { get; private set; }

            public void Insert(T value)
            {
                if (Count == values.Length - 1)
                    throw new InvalidOperationException("overflow");
                Count++;
                SetValue(Count, value);
                Promote(Count);
            }

            public int Promote(int handle)
            {
                var index = handle;
                while (true)
                {
                    var parentIndex = index >> 1;
                    if (parentIndex == 0 || Prioritize(index, parentIndex) != MostPriority.First)
                        return index;
                    Exchange(parentIndex, index);
                    index = parentIndex;
                }
            }

            public T ExtractTop()
            {
                var result = values[1];
                NotifyHandleChanged(result, -1);
                SetValue(1, values[Count]);
                Count--;
                var index = 1;
                while (true)
                {
                    var largestIndex = index;
                    var leftIndex = index << 1;
                    if (leftIndex <= Count && Prioritize(leftIndex, index) == MostPriority.First)
                        largestIndex = leftIndex;
                    var rightIndex = leftIndex | 1;
                    if (rightIndex <= Count && Prioritize(rightIndex, largestIndex) == MostPriority.First)
                        largestIndex = rightIndex;
                    if (largestIndex == index)
                        break;
                    Exchange(index, largestIndex);
                    index = largestIndex;
                }
                return result;
            }

            private void Exchange(int i, int j)
            {
                var t = values[i];
                SetValue(i, values[j]);
                SetValue(j, t);
            }

            private void SetValue(int index, T value)
            {
                values[index] = value;
                NotifyHandleChanged(value, index);
            }

            private MostPriority Prioritize(int targetIndex, int comparandIndex)
            {
                return prioritizer(values[targetIndex], values[comparandIndex]);
            }

            private void NotifyHandleChanged(T value, int newHandle)
            {
                if (OnHandleChanged != null)
                    OnHandleChanged(value, newHandle);
            }
        }
    }
}