namespace _02_Threds
{
    struct RangeToShow
    {
        public int Start { get; set; }
        public int End { get; set; }
        public int? Threads { get; set; }
        public RangeToShow(int start, int end, int? threads = null)
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
            //Thread thread = new Thread(Show50);
            //thread.Start();
            ////2
            //RangeToShow range = new RangeToShow(10, 70);
            //Thread thread2 = new Thread(Show50);
            //thread2.Start(range);
            //3
            RangeToShow range2 = new RangeToShow(start: 10, end: 70, threads: 5);
            Thread thread3 = new Thread(Show50);
            thread3.Start(range2);
        }
        
        static void Show50(object obj)
        {
            Console.WriteLine("Starting new thread");
            int start = 0;
            int end = 50;
            int threads = 1;
            if (obj is RangeToShow range)
            {
                start = range.Start;
                end = range.End;
                threads = range.Threads ?? 1;
            }
            if (1 < threads)
            {
                for (int i = start; i < (end - start) / threads + start; i++)
                    Console.WriteLine(i);
                int[] ranges = new int[threads];
                for (int i = 1; i < threads - 1; i++)
                {
                    ranges[i] = (end - start) / threads * i + start;
                }

                for (int i = 0; i < ranges.Length - 1; i++)
                {
                    RangeToShow rangeTmp = new RangeToShow(ranges[i] + 1, ranges[i + 1]);
                    Thread threadTmp = new Thread(Show50);
                    threadTmp.Start(rangeTmp);

                }
                RangeToShow rangeTmp1 = new RangeToShow(ranges[ranges.Length], end);
                Thread threadTmp1 = new Thread(Show50);
                threadTmp1.Start(rangeTmp1);
            }
            else
            {
                for (int i = start; i <= end; i++)
                {
                    Console.WriteLine(i);
                }
            }
        }
    }
}