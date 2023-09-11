using _01_Processes.View;
using Microsoft.Win32;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;
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
using System.Windows.Threading;

namespace _01_Processes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
  [AddINotifyPropertyChangedInterface]

    public partial class MainWindow : Window
    {
        DispatcherTimer _timer = null;
        IList<ProcessInfo> processes = null;
        public ObservableCollection<string> RefreshTime { get; set; }
        ProcessInfo? SelectedProcess { get; set; } = null;
        public MainWindow()
        {
            RefreshTime = new() { "Manual", "1s", "2s", "5s" };
            InitializeComponent();
            ComboBoxRefreshTime.ItemsSource = RefreshTime;
            ComboBoxRefreshTime.SelectedIndex = 0;

            InitialLoad();
            grid.ItemsSource = processes;

            //_timer = new DispatcherTimer();
            //_timer.Interval = new TimeSpan(0, 0, 0);
            //_timer.Tick += _timer_Tick;
            //_timer.Start();
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            RefreshList();
        }

        public void InitialLoad()
        {
            var result = Process.GetProcesses().Select(p => new ProcessInfo(p));
            processes = new ObservableCollection<ProcessInfo>(result);
        }
        public void RefreshList()
        {
            var currentList = Process.GetProcesses().Select(p => new ProcessInfo(p));

            foreach (var pr in currentList)
            {
                var item = processes.FirstOrDefault(p => p.Id == pr.Id);
                if (item != null)
                {
                    item.SetFields(pr);
                }
                else
                {
                    processes.Add(pr);
                }
            }
        }

        private void Button_Click_Refresh(object sender, RoutedEventArgs e)
        {
            RefreshList();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int sec = 0;
            if (sender is ComboBox combo)
            {
                sec = combo.SelectedIndex == 3 ? 5 : combo.SelectedIndex;
            }
            if (sec <= 0 && _timer != null)
            {
                _timer.Tick -= _timer_Tick;
                _timer = null;
            }
            else if (0 < sec )
            {
                _timer = new DispatcherTimer();
                _timer.Interval = new TimeSpan(0, 0, sec);
                _timer.Tick += _timer_Tick;
                _timer.Start();
            }
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            SelectedProcess.FullProcess.CloseMainWindow();
            processes.Remove(SelectedProcess);
        }

        private void ButtonFullInfo_Click(object sender, RoutedEventArgs e)
        {
            FullInfoWindow newWindow = new FullInfoWindow();
            newWindow.DataGrid.ItemsSource = Process.GetProcesses().Where(x => x.Id == SelectedProcess?.Id);
            newWindow.Show();
        }
        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                Process.Start(openFileDialog.FileName);
        }
        private void grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is DataGrid dataGrid)
            {
                SelectedProcess = dataGrid.SelectedItem as ProcessInfo;
            }
        }
    }

    [AddINotifyPropertyChangedInterface]
    class ProcessInfo
    {
        public int Id { get; set; }
        public string ProcessName { get; set; }
        public TimeSpan TotalProcessorTime { get; set; }
        public ProcessPriorityClass PriorityClass { get; set; }
        public string UserName { get; set; }
        public Process FullProcess { get; set; }

        public ProcessInfo(Process pr)
        {
            FullProcess = pr;
            Id = pr.Id;
            try { ProcessName = pr.ProcessName; } catch { }
            try { TotalProcessorTime = pr.TotalProcessorTime; } catch { }
            try { PriorityClass = pr.PriorityClass; } catch { }
            UserName = "Null"; //UserName = GetProcessOwner(pr.Id);
        }

        public void SetFields(ProcessInfo pr)
        {
            this.ProcessName = pr.ProcessName;
            this.TotalProcessorTime = pr.TotalProcessorTime;
            this.PriorityClass = pr.PriorityClass;
        }

        public string GetProcessOwner(int processId)
        {
            string query = "Select * From Win32_Process Where ProcessID = " + processId;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            ManagementObjectCollection processList = searcher.Get();

            foreach (ManagementObject obj in processList)
            {
                string[] argList = new string[] { string.Empty, string.Empty };
                int returnVal = Convert.ToInt32(obj.InvokeMethod("GetOwner", argList));
                if (returnVal == 0)
                {
                    // return DOMAIN\user
                    return argList[0];
                }
            }

            return "NO OWNER";
        }
    }
}

