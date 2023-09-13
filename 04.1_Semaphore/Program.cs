namespace _04._1_Semaphore
{
    internal class Program
    {
        static Random _random = new Random();
        static void Main(string[] args)
        {
            Semaphore s = new Semaphore(3, 3);
            for (int i = 0; i < 10; ++i)
                ThreadPool.QueueUserWorkItem(ShowRandomNumbers, s);
            Console.ReadKey();
        }
        static void ShowRandomNumbers(object obj)
        {
            Semaphore s = obj as Semaphore;

            bool stop = false;
            while (!stop)
            {
                if (s.WaitOne()) // block one state
                {
                    try
                    {
                        Console.WriteLine("Thread {0} got a lock", Thread.CurrentThread.ManagedThreadId);
                        for (int i = 0; i < 10; i++)
                        {
                            Console.WriteLine(_random.Next(0, 100));
                            Thread.Sleep(400);
                        }
                    }
                    finally
                    {
                        stop = true;
                        Console.WriteLine("Thread {0} remove a lock", Thread.CurrentThread.ManagedThreadId);
                        s.Release();
                    }
                }
                else
                    Console.WriteLine("Timeout for thread {0} expired. Re-waiting...", Thread.CurrentThread.ManagedThreadId);
            }
        }
    }
}