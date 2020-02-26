using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace TestExample.View.Dowloader
{
    /// <summary>
    /// Dowloader.xaml 的交互逻辑
    /// </summary>
    public partial class DowloaderView : Window
    {
        public DowloaderView()
        {
            InitializeComponent();

            //判断运行环境
            var versions = GetDotNetVersions();
            var hasMatched = versions.Where(version =>
            {
                var firstIndex = (version ?? "").IndexOf(".", StringComparison.Ordinal);
                var mainVersion = (version ?? "").Substring(0, firstIndex);
                if (!int.TryParse(mainVersion, out int intMainVersion)) return false;
                return intMainVersion >= 5;
            }).Count();
            if (hasMatched == 0)
            {
                MessageBox.Show("本程序至少需.NET5.0！");
            }
        }

        private void Download_OnClick(object sender, RoutedEventArgs e)
        {
            DownloadFile("http://172.168.3.188:8080/erpupdate/ErpClient/ErpUpdate.exe", @"C:\ErpUpdate.exe", ProcessBar);
        }

        private void Run_OnClick(object sender, RoutedEventArgs e)
        {
            ExecuteFile();
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="url"></param>
        /// <param name="fileName"></param>
        /// <param name="prog"></param>
        private void DownloadFile(string url, string fileName, ProgressBar prog)
        {
            try
            {
                float oldPercent = 0;
                var myrq = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
                var myrp = (System.Net.HttpWebResponse)myrq.GetResponse();
                var totalBytes = myrp.ContentLength;
                if (prog != null)
                {
                    prog.Maximum = (int)totalBytes;
                }
                var st = myrp.GetResponseStream();
                var so = new FileStream(fileName, FileMode.Create);
                long totalDownloadedByte = 0;
                var by = new byte[1024];
                var osize = st.Read(by, 0, by.Length);
                while (osize > 0)
                {
                    totalDownloadedByte = osize + totalDownloadedByte;
                    System.Windows.Forms.Application.DoEvents();
                    so.Write(by, 0, osize);
                    if (prog != null)
                    {
                        prog.Value = (int)totalDownloadedByte;
                    }
                    osize = st.Read(by, 0, by.Length);

                    var percent = totalDownloadedByte / (float)totalBytes * 100;
                    if (!(Math.Abs(oldPercent - percent) > 0)) continue;
                    oldPercent = percent;
                    LblDownloadPercentent.Content = $@"当前下载进度{Math.Round(percent, 0).ToString(CultureInfo.InvariantCulture)}%";
                    System.Windows.Forms.Application.DoEvents(); //必须加注这句代码，否则label1将因为循环执行太快而来不及显示信息
                }
                so.Close();
                st.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("发生错误了！");
            }
        }

        private void ExecuteFile()
        {
            //调用外部程序导cmd命令行
            var p = new Process();
            p.StartInfo.FileName = "C:\\ErpUpdate.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.CreateNoWindow = false;
            p.Start();
            p.WaitForExit();


            MessageBox.Show("调用程序已经退出！");
        }

        /// <summary>
        /// C#获取已安装 .NET Framework 版本
        /// </summary>
        /// <returns></returns>
        private string[] GetDotNetVersions()
        {
            var directories = new DirectoryInfo(Environment.SystemDirectory + @"\..\Microsoft.NET\Framework").GetDirectories("v?.?.*");
            var list = new ArrayList();
            foreach (var info in directories)
            {
                list.Add(info.Name.Substring(1));
            }
            return list.ToArray(typeof(string)) as string[];
        }
    }
}
