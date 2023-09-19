using PropertyChanged;
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

namespace _07._1_AsyncAwait_FactorialOfNumber
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        public ObservableCollection<FactorialOfNumber> Factorials { get; set; }
        public MainWindow()
        {
            Factorials = new ObservableCollection<FactorialOfNumber>();
            InitializeComponent();
            factorialsList.ItemsSource = Factorials;

        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            FactorialOfNumber factorialOfNumber = new(int.Parse(numberToFactorial.Text));
            Factorials.Add(factorialOfNumber);
            await factorialOfNumber.CalculateAsync();
        }
    }
    [AddINotifyPropertyChangedInterface]
    public class FactorialOfNumber
    {
        public int MainNumber { get; set; }
        public long FactorialNum { get; set; }
        public FactorialOfNumber(int number)
        {
            MainNumber = number;
        }
        public async Task CalculateAsync()
        {
            long factorial = 1;
            for (int i = 1; i <= MainNumber; i++)
            {
                factorial *= i;
            }
            await Task.Delay(2000);
            FactorialNum = factorial;
        }
    }
}
