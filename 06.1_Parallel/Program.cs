using System.Text;

namespace _06._1_Parallel
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            //Parallel.For(1, 5, Factorial);
            //Console.ReadLine();

            string filePath = "numbers.txt"; // Шлях до файлу з числами
            List<int> numbers = ReadNumbersFromFile(filePath);

            Console.WriteLine("Список чисел з файлу:");
            foreach (int number in numbers)
            {
                Console.WriteLine(number);
            }
            Console.ReadLine();
            ParallelLoopResult result = Parallel.ForEach<int>(numbers, Factorial);

            Console.ReadLine();
            //int sum = numbers.AsParallel().Sum();
            //int min = numbers.AsParallel().Min();
            //int max = numbers.AsParallel().Max();

            var res = numbers.AsParallel().Aggregate(new { summ = 0, min = int.MaxValue, max = int.MinValue },
                            (res, curr) =>
                                new
                                {
                                    summ = res.summ + curr,
                                    min = curr < res.min ? curr : res.min,
                                    max = curr > res.max ? curr : res.max
                                });
            Console.WriteLine($"sum = {res.summ}");
            Console.WriteLine($"min = {res.min}");
            Console.WriteLine($"max = {res.max}");
        }


        static void Factorial(int x)
        {
            long result = 1;

            for (int i = 1; i <= x; i++)
            {
                result *= i;
            }
            Console.WriteLine($"Task executing {Task.CurrentId}");
            //Thread.Sleep(3000);
            Console.WriteLine($"Result {result}");
            Parallel.Invoke(() => CountOfNumber(result), () => SummOfDigit(result));
        }
        static void CountOfNumber(long num)
        {
            int count = 1;
            while (0 < num / 10) 
            {
                num /= 10;
                ++count;
            }
            Console.WriteLine($"In number {num} {count} digit");
        }
        static void SummOfDigit(long num)
        {
            long oldNum = num;
            long sum = 0;

            while (num != 0)
            {
                long digit = num % 10;
                sum += digit;
                num /= 10;
            }
            Console.WriteLine($"Summ of digits in number {oldNum} = {sum}");
        }
        static List<int> ReadNumbersFromFile(string filePath)
        {
            List<int> numbers = new List<int>();

            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] strNumbers = line.Split(' ');
                        foreach (string numStr in strNumbers)
                        {
                            if (int.TryParse(numStr, out int number))
                            {
                                numbers.Add(number);
                            }
                            else
                            {
                                Console.WriteLine($"Помилка парсингу рядка: {numStr}");
                            }
                        }
                    }
                }
            }
            catch (IOException e)
            {
                Console.WriteLine($"Помилка читання файлу: {e.Message}");
            }

            return numbers;
        }
    }
}