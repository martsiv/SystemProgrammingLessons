using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _02._1_ThredsWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            TextBoxLeftRange.TextChanged += TextBox_TextChanged;
            TextBoxRightRange.TextChanged += TextBox_TextChanged;
            TextBoxThreads.TextChanged += TextBox_TextChanged;
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Перевірте, чи дані у TextBox є числами
            if (int.TryParse(TextBoxLeftRange.Text, out int leftValue) &&
                int.TryParse(TextBoxRightRange.Text, out int rightValue))
            {
                // Якщо обидва поля містять числа, активуйте кнопку
                ButtonGenerateRange.IsEnabled = true;
            }
            else
            {
                // Якщо хоча б одне поле не містить число, деактивуйте кнопку
                ButtonGenerateRange.IsEnabled = false;
            }
        }
        private void ButtonGenerateRange_Click(object sender, RoutedEventArgs e)
        {
            RangeTwoNumbers range = new RangeTwoNumbers(start: int.Parse(TextBoxLeftRange.Text), end: int.Parse(TextBoxRightRange.Text), threads: int.Parse(TextBoxThreads.Text));
            RangeManager(range);
            //Show змінити на вивід в змінну або в ліст
        }

        private void Button10000Numbers_Click(object sender, RoutedEventArgs e)
        {

        }
        static void RangeManager(RangeTwoNumbers range)
        {
            if (range.Threads != null && 1 < range.Threads  && range.Threads < range.End - range.Start)
            {
                float? segment = (range.End - (float)range.Start) / range.Threads;
                for (int i = 0; i < range.Threads; i++)
                {
                    Thread thread = new Thread(ShowRange);
                    thread.Start(new RangeTwoNumbers(start: (int)(range.Start + (segment * i)), end: (int)(range.End - ((range.Threads - i - 1) * segment)) - (i == range.Threads - 1 ? 0 : 1)));
                }
            }
            else
            {
                Thread thread = new Thread(ShowRange);
                thread.Start(range);
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
    
}
