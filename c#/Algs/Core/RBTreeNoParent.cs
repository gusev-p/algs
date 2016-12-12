using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Algs.Core
{
    public class RBTreeNoParent
    {
        private Node root = nil;
        private static readonly Node rootHolder = new Node();

        private static readonly Node nil = new Node
        {
            color = Color.Black
        };

        public void Add(int key, int value)
        {
            rootHolder.right = root;
            var current = root;
            var parent = rootHolder;
            var grandParent = nil;
            var lastDirection = Direction.Right;
            var prevDirection = Direction.Right;
            var prevPrevDirection = Direction.Right;
            var inserted = false;
            while (true)
            {
                if (current == nil)
                {
                    current = new Node
                    {
                        color = Color.Red,
                        key = key,
                        value = value,
                        left = nil,
                        right = nil
                    };
                    if (parent == nil)
                    {
                        root = current;
                        break;
                    }
                    parent.SetChild(lastDirection, current);
                    inserted = true;
                }
                else if (current.color == Color.Black && current.left.color == Color.Red &&
                         current.right.color == Color.Red)
                {
                    current.color = Color.Red;
                    current.left.color = Color.Black;
                    current.right.color = Color.Black;
                }
                //SortedDictionary<>
                if (current.color == Color.Red && parent.color == Color.Red)
                {
                    if (lastDirection != prevDirection)
                        grandParent.SetChild(prevDirection, Rotate(parent, Reverse(lastDirection)));

                }
                if (inserted)
                    break;
                if (current.key == key)
                {
                    const string messageFormat = "key [{0}] already exist";
                    throw new InvalidOperationException(string.Format(messageFormat, key));
                }
                prevDirection = lastDirection;
                lastDirection = key < current.key ? Direction.Left : Direction.Right;
                grandParent = parent;
                parent = current;
                current = current.GetChild(lastDirection);
            }
            root = rootHolder.right;
            root.color = Color.Black;
        }

        private Node Rotate(Node node, Direction direction)
        {
            var rDirection = Reverse(direction);
            var r = node.GetChild(rDirection);
            node.SetChild(rDirection, root.GetChild(direction));
            r.SetChild(direction, node);
            return r;
        }

        public bool TryGetValue(int key, out int value)
        {
            value = 0;
            return false;
        }

        private enum Color
        {
            Red,
            Black
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Direction Reverse(Direction direction)
        {
            return direction == Direction.Left
                ? Direction.Right
                : Direction.Left;
        }

        private enum Direction
        {
            Left,
            Right
        }

        private class Node
        {
            public Color color;
            public Node left;
            public Node right;
            public int key;
            public int value;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Node GetChild(Direction direction)
            {
                return direction == Direction.Left ? left : right;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void SetChild(Direction direction, Node node)
            {
                if (direction == Direction.Left)
                    left = node;
                else
                    right = node;
            }
        }
    }
}