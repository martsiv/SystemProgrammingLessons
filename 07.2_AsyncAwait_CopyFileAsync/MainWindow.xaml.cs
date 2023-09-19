using IOExtensions;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MessageBox = System.Windows.MessageBox;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;
using Path = System.IO.Path;

namespace _07._2_AsyncAwait_CopyFileAsync
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<CopyFileInfo> operations;
        CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
        CancellationToken token;
        bool isDisposedToken = false;

        public MainWindow()
        {
            InitializeComponent();

            operations = new ObservableCollection<CopyFileInfo>();
            // даємо можливість "біндиться" до параметрів операції копіювання
            operationsList.ItemsSource = operations;
        }
        private async void Window_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                CopyFile();
        }

        // запуск копіювання файла
        private async void ButtonCopy_Click(object sender, RoutedEventArgs e)
        {
            CopyFile();
        }

        int c = 10;
        private async Task CopyFile()
        {
            cancelTokenSource = new CancellationTokenSource();
            token = cancelTokenSource.Token;
            isDisposedToken = false;
            CopyFileInfo info = new CopyFileInfo()
            {
                // встановлюємо дані для копіювання
                SourceFile = filePathTB.Text,
                DestFolder = folderTarget.Text
            };
            int howManyCopies;
            if (int.TryParse(NumberOfCopy.Text, out howManyCopies) == false)
                howManyCopies = 1;
            try
            {
                for (int i = 0; i < howManyCopies; i++)
                {
                    // формуємо ім'я файла копії - name_copy_mmss.extension
                    string copyFileName = $"{Path.GetFileNameWithoutExtension(info.SourceFile)}_copy_{c++}{Path.GetExtension(info.SourceFile)}";
                    info.NewName = copyFileName;
                    operations.Add(info);
                    // формуємо шлях до файла копії
                    string destFile = Path.Combine(info.DestFolder, copyFileName);

                    // 1 - manual
                    await Task.Delay(2000);
                    // 3 - using FileTransferManager
                    await FileTransferManager.CopyWithProgressAsync(info.SourceFile, destFile, (obj) =>
                    {
                        if (obj.Percentage >= 1)
                            info.Progress = (int)obj.Percentage;
                    }, false, token);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally 
            { 
                cancelTokenSource.Dispose();
                isDisposedToken = true;
            }
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            
            if (isDisposedToken == false && !cancelTokenSource.IsCancellationRequested)
                cancelTokenSource.Cancel();
        }

        private void OpenFileButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    filePathTB.Text = dialog.FileName;
            }
        }
        private void OpenFolderButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    folderTarget.Text = dialog.SelectedPath;
            }
        }
    }
}
