using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace _03_ThreadPool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<FactorialOfNumber> factorials = new();
        public MainWindow()
        {
            InitializeComponent();
            factorialsList.ItemsSource = factorials;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FactorialOfNumber factorialOfNumber = new(int.Parse(numberToFactorial.Text));
            factorials.Add(factorialOfNumber);
            ThreadPool.QueueUserWorkItem((object obj) =>
            {
                factorialOfNumber.Calculate();
            });

        }
    }
    public class FactorialOfNumber
    {
        public int MainNumber { get; set; }
        public long FactorialNum { get; set; }
        public FactorialOfNumber(int number)
        {
            MainNumber = number;
            //long factorial = 1;
            //for (int i = 1; i <= MainNumber; i++)
            //{
            //    factorial *= i;
            //}
            //FactorialNum = factorial;
        }
        public void Calculate()
        {
            long factorial = 1;
            for (int i = 1; i <= MainNumber; i++)
            {
                factorial *= i;
            }
            FactorialNum = factorial;
        }
    }
}
