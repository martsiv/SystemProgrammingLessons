using System;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace _05._2_Task_Prime_numbers
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter start range number ");
            int start = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter end range number ");
            int end = int.Parse(Console.ReadLine());
            Task<int> showPrimeNumbersTask = new Task<int>(() => ShowPrimeNumbers(start, end));
            showPrimeNumbersTask.Start();
            Console.WriteLine("Total prime numbers: " + showPrimeNumbersTask.Result.ToString());
            showPrimeNumbersTask.Wait();
        }
        static int ShowPrimeNumbers(int start, int end)
        {
            int counter = 0;
            for(int i = start; i <= end; i++) 
            {
                Task<bool> isPrimeTask = new Task<bool>(() => Is_prime(i));
                Task showTask = isPrimeTask.ContinueWith(task =>
                {
                    if (isPrimeTask.Result == true)
                    {
                        Console.WriteLine(i);
                        ++counter;
                    }
                });
                isPrimeTask.Start();
                showTask.Wait();
            }
            return counter;
        }
        static bool Is_prime(int num)
        {
            if (num <= 3)
                return num > 1;
            else if (num % 2 == 0 || num % 3 == 0)
                return false;
            else
            {
                for (int i = 5; i * i <= num; i += 6)
                {
                    if (num % i == 0 || num % (i + 2) == 0)
                        return false;
                }
                return true;
            }
        }
    }
}