using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using Twitterizer;

namespace Whisper
{
    public class MainWindowViewModel : NotificationObject
    {
        private ObservableCollection<TwitterDirectMessage> _directMessages = new ObservableCollection<TwitterDirectMessage>();
        public ObservableCollection<TwitterDirectMessage> DirectMessages
        {
            get
            {
                
                return _directMessages;
            }
        }

        private string _message;

        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
                RaisePropertyChanged("Message");
            }
        }

        private double _opacity = 1.0;
        public double Opacity
        {
            get
            {
                return _opacity;
            }
            set
            {
                _opacity = value;
                RaisePropertyChanged("Opacity");
            }
        }

        private System.Windows.Media.Color _background = System.Windows.Media.Colors.White;
        public System.Windows.Media.Color Background
        {
            get
            {
                return _background;
            }
            set
            {
                _background = value;
                RaisePropertyChanged("Background");
            }
        }

        private TwitterUtil _twitter = new TwitterUtil();

        public void Initialize()
        {
            _twitter.RequestGetDirectMessage((result) =>
            {
                foreach (var r in result)
                {
                    AppendMessage(r);
                }
            });

            _twitter.RequestGetDirectMessageSent((result) =>
            {
                foreach (var r in result)
                {
                    AppendMessage(r);
                }
            });
        }

        private DispatcherTimer _dispatcherTimer;

        public void StartTimer()
        {
            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Interval = new TimeSpan(0, 0, 60);
            _dispatcherTimer.Tick += (sender, e) =>
            {
                try
                {
                    _twitter.RequestGetDirectMessage((result) =>
                    {
                        foreach (var r in result)
                        {
                            AppendMessage(r);
                        }
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            };
            _dispatcherTimer.Start();
        }

        private void AppendMessage(TwitterDirectMessage r)
        {
            int count = DirectMessages.Count(m=>m.Id == r.Id);

            //存在しない場合追加する
            if (count == 0)
            {
                //並び替え（ID順に並び替える）
                for (int i = 0; i < DirectMessages.Count(); i++)
                {
                    var cur = DirectMessages[i];
                    if (cur.Id <= r.Id)
                    {
                        DirectMessages.Insert(i, r);
                        return;
                    }
                }
                DirectMessages.Add(r);
                //for (int i = DirectMessages.Count(); i > 40; i--)
                //{
                //    DirectMessages.RemoveAt(i - 1);
                //}
            }
        }

        public void SendMessage()
        {
            if (string.IsNullOrEmpty(Message))
            {
                return;
            }

            _twitter.RequestGetDirectMessageNew(Whisper.Properties.Settings.Default.ReceiverID, Message, (result) =>
            {
                AppendMessage(result);
            });

            Message = "";
        }
    }
}
