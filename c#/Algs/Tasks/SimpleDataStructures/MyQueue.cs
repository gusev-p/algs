using System.Collections.Generic;

namespace Algs.Tasks.SimpleDataStructures
{
    public class MyQueue<T>
    {
        private readonly Stack<T> stackNewestOnTop = new Stack<T>();
        private readonly Stack<T> stackOldestOnTop = new Stack<T>();

        public void Enqueue(T value)
        {
            stackNewestOnTop.Push(value);
        }

        public T Peek()
        {
            PrepareOldestStack();
            return stackOldestOnTop.Peek();
        }

        public T Dequeue()
        {
            PrepareOldestStack();
            return stackOldestOnTop.Pop();
        }

        private void PrepareOldestStack()
        {
            if (stackOldestOnTop.Count > 0)
                return;
            while (stackNewestOnTop.Count > 0)
                stackOldestOnTop.Push(stackNewestOnTop.Pop());
        }
    }
}