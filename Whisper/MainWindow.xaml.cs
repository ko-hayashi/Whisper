using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Windows.Threading;
using System.Runtime.Serialization.Json;
using System.Web.Script.Serialization;

namespace Whisper
{

    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private WpfNotifyIcon _notifyIcon = new WpfNotifyIcon();

        private void InitializeNotifyIcon()
        {
            _notifyIcon.NotifyIcon.Text = "Whisper";
            _notifyIcon.NotifyIcon.Icon = Whisper.Properties.Resources.AppIcon;

            ContextMenuStrip menuStrip = new ContextMenuStrip();
            ToolStripMenuItem exitItem = new ToolStripMenuItem();
            exitItem.Text = Whisper.Properties.Resources.Exit;
            exitItem.Click += (sender, e) =>
            {
                _notifyIcon.Dispose();
                System.Windows.Application.Current.Shutdown();
            };
            menuStrip.Items.Add(exitItem);
            _notifyIcon.NotifyIcon.ContextMenuStrip = menuStrip;

            _notifyIcon.Attach(this);
        }

        private DispatcherTimer _dispatcherTimer;

        private void InitializeTimer()
        {
            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            _dispatcherTimer.Tick += (sender, e) =>
            {
                ViewModel.DirectMessages.Add(DateTime.Now.ToString());

                _notifyIcon.NotifyIcon.ShowBalloonTip(5000, "you got a mail", DateTime.Now.ToString(), ToolTipIcon.Info);
            };
            _dispatcherTimer.Start();
        }

        public MainWindowViewModel ViewModel
        {
            get
            {
                return this.DataContext as MainWindowViewModel;
            }
        }

        private void jsonTest()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            sb.Append("{");
            sb.Append("\"created_at\": \"Mon Aug 27 17:21:03 +0000 2012\", ");
            sb.Append("\"entities\": {");
            sb.Append("\"hashtags\": [], ");
            sb.Append("\"urls\": [], ");
            sb.Append("\"user_mentions\": []");
            sb.Append("}");
            sb.Append("}");
            sb.Append("]");
            string json = sb.ToString();

            using (System.IO.MemoryStream ss = new System.IO.MemoryStream(Encoding.UTF8.GetBytes(json)))
            using (System.IO.StreamWriter ww = new System.IO.StreamWriter(ss))
            {
                ww.Write(json);

                try
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Result[]));
                    var result = serializer.ReadObject(ss) as Result[];
                    Console.WriteLine(result);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }
 
        public MainWindow()
        {
            InitializeComponent();

            InitializeNotifyIcon();
            InitializeTimer();

            //jsonTest();
            
            OAuthLib.Consumer consumer = new OAuthLib.Consumer(Whisper.Properties.Settings.Default.ConsumerKey, Whisper.Properties.Settings.Default.ConsumerSecret);
            OAuthLib.AccessToken accessToken = new OAuthLib.AccessToken(Whisper.Properties.Settings.Default.AccessToken, Whisper.Properties.Settings.Default.AccessTokenSecret);
            var response = consumer.AccessProtectedResource(accessToken, "https://api.twitter.com/1.1/direct_messages.json", "GET", "http://twitter.com", new [] { new OAuthLib.Parameter("since_id","100") });
            using (var stream = response.GetResponseStream())
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Result[]));
                var result = serializer.ReadObject(stream) as Result[];
                Console.WriteLine(result);
            }
             
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _notifyIcon.Dispose();
        }
    }
}
