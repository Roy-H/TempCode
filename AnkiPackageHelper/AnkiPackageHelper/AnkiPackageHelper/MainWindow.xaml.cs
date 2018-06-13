using System.IO;
using System.Threading.Tasks;
using System.Windows;
using SevenZip;
using System.Data.SQLite;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Windows.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AnkiPackageHelper
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }

        public void ExtractFileCallback(ExtractFileCallbackArgs extractFileCallbackArgs)
        {

        }

        MediaElement player = new MediaElement();
        private async void Grid_Drop(object sender, DragEventArgs e)
        {
            
            if (!Directory.Exists("./achive"))
                Directory.CreateDirectory("./achive");


            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {                
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                
                var folder = System.IO.Path.GetFileName(files[0]);
                await Task.Factory.StartNew(() =>
                {
                    try
                    {
                        System.IO.Compression.ZipFile.ExtractToDirectory(files[0], "./achive/" + folder);
                    }
                    catch (Exception)
                    {

                       
                    }          
                });
                MessageBox.Show(folder + " extract complete!");
                List<List<string>> mediaCollection = new List<List<string>>();
                SQLiteConnection mdbConnection;
                var ankiFiles = Directory.GetFiles(@"./achive/" + folder, "*.anki2",SearchOption.TopDirectoryOnly);
                var mediaFiles = Directory.GetFiles(@"./achive/" + folder, "media", SearchOption.TopDirectoryOnly);

                if (ankiFiles != null && ankiFiles.Length > 0)
                {                  
                    mdbConnection = new SQLiteConnection("Data Source="+ ankiFiles[0] + ";Version=3;");
                    mdbConnection.Open();
                    string sql = "select sfld,flds from notes";
                    SQLiteCommand command = new SQLiteCommand(sql, mdbConnection);
                    SQLiteDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        
                        Regex soundPattern = new Regex("\\[sound:(.*?)\\]");
                        Regex imagePattern = new Regex("\\[img:(.*?)\\]");
                        Regex mediaPattern = new Regex(@"\[sound:(.*?)\]|src\s*=\s*'(.+?)'");
                        var media = mediaPattern.Matches(reader["flds"].ToString());
                        
                        List<string> list = new List<string>();
                        foreach (Match item in media)
                        {
                            list.Add(item.Groups[1].ToString());
                        }

                        mediaCollection.Add(list);
                    }
                    //Console.WriteLine("Surface: " + reader["sfld"] + "\tBack: " + reader["flds"]);

                }
                Dictionary<string, string> mediamap = new Dictionary<string, string>();
                if (mediaFiles != null && mediaFiles.Length > 0)
                {
                    string json = File.ReadAllText(mediaFiles[0]);
                    JObject jobj =  JsonConvert.DeserializeObject(json) as JObject;
                    foreach (var item in jobj)
                    {
                        Console.WriteLine(item.Key + "---"+item.Value);
                        mediamap[item.Key] = item.Value.ToString();
                    }
                   
                }
            }
        }

        
        private void Grid_Drop_Sound(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                player.Source = new Uri(files[0]);
                player.UnloadedBehavior = MediaState.Manual;
                player.Play();
                return;
            }
        }
    }
}
