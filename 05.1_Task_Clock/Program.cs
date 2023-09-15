namespace _05._1_Task_Clock
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Task task1 = Task.Run(() => ShowCurrentDateTime());
            Task task2 = new Task(() => ShowCurrentDateTime());
            task2.Start();
            Task task3 = Task.Factory.StartNew(() => ShowCurrentDateTime());
            Task.WaitAll(task1, task2, task3);
        }
        static void ShowCurrentDateTime()
        {
            Console.WriteLine(DateTime.Now.ToString());
        }
    }
}