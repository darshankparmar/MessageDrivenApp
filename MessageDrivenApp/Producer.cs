using Microsoft.Extensions.Logging;
using System.Threading;
using System;

namespace MessageDrivenApp
{
    public class Producer
    {
        private readonly MessageQueue _queue;
        private readonly ILogger<Producer> _logger;
        private readonly Random _random = new Random();
        private bool _running;

        private int _messageCount;

        public int MessageCount => _messageCount;

        public Producer(MessageQueue queue, ILogger<Producer> logger)
        {
            _queue = queue;
            _logger = logger;
        }

        public void Start()
        {
            _running = true;
            new Thread(Run).Start();
        }

        public void Stop()
        {
            _running = false;
        }

        private void Run()
        {
            while (_running)
            {
                string message = $"Secure Message {_random.Next(1000)}";
                _queue.Enqueue(message);
                Interlocked.Increment(ref _messageCount);
                _logger.LogInformation($"Produced: {message}");
                Thread.Sleep(Constants.threadSleepTimeOutForMessage);
            }
        }
    }
}
