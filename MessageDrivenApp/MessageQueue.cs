using System.Collections.Concurrent;

namespace MessageDrivenApp
{
    public class MessageQueue
    {
        private readonly ConcurrentQueue<string> _queue = new ConcurrentQueue<string>();

        public void Enqueue(string message)
        {
            _queue.Enqueue(message);
        }

        public bool TryDequeue(out string message)
        {
            return _queue.TryDequeue(out message);
        }
    }
}
