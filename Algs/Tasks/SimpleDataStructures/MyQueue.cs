using System.Collections.Generic;

namespace Algs.Tasks.SimpleDataStructures
{
    public class MyQueue<T>
    {
        private readonly Stack<T> stackNewestOnTop = new Stack<T>();
        private readonly Stack<T> stackOldestOnTop = new Stack<T>();

        public void Enqueue(T value)
        {
            PumpStack(stackOldestOnTop, stackNewestOnTop);
            stackNewestOnTop.Push(value);
        }

        public T Peek()
        {
            PumpStack(stackNewestOnTop, stackOldestOnTop);
            return stackOldestOnTop.Peek();
        }

        public T Dequeue()
        {
            PumpStack(stackNewestOnTop, stackOldestOnTop);
            return stackOldestOnTop.Pop();
        }

        private static void PumpStack(Stack<T> source, Stack<T> target)
        {
            while (source.Count > 0)
                target.Push(source.Pop());
        }
    }
}