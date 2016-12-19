using System;
using Algs.Core;

namespace Algs.Tasks.Hashtables
{
    public static class Sum2
    {
        private const int minVal = -10000;
        private const int maxVal = 10000;

        public static void TaskMain()
        {
            var targets = new Hashset();
            var array = new long[1000000];
            for (var i = 0; i < array.Length; i++)
                array[i] = long.Parse(Console.ReadLine());
            Array.Sort(array);
            foreach (var a in array)
                if (a < minVal)
                    Include(targets, array, minVal - a, maxVal - a, a);
                else if (a < maxVal)
                    Include(targets, array, a - minVal, a + maxVal, a);
                else
                    Include(targets, array, a - minVal, a - maxVal, a);
            Console.WriteLine(targets.Count);
        }

        private static void Include(Hashset targets, long[] values, long leftValue, long rightValue, long a)
        {
            var leftIndex = ArrayHelpers.FindFirstGreaterOrEqual(values, leftValue);
            if (leftIndex == -1)
                return;
            var rightIndex = ArrayHelpers.FindLastSmallerOrEqual(values, rightValue);
            if (rightIndex == -1)
                return;
            for (var i = leftIndex; i <= rightIndex; i++)
            {
                var b = values[i];
                if (a != b)
                    targets.Include(a + b);
            }
        }

        public class Hashset
        {
            private readonly Node[] table = new Node[21767];

            public int Count { get; private set; }

            public void Include(long value)
            {
                var index = GetIndex(value);
                if (!ContainsInternal(value, index))
                {
                    table[index] = new Node
                    {
                        value = value,
                        next = table[index]
                    };
                    Count++;
                }
            }

            private int GetIndex(long value)
            {
                long hashcode = value.GetHashCode();
                hashcode -= int.MinValue;
                return (int) (hashcode%table.Length);
            }

            public bool Contains(long value)
            {
                return ContainsInternal(value, GetIndex(value));
            }

            private bool ContainsInternal(long value, int index)
            {
                for (var n = table[index]; n != null; n = n.next)
                    if (n.value == value)
                        return true;
                return false;
            }

            private class Node
            {
                public Node next;
                public long value;
            }
        }
    }
}