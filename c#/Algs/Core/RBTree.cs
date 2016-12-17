using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Algs.Core
{
    //Реализация RBTree на узлах со ссылками на родителя.
    //Как в CLRS, только с заинлайненными вращениями.
    //Почему-то быстрее оказывается, чем однопроходная версия, 
    public class RBTree
    {
        private Node root = nil;

        public int GetValue(int key)
        {
            int value;
            if (!TryGetValue(key, out value))
            {
                const string messageFormat = "key [{0}] not found";
                throw new InvalidOperationException(string.Format(messageFormat, key));
            }
            return value;
        }

        public bool TryGetValue(int key, out int value)
        {
            var n = FindNode(key);
            if (n != null)
            {
                value = n.value;
                return true;
            }
            value = 0;
            return false;
        }

        public void Add(int key, int value)
        {
            if (!TryAdd(key, value))
            {
                const string messageFormat = "key [{0}] already exist";
                throw new InvalidOperationException(string.Format(messageFormat, key));
            }
        }

        public bool TryAdd(int key, int value)
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
                return true;
            }
            var x = root;
            Node p;
            do
            {
                if (x.key == key)
                    return false;
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
            return true;
        }

        public int this[int key]
        {
            get { return GetValue(key); }
            set
            {
                var n = FindNode(key);
                if (n != null)
                    n.value = value;
                else
                    Add(key, value);
            }
        }

        public void Remove(int key)
        {
            if (!TryRemove(key))
            {
                const string mesageFormat = "key [{0}] not found";
                throw new InvalidOperationException(string.Format(mesageFormat, key));
            }
        }

        public bool TryRemove(int key)
        {
            var y = FindNode(key);
            if (y == null)
                return false;
            if (y.left != nil && y.right != nil)
            {
                var original = y;
                y = y.right;
                while (y.left != nil)
                    y = y.left;
                original.key = y.key;
                original.value = y.value;
            }
            var x = y.left == nil ? y.right : y.left;
            if (x != nil)
                x.parent = y.parent;
            if (y.parent == nil)
                root = x;
            else if (y == y.parent.left)
                y.parent.left = x;
            else
                y.parent.right = x;
            Count--;
            if (y.color == Color.Red)
                return true;
            var p = y.parent;
            while (x != root && x.color != Color.Red)
            {
                if (x == p.left)
                {
                    var w = p.right;
                    if (w.color == Color.Red)
                    {
                        p.right = w.left;
                        if (p.right != nil)
                            p.right.parent = p;
                        w.left = p;
                        w.parent = p.parent;
                        if (p.parent == nil)
                            root = w;
                        else if (p == p.parent.left)
                            w.parent.left = w;
                        else
                            w.parent.right = w;
                        p.parent = w;
                        w.color = Color.Black;
                        p.color = Color.Red;
                        w = p.right;
                    }
                    if (w.left.color == Color.Black && w.right.color == Color.Black)
                    {
                        w.color = Color.Red;
                        x = p;
                        p = x.parent;
                    }
                    else
                    {
                        if (w.right.color == Color.Black)
                        {
                            var a = w.left;
                            w.left = a.right;
                            if (w.left != nil)
                                w.left.parent = w;
                            a.parent = p;
                            a.right = w;
                            w.parent = a;
                            a.color = Color.Black;
                            w.color = Color.Red;
                            w = a;
                        }
                        p.right = w.left;
                        if (p.right != nil)
                            p.right.parent = p;
                        w.left = p;
                        w.parent = p.parent;
                        if (p.parent == nil)
                            root = w;
                        else if (p == p.parent.left)
                            w.parent.left = w;
                        else
                            w.parent.right = w;
                        p.parent = w;
                        w.color = p.color;
                        p.color = Color.Black;
                        w.right.color = Color.Black;
                        break;
                    }
                }
                else
                {
                    var w = p.left;
                    if (w.color == Color.Red)
                    {
                        p.left = w.right;
                        if (p.left != nil)
                            p.left.parent = p;
                        w.right = p;
                        w.parent = p.parent;
                        if (p.parent == nil)
                            root = w;
                        else if (p == p.parent.left)
                            w.parent.left = w;
                        else
                            w.parent.right = w;
                        p.parent = w;
                        w.color = Color.Black;
                        p.color = Color.Red;
                        w = p.left;
                    }
                    if (w.left.color == Color.Black && w.right.color == Color.Black)
                    {
                        w.color = Color.Red;
                        x = p;
                        p = x.parent;
                    }
                    else
                    {
                        if (w.left.color == Color.Black)
                        {
                            var b = w.right;
                            w.right = b.left;
                            if (w.right != nil)
                                w.right.parent = w;
                            w.parent = b;
                            b.left = w;
                            b.parent = p;
                            w.color = Color.Red;
                            b.color = Color.Black;
                            w = b;
                        }
                        p.left = w.right;
                        if (p.left != nil)
                            p.left.parent = p;
                        w.right = p;
                        w.parent = p.parent;
                        if (p.parent == nil)
                            root = w;
                        else if (p == p.parent.left)
                            w.parent.left = w;
                        else
                            w.parent.right = w;
                        p.parent = w;
                        w.color = p.color;
                        p.color = Color.Black;
                        w.left.color = Color.Black;
                        break;
                    }
                }
            }
            x.color = Color.Black;
            return true;
        }

        public string Dump()
        {
            return TreeDumper.Dump(root);
        }

        public int GetHeight()
        {
            return root == nil ? 0 : GetHeight(root);
        }

        public void Check()
        {
            Check(root);
        }

        private static int Check(Node n)
        {
            if (n == nil)
            {
                Assert.That(n.color, Is.EqualTo(Color.Black));
                Assert.That(n.left, Is.Null);
                Assert.That(n.right, Is.Null);
                Assert.That(n.parent, Is.Null);
                Assert.That(n.key, Is.EqualTo(0));
                Assert.That(n.value, Is.EqualTo(0));
                return 1;
            }
            if (n.left != nil && n.left.key >= n.key)
                Assert.Fail("BST violation for left child of " + n.key);
            if (n.right != nil && n.right.key <= n.key)
                Assert.Fail("BST violation for right child of " + n.key);
            if (n.color == Color.Red)
            {
                if(n.left.color == Color.Red)
                    Assert.Fail("red constraint violation for left child of " + n.key);
                if (n.right.color == Color.Red)
                    Assert.Fail("red constraint violation for right child of " + n.key);
            }
            var leftBlackHeight = Check(n.left);
            var rightBlackHeight = Check(n.right);
            if (leftBlackHeight != rightBlackHeight)
                Assert.Fail("black constraint violation for " + n.key);
            return (n.color == Color.Black ? 1 : 0) + leftBlackHeight;
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

        private Node FindNode(int key)
        {
            for (var x = root; x != nil; x = key < x.key ? x.left : x.right)
                if (x.key == key)
                    return x;
            return null;
        }

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