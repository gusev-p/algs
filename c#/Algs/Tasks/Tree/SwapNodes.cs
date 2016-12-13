using System;
using System.Text;
using Algs.TestUtilities;

namespace Algs.Tasks.Tree
{
    public static class SwapNodes
    {
        public static void TaskMain()
        {
            var n = Input.ReadInt();
            var nodes = new Node[n + 1];
            for (var i = 1; i <= n; i++)
            {
                var line = Input.ReadInts();
                nodes[i].left = line[0];
                nodes[i].right = line[1];
            }
            var t = Input.ReadInt();
            for (var i = 0; i < t; i++)
            {
                var k = Input.ReadInt();
                ApplySwap(nodes, 1, 1, k);
                Console.WriteLine(new InorderTraversal(nodes).Traverse());
            }
        }

        private static void ApplySwap(Node[] tree, int n, int depth, int k)
        {
            if (n < 0)
                return;
            if (k == depth)
            {
                var t = tree[n].left;
                tree[n].left = tree[n].right;
                tree[n].right = t;
                ApplySwap(tree, tree[n].left, 1, k);
                ApplySwap(tree, tree[n].right, 1, k);
            }
            else
            {
                ApplySwap(tree, tree[n].left, depth + 1, k);
                ApplySwap(tree, tree[n].right, depth + 1, k);
            }
        }

        private class InorderTraversal
        {
            private readonly Node[] tree;
            private readonly StringBuilder builder = new StringBuilder();
            private bool isFirst = true;

            public InorderTraversal(Node[] tree)
            {
                this.tree = tree;
            }

            public string Traverse()
            {
                DoTraverse(1);
                return builder.ToString();
            }

            private void DoTraverse(int n)
            {
                if (n < 0)
                    return;
                DoTraverse(tree[n].left);
                if (isFirst)
                    isFirst = false;
                else
                    builder.Append(" ");
                builder.Append(n);
                DoTraverse(tree[n].right);
            }
        }

        private struct Node
        {
            public int left;
            public int right;
        }
    }
}