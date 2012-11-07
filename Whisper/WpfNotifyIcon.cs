using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Whisper
{
    public class WpfNotifyIcon : IDisposable
    {
        private System.Windows.Window _window = null;

        public void Detach()
        {
            if (_window != null)
            {
                _window.Closing -= new System.ComponentModel.CancelEventHandler(_window_Closing);
                _window = null;
            }
        }

        public void Attach(System.Windows.Window window)
        {
            Detach();

            _window = window;
            _window.Closing += new System.ComponentModel.CancelEventHandler(_window_Closing);

            NotifyIcon.MouseClick += (sender, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (_window != null)
                    {
                        _window.Visibility = System.Windows.Visibility.Visible;
                        _window.WindowState = System.Windows.WindowState.Normal;
                        _window.Activate();
                    }
                }
            };
            NotifyIcon.Visible = true;

            _window.ShowInTaskbar = false;
            _window.Visibility = System.Windows.Visibility.Hidden;
        }

        void _window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            _window.Visibility = System.Windows.Visibility.Hidden;
        }

        private NotifyIcon _notifyIcon = null;
        public NotifyIcon NotifyIcon
        {
            get
            {
                if (_notifyIcon == null)
                {
                    _notifyIcon = new NotifyIcon();
                }
                return _notifyIcon;
            }
        }

        public void Dispose()
        {
            Detach();

            if (_notifyIcon != null)
            {
                _notifyIcon.Dispose();
                _notifyIcon = null;
            }
        }
    }
}
