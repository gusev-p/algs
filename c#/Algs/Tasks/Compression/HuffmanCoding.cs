using System;
using Algs.Core;
using Algs.TestUtilities;

namespace Algs.Tasks.Compression
{
    public static class HuffmanCoding
    {
        public static void TaskMain()
        {
            var symbosCount = Input.ReadInt();
            var nodes = new Node[symbosCount];
            for (var i = 0; i < symbosCount; i++)
                nodes[i] = new Node {frequency = Input.ReadInt()};
            var heap = PriorityQueue<Node>.Create(nodes,
                delegate(Node n1, Node n2)
                {
                    if (n1.frequency < n2.frequency)
                        return MostPriority.First;
                    if (n1.frequency > n2.frequency)
                        return MostPriority.Second;
                    return MostPriority.Both;
                });
            Node root;
            while (true)
            {
                var n1 = heap.ExtractTop();
                if (heap.Count == 0)
                {
                    root = n1;
                    break;
                }
                var n2 = heap.ExtractTop();
                heap.Insert(new Node
                {
                    frequency = n1.frequency + n2.frequency,
                    left = n1,
                    right = n2
                });
            }
            var maxLeaf = Traverse(root, (x, y) => x > y ? x : y);
            var minLeaf = Traverse(root, (x, y) => x < y ? x : y);
            Console.WriteLine("max leaf [{0}]", maxLeaf);
            Console.WriteLine("min leaf [{0}]", minLeaf);
        }

        private static int Traverse(Node n, Func<int, int, int> combine)
        {
            if (n.left == null)
            {
                if (n.right != null)
                    throw new InvalidOperationException("assertion failure");
                return 0;
            }
            if (n.right == null)
                throw new InvalidOperationException("assertion failure");
            return combine(Traverse(n.left, combine), Traverse(n.right, combine)) + 1;
        }

        private class Node
        {
            public int frequency;
            public Node left;
            public Node right;
        }
    }
}