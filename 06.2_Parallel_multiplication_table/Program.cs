namespace _06._2_Parallel_multiplication_table
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Parallel.For(3, 8, CreateTable);

        }
        static void CreateTable(int num)
        {
            string res = string.Empty;
            for (int i = 1; i <= 10; i++)
            {
                string tmp = $"{num} * {i} = {num * i}";
                //Console.WriteLine(tmp);
                res += tmp + "\n";
            }
            using (StreamWriter writer = new StreamWriter($"result{num}.txt", false))
            {
                writer.WriteLine(res);
            }
        }
    }
}