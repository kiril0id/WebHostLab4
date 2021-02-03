using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
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

namespace ClienWpf
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static HttpClient client = new HttpClient();
        static List<string> filelist = new List<string>();
        static readonly string localpath = "https://localhost:44306/";
        public MainWindow()
        {
            InitializeComponent();

            client.BaseAddress = new Uri(localpath);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

        }

        async private void ShowText(object sender, RoutedEventArgs e)
        {
            string text = await GetProductAsync($"api/file");
            Regex regex = new Regex("\\\"(.*?)\\\"");
            MatchCollection matches = regex.Matches(text);
            if (matches.Count > 0)
            {
                foreach (Match match in matches)
                {
                    filelist.Add(match.Value);
                }
                listbox.ItemsSource = filelist;
            }
            else
            {
                MessageBox.Show("Файлы не найдены", "Ошибка");
            }
        }

        private void DowlandFile(object sender, RoutedEventArgs e)
        {
            if (listbox.SelectedIndex == -1)
            {
                MessageBox.Show("Файл не выбран", "Ошибка");
            }
            else
            {
                string pathDowland = localpath + $"api/file/" + (listbox.SelectedIndex + 1).ToString();
                WebClient web = new WebClient();
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Text file (*.txt)|*.txt";
                ProcessStartInfo infoStartProcess = new ProcessStartInfo();

                
                if (saveFileDialog.ShowDialog() == true)
                {
                    web.DownloadFile(pathDowland, saveFileDialog.FileName);
                    infoStartProcess.FileName = saveFileDialog.FileName;
                    Process.Start(infoStartProcess);
                    // (listbox.SelectedIndex + 1).ToString() + ".txt"
                }
                
               
                
            }
            
        }

        static async Task<string> GetProductAsync(string path)
        {
            string files = string.Empty;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                files = await response.Content.ReadAsStringAsync();
            }

            return files;
        }
    }
}
