namespace _02_Threds
{
    struct RangeTwoNumbers
    {
        public int Start { get; set; }
        public int End { get; set; }
        public int? Threads { get; set; }
        public RangeTwoNumbers(int start, int end, int? threads = null)
        {
            Start = start;
            End = end;
            Threads = threads;
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            ////1
            //Thread thread = new Thread(ShowRange);
            //thread.Start();
            ////2
            //RangeTwoNumbers range = new RangeTwoNumbers(10, 70);
            //Thread thread2 = new Thread(ShowRange);
            //thread2.Start(range);
            ////3
            //RangeTwoNumbers range2 = new RangeTwoNumbers(start: 10, end: 70, threads: 5);
            //RangeManager(range2);
            //4
            //int[] ints = Get10000Numbers(new(start: 10, end: 500));
            //Thread thread3 = new Thread((object obj) => Console.WriteLine($"Min = {((int[])obj).Min()}"));
            //Thread thread4 = new Thread((object obj) => Console.WriteLine($"Max = {((int[])obj).Max()}"));
            //Thread thread5 = new Thread((object obj) => Console.WriteLine($"Average = {((int[])obj).Average()}"));
            //thread3.Start(ints);
            //thread4.Start(ints);
            //thread5.Start(ints);
            //5
            int[] ints = Get10000Numbers(new(start: 10, end: 500));
            Thread thread6 = new Thread((object obj) =>
            {
                int min = 0;
                int max = 0;
                double avg = 0;
                Thread thread3 = new Thread((object obj) =>
                {
                    min = ((int[])obj).Min();
                });
                Thread thread4 = new Thread((object obj) =>
                {
                    max = ((int[])obj).Max();
                });
                Thread thread5 = new Thread((object obj) =>
                {
                    avg = ((int[])obj).Average();
                });
                thread3.Start(ints);
                thread4.Start(ints);
                thread5.Start(ints);
                //Write to File
                string path = "note1.txt";
                string text = string.Empty;
                foreach (int i in ints)
                {
                    text += $"{i}\n";
                }
                text += $"min = {min}\nmax = {max}\naverage = {avg}";
                using (StreamWriter writer = new StreamWriter(path, false))
                {
                    writer.WriteLine(text);
                }
            });
            thread6.Start(ints);
        }
        static int[] Get10000Numbers(RangeTwoNumbers? range = null)
        {
            Random rnd = new Random();
            int start = (range == null ? 0 : range.Value.Start);
            int end = (range == null ? 100 : range.Value.End);
            int[] numbers = new int[10000];
            for (int i = 0; i < numbers.Length; i++)
            {
                numbers[i] = rnd.Next(start, end);
                //Console.WriteLine(numbers[i]);
            }
            return numbers;
        }

        //static object ShowMinInArray(object obj)
        //{
        //    Console.WriteLine($"Min = {((int[])obj).Min()}");
           
        //}
        //static object ShowMaxInArray(object obj)
        //{
        //    Console.WriteLine($"Max = {((int[])obj).Max()}");
        //}
        //static object ShowAvgInArray(object obj)
        //{
        //    Console.WriteLine($"Average = {((int[])obj).Average()}");
        //}
        static void RangeManager(RangeTwoNumbers range)
        {
            if (1 < range.Threads && range.Threads < range.End - range.Start)
            {
                float? segment = (range.End - (float)range.Start) / range.Threads;
                for (int i = 0; i < range.Threads; i++)
                {
                    Thread thread = new Thread(ShowRange);
                    thread.Start(new RangeTwoNumbers(start: (int)(range.Start + (segment * i)), end: (int)(range.End - ((range.Threads - i - 1) * segment)) - (i == range.Threads - 1 ? 0 : 1)));
                }
            }
        }
        static void ShowRange(object obj)
        {
            Console.WriteLine("Starting new thread");
            int start = 0;
            int end = 50;
            if (obj is RangeTwoNumbers range)
            {
                start = range.Start;
                end = range.End;
                if (end < start)
                {
                    int tmp = end;
                    end = start;
                    start = tmp;
                }
            }
            for (int i = start; i <= end; i++)
            {
                Console.WriteLine(i);
            }
        }
    }
}