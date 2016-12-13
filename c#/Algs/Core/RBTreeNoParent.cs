using System;
using System.Runtime.CompilerServices;

namespace Algs.Core
{
    //Однопроходная top-down реализация балансировки одновременно с поиском
    //в красно-черном дереве с узлами без ссылкок на родителя.
    //Изначально идея описана в пейпере Robert Tarjan
    //"Efficient top-down updating of red-black trees.",
    //современную интерпретацию можно найти, например, здесь
    //http://www.eternallyconfuzzled.com/tuts/datastructures/jsw_tut_rbtree.aspx.
    //Этот же алгоритм используется в стандартном SortedDictionary.

    //Основная идея в том, чтобы не допустить возникновения ситуации, при которой
    //после вставки нового красного узла нам пришлось бы идти вверх на неограниченное
    //число узлов. Для этого по мере спуска по дереву для текущего узла поддерживается
    //инвариант "если узел черный, то у него должен быть хотя бы один черный потомок".
    //За счет этого, после вставки у нового узла либо будет черный родитель,
    //и тогда балансировка не нужна, либо красный с черным братом. В последнем
    //случае нарушенное правило красно-черного дерева
    //"красный узел не может иметь красных детей" (red constraint)
    //решается стандартным образом, как в CLRS, одним или двумя вращениями.

    //Для поддержания инварианта рассматриваются два случая:
    //1.  У текущего черного узла два красных потомка. В этой ситуации применяем
    //    flipColors, т.е. меняем цвет текущего узла на красный,
    //    а цвет потомков - на черный. При этом мы можем нарушить red constraint
    //    между текущим узлом и его родителем, это решается пунктом 2.
    //2.  У текущего красного узла красный родитель. Так как изначально, до
    //    добавление нового узла, дерево удовлетворяло всем правилам rb-tree,
    //    такая ситуация может образоваться либо после применения пункта 1,
    //    либо после добавления нового красного узла к красному родителю.

    //    Здесь могут быть два варианта в зависимости от соотношения
    //    направлений связей (левый/правый потомок) узла с его родителем для
    //    текущего узла и для его родителя.
    //    2.a Если направления этих связей различны, то исправляем нижнюю
    //    чтобы она была равна верхней. Для этого, в зависимости от значения
    //    этой связи делаем либо правый, либо левый поворот вокруг родителя
    //    текущего узла. Таким образом, мы сводим ситуацию к 2.b.

    //    2.b Если направления связей совпадают, то вращаем вокруг дедушки текущего узла,
    //    меняем цвет дедушки - на красный, а результата вращения - на черный.
    //    Если применялся только пункт 2.b, то текущий узел остается красным, 
    //    если же 2.b применялся после 2.a, то текущий узел становится черным,
    //    т.к. он будет корнем результата вращения.

    //    Здесь важно то, что после применения этих преобразований в обоих случаях
    //    две последующие итерации спуска по дереву (если они состоятся и мы не выйдем
    //    раньше) мы не будем применять правила 1 и 2, а значит в ходе вращений мы можем не париться
    //    за обновление указателей grandParent и greatGrandParent, они обновятся
    //    сами. Тем не менее, нам нужно обновить значение parent, если применялся 2.a,
    //    им становится прадедушка, т.к. текущий узел в этом случае - корень результата
    //    вращения.

    //    Почему справедливо утверждение выше? Допустим мы применяли только 2.b, без 2.a.
    //    Тогда потомки текущего узла остались неизменными в ходе вращений.
    //    Две последующие итерации состоятся только если в пункт 2 мы попали из пункта 1,
    //    т.к. иначе мы стоим на новом узле и текущая итерация была последней.
    //    До применения colorFlip в пункте 1 дочерние узлы были
    //    красными, и по red-constrant-у не могли иметь красных потомков, что и требовалось.

    //    Допустим теперь, что мы применили 2.b после 2.a. В принципе здесь действует таже
    //    логика, что и выше, плюс дополнительное наблюдение, что мы не можем свернуть
    //    в те узлы, к которым она не применима, из-за соотношений между текущим узлом,
    //    его родителем и дедушкой, накладываемых структурой бинарного дерева поиска.

    public class RBTreeNoParent
    {
        private Node root = nil;

        private static readonly Node nil = new Node
        {
            color = Color.Black
        };

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
            var current = root;
            var parent = nil;
            var grandParent = nil;
            var greatGrandParent = nil;
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
                if (current.color == Color.Red && parent.color == Color.Red)
                {
                    if (lastDirection != prevDirection)
                    {
                        grandParent.SetChild(prevDirection, Rotate(parent, Reverse(lastDirection)));
                        parent = greatGrandParent;
                    }
                    var r = Rotate(grandParent, Reverse(prevDirection));
                    r.color = Color.Black;
                    grandParent.color = Color.Red;
                    if (greatGrandParent == nil)
                        root = r;
                    else
                        greatGrandParent.SetChild(prevPrevDirection, r);
                }
                if (inserted)
                    break;
                if (current.key == key)
                    return false;
                prevPrevDirection = prevDirection;
                prevDirection = lastDirection;
                lastDirection = key < current.key ? Direction.Left : Direction.Right;
                greatGrandParent = grandParent;
                grandParent = parent;
                parent = current;
                current = current.GetChild(lastDirection);
            }
            root.color = Color.Black;
            Count++;
            return true;
        }

        public int Count { get; private set; }

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

        private static Node Rotate(Node node, Direction direction)
        {
            var rDirection = Reverse(direction);
            var r = node.GetChild(rDirection);
            node.SetChild(rDirection, r.GetChild(direction));
            r.SetChild(direction, node);
            return r;
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