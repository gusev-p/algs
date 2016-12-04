using System;
using System.Collections.Generic;
using System.Linq;
using Algs.TestUtilities;

namespace Algs.Tasks.GraphAlg
{
    public static class ComponentsInAGraph
    {
        public static void TaskMain()
        {
            var n = Input.ReadInt();
            var outgoing = new Dictionary<int, List<int>>();
            for (var i = 0; i < n; i++)
            {
                var v = Input.ReadInts();
                Include(outgoing, v[0], v[1]);
                Include(outgoing, v[1], v[0]);
            }
            var componentNumbers = new Dictionary<int, int>();
            var queue = new Queue<int>();
            var currentNumber = 0;
            foreach (var v in outgoing.Keys)
            {
                if (componentNumbers.ContainsKey(v))
                    continue;
                componentNumbers.Add(v, currentNumber);
                queue.Enqueue(v);
                while (queue.Count > 0)
                {
                    var adjucent = outgoing[queue.Dequeue()];
                    foreach (var w in adjucent)
                        if (!componentNumbers.ContainsKey(w))
                        {
                            componentNumbers.Add(w, currentNumber);
                            queue.Enqueue(w);
                        }
                }
                currentNumber++;
            }
            var componentSizes = componentNumbers.Values
                .GroupBy(x => x, (_, y) => y.Count())
                .ToArray();
            Console.WriteLine(componentSizes.Min() + " " + componentSizes.Max());
        }

        private static void Include(Dictionary<int, List<int>> dictionary, int key, int value)
        {
            List<int> values;
            if (!dictionary.TryGetValue(key, out values))
                dictionary.Add(key, values = new List<int>());
            values.Add(value);
        }
    }
}