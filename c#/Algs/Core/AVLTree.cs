namespace Algs.Core
{
    public class AVLTree
    {
        private Node root;

        public void Insert(int key)
        {
            var newNode = new Node
            {
                key = key,
                height = 0
            };
            if (root == null)
                root = newNode;
            else
                DoInsert(root, newNode);
        }

        private static Node DoInsert(Node current, Node nodeToInsert)
        {
            if (nodeToInsert.key < current.key)
            {
                current.left = current.left == null
                    ? nodeToInsert
                    : DoInsert(current.left, nodeToInsert);
                UpdateHeight(current);
                if (GetLeftHeight(current) <= GetRightHeight(current) + 1)
                    return current;
                if (GetRightHeight(current.left) > GetLeftHeight(current.left))
                    current.left = RotateLeft(current.left);
                return RotateRight(current);
            }
            current.right = current.right == null
                ? nodeToInsert
                : DoInsert(current.right, nodeToInsert);
            UpdateHeight(current);
            if (GetRightHeight(current) <= GetLeftHeight(current) + 1)
                return current;
            if (GetLeftHeight(current.right) > GetRightHeight(current.right))
                current.right = RotateRight(current.right);
            return RotateLeft(current);
        }

        private static Node RotateLeft(Node node)
        {
            var z = node.right;
            node.right = z.left;
            z.left = node;
            UpdateHeight(node);
            UpdateHeight(z);
            return z;
        }

        private static Node RotateRight(Node node)
        {
            var z = node.left;
            node.left = z.right;
            z.right = node;
            UpdateHeight(node);
            UpdateHeight(z);
            return z;
        }

        private static int GetLeftHeight(Node node)
        {
            return node.left == null ? -1 : node.left.height;
        }

        private static int GetRightHeight(Node node)
        {
            return node.right == null ? -1 : node.right.height;
        }

        private static void UpdateHeight(Node node)
        {
            var leftHeight = GetLeftHeight(node);
            var rightHeight = GetRightHeight(node);
            var height = leftHeight > rightHeight ? leftHeight : rightHeight;
            node.height = height + 1;
        }

        private class Node
        {
            public Node left;
            public Node right;
            public int key;
            public int height;
        }
    }
}