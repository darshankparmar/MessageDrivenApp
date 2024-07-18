using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading;

namespace MessageDrivenApp
{
    public class Consumer
    {
        private readonly MessageQueue _queue;
        private readonly ILogger<Consumer> _logger;
        private bool _running;
        private int _successCount;
        private int _errorCount;

        public int SuccessCount => _successCount;
        public int ErrorCount => _errorCount;

        public Func<string, bool>? ProcessMessageWithErrorSimulation { get; set; }

        public Consumer(MessageQueue queue, ILogger<Consumer> logger)
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
                try
                {
                    if (_queue.TryDequeue(out string message))
                    {
                        try
                        {
                            if (ProcessMessageWithErrorSimulation != null && !ProcessMessageWithErrorSimulation(message))
                            {
                                throw new Exception("Simulated processing error");
                            }

                            _logger.LogInformation($"Consumed: {message}");
                            Interlocked.Increment(ref _successCount);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"Error processing message: {message}");
                            Interlocked.Increment(ref _errorCount);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error retriveing message from queue");
                    Interlocked.Increment(ref _errorCount);
                }
                Thread.Sleep(Constants.threadSleepTimeOutForMessage);
            }
        }
    }
}
