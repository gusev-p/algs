using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Algs.Tasks.GraphAlg
{
    public static class Scc
    {
        private const int verticiesCount = 875714;
        private const string sourceFile = @"C:\sources\Algs\c#\Algs\bin\Debug\scc.txt";

        public static void TaskMain()
        {
            Console.Out.WriteLine("reading edges from file");
            var edges = ReadEdges();

            Console.Out.WriteLine("reversing edges");
            ReverseEdges(edges);

            Console.Out.WriteLine("creating edges index");
            var edgesIndex = CreateOutgoingEdgesIndex(edges);

            Console.Out.WriteLine("calculating vertex numbers");
            var numbers = CalculateVertexNumbers(edges, edgesIndex);

            Console.Out.WriteLine("reorder verticies by numbers");
            var verticies = ReorderVerticiesByNumbers(numbers);

            Console.Out.WriteLine("reversing edges");
            ReverseEdges(edges);

            Console.Out.WriteLine("creating edges index");
            edgesIndex = CreateOutgoingEdgesIndex(edges);

            Console.Out.WriteLine("calculating SCC numbers");
            var sccNumbers = CalculateSccNumbers(edges, verticies, edgesIndex);

            Console.Out.WriteLine("formatting result");
            var topSccs = sccNumbers
                .GroupBy(x => x, (x, y) => new
                {
                    number = x,
                    count = y.Count()
                })
                .OrderByDescending(x => x.count)
                .Select(x => x.count)
                .Take(5);
            Console.Out.WriteLine(string.Join(" ", topSccs));
        }

        private static int[] ReorderVerticiesByNumbers(int[] numbers)
        {
            var result = new int[numbers.Length];
            for (var i = 0; i < numbers.Length; i++)
                result[numbers[i]] = i;
            return result;
        }

        private static int[] CalculateSccNumbers(Edge[] edges, int[] verticies, int[] edgesIndex)
        {
            var sccNumber = 0;
            var dfs = new DfsWithoutRecursion(edges, edgesIndex);
            for (var i = verticies.Length - 1; i >= 0; i--)
            {
                var v = verticies[i];
                if (dfs.Result[v] >= 0)
                    continue;
                var currentSccNumber = sccNumber++;
                dfs.ExecuteFrom(v, n => currentSccNumber);
            }
            return dfs.Result;
        }

        private static int[] CalculateVertexNumbers(Edge[] edges, int[] edgesIndex)
        {
            var currentNumber = 0;
            var dfs = new DfsWithoutRecursion(edges, edgesIndex);
            for (var i = 0; i < dfs.Result.Length; i++)
                if (dfs.Result[i] < 0)
                    dfs.ExecuteFrom(i, _ => currentNumber++);
            return dfs.Result;
        }

        private static int[] CreateOutgoingEdgesIndex(Edge[] edges)
        {
            Array.Sort(edges, (e1, e2) => e1.from.CompareTo(e2.from));
            var result = new int[verticiesCount];
            for (var i = 0; i < result.Length; i++)
                result[i] = -1;
            var prev = -1;
            for (var i = 0; i < edges.Length; i++)
            {
                var v = edges[i].from;
                if (v != prev)
                {
                    result[v] = i;
                    prev = v;
                }
            }
            return result;
        }

        private static void ReverseEdges(Edge[] edges)
        {
            for (var i = 0; i < edges.Length; i++)
            {
                var t = edges[i].to;
                edges[i].to = edges[i].from;
                edges[i].from = t;
            }
        }

        private static Edge[] ReadEdges()
        {
            var result = new List<Edge>(1000);
            string s;
            using (var f = OpenInputFile())
            using (var reader = new StreamReader(f))
                while ((s = reader.ReadLine()) != null)
                {
                    var items = s.Split(' ');
                    result.Add(new Edge
                    {
                        from = int.Parse(items[0]) - 1,
                        to = int.Parse(items[1]) - 1
                    });
                }
            return result.ToArray();
        }

        private static FileStream OpenInputFile()
        {
            return new FileStream(sourceFile, FileMode.Open, FileAccess.Read,
                FileShare.None, 10*1024*1024, FileOptions.SequentialScan);
        }

        private struct Edge
        {
            public int from;
            public int to;
        }

        private class DfsWithoutRecursion
        {
            private readonly Edge[] edges;
            private readonly int[] edgesIndex;
            private readonly Stack<int> stack = new Stack<int>();

            public DfsWithoutRecursion(Edge[] edges, int[] edgesIndex)
            {
                this.edges = edges;
                this.edgesIndex = edgesIndex;
                Result = new int[verticiesCount];
                for (var i = 0; i < Result.Length; i++)
                    Result[i] = -1;
            }

            public int[] Result { get; private set; }

            public void ExecuteFrom(int vertex, Func<int, int> getResultItem)
            {
                Result[vertex] = -2;
                stack.Push(vertex);
                while (stack.Count > 0)
                {
                    if (stack.Count > verticiesCount)
                        throw new InvalidOperationException("stack overflow");
                    var v = stack.Peek();
                    var allOutgoingProcessed = true;
                    var start = edgesIndex[v];
                    if (start >= 0)
                    {
                        var j = start - 2 - Result[v];
                        while (j < edges.Length && edges[j].from == v)
                        {
                            var edge = edges[j];
                            if (Result[edge.to] == -1)
                            {
                                allOutgoingProcessed = false;
                                Result[v]--;
                                Result[edge.to] = -2;
                                stack.Push(edge.to);
                                break;
                            }
                            j++;
                        }
                    }
                    if (allOutgoingProcessed)
                    {
                        stack.Pop();
                        Result[v] = getResultItem(v);
                    }
                }
            }
        }
    }
}