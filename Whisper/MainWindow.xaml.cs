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


        public MainWindowViewModel ViewModel
        {
            get
            {
                return this.DataContext as MainWindowViewModel;
            }
        }

        //private void jsonTest()
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("[");
        //    sb.Append("{");
        //    sb.Append("\"created_at\": \"Mon Aug 27 17:21:03 +0000 2012\", ");
        //    sb.Append("\"entities\": {");
        //    sb.Append("\"hashtags\": [], ");
        //    sb.Append("\"urls\": [], ");
        //    sb.Append("\"user_mentions\": []");
        //    sb.Append("}");
        //    sb.Append("}");
        //    sb.Append("]");
        //    string json = sb.ToString();

        //    using (System.IO.MemoryStream ss = new System.IO.MemoryStream(Encoding.UTF8.GetBytes(json)))
        //    using (System.IO.StreamWriter ww = new System.IO.StreamWriter(ss))
        //    {
        //        ww.Write(json);

        //        try
        //        {
        //            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Result[]));
        //            var result = serializer.ReadObject(ss) as Result[];
        //            Console.WriteLine(result);
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(ex.ToString());
        //        }
        //    }
        //}
 
        public MainWindow()
        {
            InitializeComponent();

            textBox1.Focus();

            InitializeNotifyIcon();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            try
            {
                DragMove();
            }
            catch { }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _notifyIcon.Dispose();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.Initialize();
            ViewModel.DirectMessages.CollectionChanged += (sender2, e2) =>
            {
                if (e2.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                {
                    foreach (Twitterizer.TwitterDirectMessage item in e2.NewItems)
                    {
                        _notifyIcon.ShowBalloonTip(1000, "新着メッセージ", DateTime.Now.ToString(), ToolTipIcon.Info);
                    }
                }
            };
            ViewModel.StartTimer();
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            _notifyIcon.HideWindow();
        }

        private void TextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                ViewModel.SendMessage();
            }
        }

        private void listBox1_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ViewModel.Opacity = 0.8;
            ViewModel.Background = Colors.White;
        }

        private void listBox1_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ViewModel.Opacity = 0.2;
            ViewModel.Background = Colors.Transparent;
        }
    }
}
