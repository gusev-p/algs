using System;
using System.Collections.Generic;

namespace Algs.Tasks.GraphAlg
{
    public static class KargerMinCut
    {
        private const int verticiesCount = 200;

        public static void TaskMain()
        {
            var edgeFactory = CreateEdgeFactory();
            var minCutSize = int.MaxValue;
            var iterationsCount = (int) (verticiesCount*verticiesCount*Math.Log(verticiesCount)) + 1;
            for (var i = 0; i < iterationsCount; i++)
            {
                var edges = edgeFactory();
                var edgesCount = edges.Length;
                var random = new Random(i*Environment.TickCount);
                for (var j = 0; j < verticiesCount - 2; j++)
                {
                    var contractionEdge = edges[random.Next(edgesCount)];
                    var v1Incident = contractionEdge.v1.incidentEdges;
                    var v2Incident = contractionEdge.v2.incidentEdges;
                    var e = v1Incident.First;
                    while (e != null)
                    {
                        var next = e.Next;
                        if (e.Value.v1 == contractionEdge.v2 || e.Value.v2 == contractionEdge.v2)
                        {
                            if (e.Value.index < edgesCount - 1)
                            {
                                edges[e.Value.index] = edges[edgesCount - 1];
                                edges[e.Value.index].index = e.Value.index;
                            }
                            edgesCount--;
                            if (edgesCount <= 0)
                                throw new InvalidOperationException("shit222");
                            v1Incident.Remove(e);
                        }
                        e = next;
                    }
                    e = v2Incident.First;
                    while (e != null)
                    {
                        if (e.Value.v1 != contractionEdge.v1 && e.Value.v2 != contractionEdge.v1)
                        {
                            if (e.Value.v1 == contractionEdge.v2)
                                e.Value.v1 = contractionEdge.v1;
                            else
                                e.Value.v2 = contractionEdge.v1;
                            v1Incident.AddLast(e.Value);
                        }
                        e = e.Next;
                    }
                }
                var currentCutSize = edges[0].v1.incidentEdges.Count;
                if (currentCutSize < minCutSize)
                    minCutSize = currentCutSize;
                if ((i + 1)%20 == 0)
                    Console.Out.WriteLine("iteration [{0}] completed, [{1}%]",
                        i + 1, (double) (i + 1)/iterationsCount*100);
            }
            Console.WriteLine(minCutSize);
        }

        private static Func<Edge[]> CreateEdgeFactory()
        {
            var adjacencyList = new int[verticiesCount][];
            var edgesCount = 0;
            for (var i = 0; i < adjacencyList.Length; i++)
            {
                var adjacent = Console.ReadLine().Split(new[] {'\t'}, StringSplitOptions.RemoveEmptyEntries);
                adjacencyList[i] = Array.ConvertAll(adjacent, s =>
                {
                    int result;
                    if (!int.TryParse(s, out result))
                    {
                        const string messageFormat = "can't parse int from [{0}]";
                        throw new InvalidOperationException(string.Format(messageFormat, s));
                    }
                    return result;
                });
                edgesCount += adjacent.Length - 1;
            }
            edgesCount /= 2;
            return delegate
            {
                var verticies = new Vertex[verticiesCount];
                for (var i = 0; i < verticies.Length; i++)
                    verticies[i] = new Vertex();
                var edges = new Edge[edgesCount];
                var index = 0;
                foreach (var adjacent in adjacencyList)
                    for (var j = 1; j < adjacent.Length; j++)
                    {
                        var v1 = adjacent[0] - 1;
                        var v2 = adjacent[j] - 1;
                        if (v1 > v2)
                            continue;
                        var newEdge = new Edge
                        {
                            v1 = verticies[v1],
                            v2 = verticies[v2],
                            index = index
                        };
                        verticies[v1].incidentEdges.AddLast(newEdge);
                        verticies[v2].incidentEdges.AddLast(newEdge);
                        edges[index++] = newEdge;
                    }
                return edges;
            };
        }

        private class Vertex
        {
            public readonly LinkedList<Edge> incidentEdges = new LinkedList<Edge>();
        }

        private class Edge
        {
            public Vertex v1;
            public Vertex v2;
            public int index;
        }
    }
}