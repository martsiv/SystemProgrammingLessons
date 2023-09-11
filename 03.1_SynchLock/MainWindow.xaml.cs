using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using System.Net.Sockets;
using PropertyChanged;
using Binding = System.Windows.Data.Binding;
using static System.Resources.ResXFileRef;
using Application = System.Windows.Application;

namespace _03._1_SynchLock
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    [AddINotifyPropertyChangedInterface]
    public partial class MainWindow : Window
    {
        string FolderPath { get; set; }
        Stat stat = new Stat();
        public Stat Stata => stat;
        public MainWindow()
        {
            InitializeComponent();
            Binding binding = new Binding();

            binding.ElementName = "Stata"; 
            binding.Path = new PropertyPath("Words");
            WordsTextBlock.SetBinding(TextBlock.TextProperty, binding);
        }
        private void ButtonCalculate_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    FolderPath = dialog.SelectedPath;
                    string[] files = Directory.GetFiles(FolderPath, "*.txt");

                    foreach (var file in files)
                    {
                        //FileStream fs = new FileStream(file, FileMode.Open);
                        //fs.Read

                        //StreamReader sr = new StreamReader(file);
                        //sr.ReadToEnd();

                        string text = File.ReadAllText(file);
                        TextAnalyseWords(text);
                        TextAnalyseRows(text);
                        TextAnalyseLetters(text);
                        //Thread thread = new Thread(TextAnalyseWords);
                        //thread.Start(text);
                        //Thread thread1 = new Thread(TextAnalyseRows);
                        //thread1.Start(text);
                        //Thread thread3 = new Thread(TextAnalyseLetters);
                        //thread3.Start(text);
                        //Task.Run(() => TextAnalyse(text, statistic));
                    }
                }
            }
        }
        private void TextAnalyseWords(object stat)
        {
            lock (new object())
            {
                if (stat is string text && !string.IsNullOrEmpty(text))
                {
                    string[] wordArray = text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    this.stat.Words += wordArray.Length;
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        Binding binding = new Binding("Stata");

                        //binding.ElementName = "Stata";
                        binding.Path = new PropertyPath("Words");
                        WordsTextBlock.SetBinding(TextBlock.TextProperty, binding);
                    }));
                    
                }
            }

        }
        private void TextAnalyseRows(object stat)
        {
            lock (new object())
            {
                if (stat is string text && !string.IsNullOrEmpty(text))
                {
                    string[] rowArray = text.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                    this.stat.Lines += rowArray.Length;
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        Binding binding = new Binding();

                        binding.ElementName = "Stata";
                        binding.Path = new PropertyPath("Lines");
                        RowsTextBlock.SetBinding(TextBlock.TextProperty, binding);
                    }));
                }
            }
        }
        private void TextAnalyseLetters(object stat)
        {
            lock (new object())
            {
                if (stat is string text && !string.IsNullOrEmpty(text))
                {
                    string separators = ". , ; : – — ‒ … ! ? \"\" '' «» () {} [] <> /";

                    char[] separatorArray = separators.ToCharArray();

                    foreach (char c in text)
                    {
                        if (separatorArray.Contains(c))
                        {
                            this.stat.Punctuation++;
                            Application.Current.Dispatcher.Invoke(new Action(() =>
                            {
                                Binding binding = new Binding();

                                binding.ElementName = "Stata";
                                binding.Path = new PropertyPath("Punctuation");
                                SymbolsTextBlock.SetBinding(TextBlock.TextProperty, binding);
                            }));
                        }
                    }
                }
            }
        }
    }
    public class Stat
    {
        public int Words { get; set; }
        public int Lines { get; set; }
        public int Punctuation { get; set; }
        public Stat()
        {
            Words = 0;
            Lines = 0;
            Punctuation = 0;
        }
    }

}
