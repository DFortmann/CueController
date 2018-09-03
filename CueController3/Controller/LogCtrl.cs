using CueController3.Model;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace CueController3.Controller
{
    class LogCtrl
    {
        private static ObservableCollection<LogMessage> logs = new ObservableCollection<LogMessage>();

        public static void Init()
        {
            Core.win.clearLogButton.Click += ClearLogButton_Click;
            Core.win.logTable.ItemsSource = logs;
            Core.win.logTable.IsReadOnly = true;
            Core.win.logTable.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
            Core.win.logTable.Focusable = false;
        }

        private static void ClearLogButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            logs.Clear();
        }

        public static void Status(string message)
        {
            AddLog(new LogMessage(LogMessageType.STATUS, message));
        }

        public static void ThreadSafeStatus(string message)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    Status(message);
                }));
            }
            catch
            {
                //Only fails if app was closed before.
            }
        }

        public static void Success(string message)
        {
            AddLog(new LogMessage(LogMessageType.SUCCESS, message));
        }

        public static void ThreadSafeSuccess(string message)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    Success(message);
                }));
            }
            catch {
                //Only fails if app was closed before.
            }
        }

        public static void Warning(string message)
        {
            AddLog(new LogMessage(LogMessageType.WARNING, message));
        }


        public static void ThreadSafeWarning(string message)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    Warning(message);
                }));
            }
            catch
            {
                //Only fails if app was closed before.
            }
        }

        public static void Error(string message)
        {
            AddLog(new LogMessage(LogMessageType.ERROR, message));
        }

        public static void ThreadSafeError(string message)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    Error(message);
                }));
            }
            catch
            {
                //Only fails if app was closed before.
            }
        }

        private static void AddLog(LogMessage log)
        {
            if (logs.Count > 100) logs.RemoveAt(0);
            logs.Add(log);
            Core.win.logTable.ScrollIntoView(log);
        }
    }
}
