using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MessageDrivenApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
            .AddLogging(configure => configure.AddConsole())
            .BuildServiceProvider();

            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            var loggerProducer = loggerFactory.CreateLogger<Producer>();
            var loggerConsumer = loggerFactory.CreateLogger<Consumer>();

            var messageQueue = new MessageQueue();
            var producer = new Producer(messageQueue, loggerProducer);
            var producer1 = new Producer(messageQueue, loggerProducer);
            var consumer = new Consumer(messageQueue, loggerConsumer);

            producer.Start();
            Thread.Sleep(1000);
            consumer.Start();

            Console.WriteLine("Press any key to stop...");
            Console.ReadKey();

            producer.Stop();
            Thread.Sleep(1000);
            consumer.Stop();

            Thread.Sleep(1000);

            Console.WriteLine("Total Generated Message (By Producer) Count : " + producer.MessageCount);
            Console.WriteLine("Total Recieved Message (By Consumer) Count : " + consumer.SuccessCount);
            Console.WriteLine("Total Error Count : " + consumer.ErrorCount);

            Console.Read();
        }
    }
}
