namespace Algs.Tasks.Tries
{
    public class Trie
    {
        private readonly Node root = new Node();

        public void Add(string contact)
        {
            root.count++;
            var n = root;
            foreach (var c in contact)
            {
                var index = c - 'a';
                if (n.children[index] == null)
                    n.children[index] = new Node();
                n = n.children[index];
                n.count++;
            }
        }

        public int Find(string contact)
        {
            var n = root;
            foreach (var c in contact)
            {
                var index = c - 'a';
                n = n.children[index];
                if (n == null)
                    return 0;
            }
            return n.count;
        }

        private class Node
        {
            public readonly Node[] children = new Node[26];
            public int count;
        }
    }
}