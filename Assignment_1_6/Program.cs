using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Assignment_1_5
{
    class Program
    {
        const int BUFFER_SIZE = 10;

        const int MIN_NUMBER = 1;
        const int MAX_NUMBER = 20;

        const int PRODUCER_DELAY = 500;
        const int CONSUMER_DELAY = 1000;

        static void Main()
        {
            BlockingCollection<int> buffer = new BlockingCollection<int>(BUFFER_SIZE);

            Thread producer1 = new Thread(() => Produce(buffer, MIN_NUMBER, MAX_NUMBER, PRODUCER_DELAY));
            Thread producer2 = new Thread(() => Produce(buffer, MIN_NUMBER, MAX_NUMBER, PRODUCER_DELAY));
 
            Thread consumer1 = new Thread(() => Consume(buffer, CONSUMER_DELAY));
            Thread consumer2 = new Thread(() => Consume(buffer, CONSUMER_DELAY));
            
            producer1.Start();
            producer2.Start();
            consumer1.Start();
            consumer2.Start();

            producer1.Join();
            producer2.Join();
            buffer.CompleteAdding();

            consumer1.Join();
            consumer2.Join();

            Console.WriteLine("All Done");
        }

        public static void Produce(BlockingCollection<int> buffer, int minimumValue, int maximumValue, int delayInMilliSeconds)
        {
            for (int item = minimumValue; item <= maximumValue; item++)
            {
                buffer.Add(item);
                Thread.Sleep(delayInMilliSeconds);
            }
            
            Console.WriteLine("Producer Done");
        }

        public static void Consume(BlockingCollection<int> buffer, int delayInMilliSeconds)
        {
            foreach(int number in buffer.GetConsumingEnumerable())
            {
                Console.WriteLine(number);
                Thread.Sleep(delayInMilliSeconds);
            }

            Console.WriteLine("Consumer Done");
        }
    }
}

