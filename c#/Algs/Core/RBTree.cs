using System;
using System.Collections.Generic;
using System.Text;

namespace Algs.Core
{
    public class RBTree
    {
        private Node root = nil;

        public bool TryGetValue(int key, out int value)
        {
            for (var x = root; x != nil; x = key < x.key ? x.left : x.right)
                if (x.key == key)
                {
                    value = x.value;
                    return true;
                }
            value = 0;
            return false;
        }

        public string Dump()
        {
            return TreeDumper.Dump(root);
        }

        public void Add(int key, int value)
        {
            var newNode = new Node
            {
                color = Color.Red,
                key = key,
                value = value,
                left = nil,
                right = nil
            };
            Count++;
            if (root == nil)
            {
                root = newNode;
                root.color = Color.Black;
                root.parent = nil;
                return;
            }
            var x = root;
            Node p;
            do
            {
                if (x.key == key)
                {
                    const string messageFormat = "key [{0}] already exist";
                    throw new InvalidOperationException(string.Format(messageFormat, key));
                }
                p = x;
                x = key < x.key ? x.left : x.right;
            } while (x != nil);
            newNode.parent = p;
            if (key < p.key)
                p.left = newNode;
            else
                p.right = newNode;
            x = newNode;
            while (x.parent.color == Color.Red)
            {
                var a = x.parent;
                var b = a.parent;
                if (a == b.left)
                {
                    var y = b.right;
                    if (y.color == Color.Red)
                    {
                        y.color = Color.Black;
                        a.color = Color.Black;
                        b.color = Color.Red;
                        x = b;
                    }
                    else if (x == a.right)
                    {
                        a.right = x.left;
                        if (a.right != nil)
                            a.right.parent = a;
                        b.left = x.right;
                        if (b.left != nil)
                            b.left.parent = b;
                        x.parent = b.parent;
                        if (x.parent == nil)
                            root = x;
                        else if (x.parent.left == b)
                            x.parent.left = x;
                        else
                            x.parent.right = x;
                        x.left = a;
                        a.parent = x;
                        x.right = b;
                        b.parent = x;
                        x.color = Color.Black;
                        b.color = Color.Red;
                        break;
                    }
                    else
                    {
                        b.left = a.right;
                        if (b.left != nil)
                            b.left.parent = b;
                        a.parent = b.parent;
                        if (a.parent == nil)
                            root = a;
                        else if (a.parent.left == b)
                            a.parent.left = a;
                        else
                            a.parent.right = a;
                        a.right = b;
                        b.parent = a;
                        a.color = Color.Black;
                        b.color = Color.Red;
                        break;
                    }
                }
                else
                {
                    var y = b.left;
                    if (y.color == Color.Red)
                    {
                        y.color = Color.Black;
                        a.color = Color.Black;
                        b.color = Color.Red;
                        x = b;
                    }
                    else if (x == a.left)
                    {
                        b.right = x.left;
                        if (b.right != nil)
                            b.right.parent = b;
                        a.left = x.right;
                        if (a.left != nil)
                            a.left.parent = a;
                        x.parent = b.parent;
                        if (x.parent == nil)
                            root = x;
                        else if (x.parent.left == b)
                            x.parent.left = x;
                        else
                            x.parent.right = x;
                        x.left = b;
                        b.parent = x;
                        x.right = a;
                        a.parent = x;
                        b.color = Color.Red;
                        x.color = Color.Black;
                        break;
                    }
                    else
                    {
                        b.right = a.left;
                        if (b.right != nil)
                            b.right.parent = b;
                        a.parent = b.parent;
                        if (a.parent == nil)
                            root = a;
                        else if (a.parent.left == b)
                            a.parent.left = a;
                        else
                            a.parent.right = a;
                        a.left = b;
                        b.parent = a;
                        b.color = Color.Red;
                        a.color = Color.Black;
                        break;
                    }
                }
            }
            root.color = Color.Black;
        }

        public int GetHeight()
        {
            return root == nil ? 0 : GetHeight(root);
        }

        private static int GetHeight(Node n)
        {
            var height = 0;
            if (n.left != nil)
                height = GetHeight(n.left) + 1;
            if (n.right != nil)
            {
                var h = GetHeight(n.right) + 1;
                if (h > height)
                    height = h;
            }
            return height;
        }

        public int Count { get; private set; }

        private enum Color
        {
            Red,
            Black
        }

        private static readonly Node nil = new Node
        {
            color = Color.Black
        };

        private class Node
        {
            public Color color;
            public Node left;
            public Node right;
            public Node parent;
            public int key;
            public int value;
        }

        private static class TreeDumper
        {
            public static string Dump(Node node)
            {
                if (node == nil)
                    return "<nil>";
                var levels = new List<Level>();
                FillLevels(node, 0, 0, levels);
                var b = new StringBuilder();
                foreach (var level in levels)
                    level.Write(b);
                return b.ToString();
            }

            private static int FillLevels(Node node, int level, int shift, List<Level> target)
            {
                if (level >= target.Count)
                    target.Add(new Level());
                if (node.left != nil)
                    shift = FillLevels(node.left, level + 1, shift, target);
                target[level].Add(shift, node.key);
                shift += 1;
                if (node.right != nil)
                    shift = FillLevels(node.right, level + 1, shift, target);
                return shift;
            }

            private class Level
            {
                private readonly List<Block> blocks = new List<Block>();
                private int length;

                public void Add(int shift, int value)
                {
                    var indent = shift - length;
                    blocks.Add(new Block
                    {
                        value = value,
                        indent = indent
                    });
                    length += indent + 1;
                }

                public void Write(StringBuilder target)
                {
                    foreach (var b in blocks)
                    {
                        if (b.indent > 0)
                            target.Append(new string(' ', b.indent*5));
                        target.Append(b.value.ToString().PadRight(5));
                    }
                    target.AppendLine();
                }
            }

            private class Block
            {
                public int indent;
                public int value;
            }
        }
    }
}