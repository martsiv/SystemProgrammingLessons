namespace _05._3_Task_Array_Methods
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Task 4
            int[] arr = { 2, 7, 2, 9, -2, 6, 13, 6, 14, 10, 13 };
            Task task1 = Task.Run(() => { Console.WriteLine("Min = " + arr.Min()); });
            Task task2 = Task.Run(() => { Console.WriteLine("Max = " + arr.Max()); });
            Task task3 = Task.Run(() => { Console.WriteLine("Average = " + arr.Average()); });
            Task task4 = Task.Run(() => { Console.WriteLine("Summ = " + arr.Sum()); });
            Task.WaitAll(task1, task2, task3, task4);

            //Task5
            Task<int[]> task5 = new Task<int[]>(() => arr.Distinct().ToArray());

            
        }
    }
}