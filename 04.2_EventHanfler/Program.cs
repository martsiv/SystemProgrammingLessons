using static System.Net.Mime.MediaTypeNames;
using System.IO;

namespace _04._2_EventHanfler
{
    internal class Program
    {
        static AutoResetEvent waitHandler = new AutoResetEvent(true);

        static void Main(string[] args)
        {
            MyClass myClass = new MyClass();
            Thread myThreadSaveNumbers = new(myClass.SaveNumbers);
            myThreadSaveNumbers.Name = $"Thread save pairs";
            myThreadSaveNumbers.Start(waitHandler);

            Thread myThreadSaveSumm = new(myClass.SaveSumm);
            myThreadSaveSumm.Name = $"Thread save summ";
            myThreadSaveSumm.Start(waitHandler);

            Thread myThreadSaveMult = new(myClass.SaveMult);
            myThreadSaveMult.Name = $"Thread save mult";
            myThreadSaveMult.Start(waitHandler);

            //myClass.SaveNumbers(new Object());
            //myClass.SaveSumm(new Object());
            //myClass.SaveMult(new Object());
        }
    }
    public class MyClass
    {
        string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
        private string _filenameNumbers = "pairsOfNumbers.txt";
        private string _filenameSumm = "summ.txt";
        private string _filenameMult = "mult.txt";
        private Random _random = new Random();
        public void SaveNumbers(object obj)
        {
            Thread.Sleep(5000);
            EventWaitHandle ev = obj as EventWaitHandle;
            if (ev.WaitOne()) // wait
            {
                Console.WriteLine($"{Thread.CurrentThread.Name}");

                Console.WriteLine("Started generating numbers");
                string text = "";
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        int tmp = _random.Next(0, 99);
                        Console.Write(tmp + " ");
                        text += tmp;
                        text += " ";
                    }
                    Console.WriteLine();
                    text += "\n";
                }
                using (StreamWriter writer = new StreamWriter(Path.Combine(projectDirectory, _filenameNumbers), false))
                {
                    writer.WriteLine(text);
                }
                Console.WriteLine("Pairs saved!");
                ev.Set();
            }
            else
            {
                Console.WriteLine("Thread {0} late", Thread.CurrentThread.ManagedThreadId);
            }
        }
        public void SaveSumm(object obj)
        {
            EventWaitHandle ev = obj as EventWaitHandle;
            if (ev.WaitOne()) // wait
            {
                Console.WriteLine($"{Thread.CurrentThread.Name}");

                Console.WriteLine("Started calculating summ");
                string text = File.ReadAllText(Path.Combine(projectDirectory, _filenameNumbers));

                string newText = "";
                string[] wordArray = text.Split(new char[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                int[] numbers = new int[wordArray.Length];
                for (int i = 0; i < wordArray.Length; i++)
                    numbers[i] = int.Parse(wordArray[i]);
                for (int i = 0; i < numbers.Length; i = i + 2)
                {
                    int tmp = numbers[i] + numbers[i + 1];
                    Console.Write(tmp + " ");
                    newText += tmp + " ";
                }
                using (StreamWriter writer = new StreamWriter(Path.Combine(projectDirectory, _filenameSumm), false))
                {
                    writer.WriteLine(newText);
                }
                Console.WriteLine("Summ saved!");
                ev.Set();
            }
            else
            {
                Console.WriteLine("Thread {0} late", Thread.CurrentThread.ManagedThreadId);
            }
        }
        public void SaveMult(object obj)
        {
            EventWaitHandle ev = obj as EventWaitHandle;
            if (ev.WaitOne()) // wait
            {
                Console.WriteLine($"{Thread.CurrentThread.Name}");

                Console.WriteLine("Started calculating mult");
                string text = File.ReadAllText(Path.Combine(projectDirectory, _filenameNumbers));

                string newText = "";
                string[] wordArray = text.Split(new char[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                int[] numbers = new int[wordArray.Length];
                for (int i = 0; i < wordArray.Length; i++)
                    numbers[i] = int.Parse(wordArray[i]);
                for (int i = 0; i < numbers.Length; i = i + 2)
                {
                    int tmp = numbers[i] * numbers[i + 1];
                    Console.Write(tmp + " ");
                    newText += tmp + " ";
                }
                using (StreamWriter writer = new StreamWriter(Path.Combine(projectDirectory, _filenameMult), false))
                {
                    writer.WriteLine(newText);
                }
                Console.WriteLine("Mult saved!");
                ev.Set();
            }
            else
            {
                Console.WriteLine("Thread {0} late", Thread.CurrentThread.ManagedThreadId);
            }
        }
    }
}