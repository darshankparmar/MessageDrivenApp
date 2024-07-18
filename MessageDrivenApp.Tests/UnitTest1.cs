using NUnit.Framework;
using Moq;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace MessageDrivenApp.Tests
{
    [TestFixture]
    public class ProducerConsumerTests
    {
        [Test]
        public void ProducerShouldAddMessagesToQueue()
        {
            var queue = new MessageQueue();
            var loggerMock = new Mock<ILogger<Producer>>();
            var producer = new Producer(queue, loggerMock.Object);

            producer.Start();
            Thread.Sleep(3000);
            producer.Stop();

            Assert.IsTrue(queue.TryDequeue(out _));
        }

        [Test]
        public void ConsumerShouldProcessMessagesFromQueue()
        {
            var queue = new MessageQueue();
            var loggerMock = new Mock<ILogger<Consumer>>();
            var consumer = new Consumer(queue, loggerMock.Object);

            queue.Enqueue("Test Message");
            consumer.Start();
            Thread.Sleep(1000);
            consumer.Stop();

            Assert.That(consumer.SuccessCount, Is.EqualTo(1));
        }

        [Test]
        public void ConsumerShouldHandleErrors()
        {
            var queue = new MessageQueue();
            var loggerMock = new Mock<ILogger<Consumer>>();
            var consumer = new Consumer(queue, loggerMock.Object);

            queue.Enqueue("Test Message");

            // Simulate an error in message processing by causing an exception
            consumer.ProcessMessageWithErrorSimulation = (message) => false;

            consumer.Start();
            Thread.Sleep(1000);
            consumer.Stop();

            Assert.That(consumer.ErrorCount, Is.EqualTo(1));
        }
    }
}
