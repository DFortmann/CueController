using System;
using System.Windows.Media;

namespace CueController3.Model
{
    public enum LogMessageType {STATUS, SUCCESS, WARNING, ERROR}

    class LogMessage
    {
        public string time { get; set; }
        public string message { get; set; }
        public Brush bgColor { get; set; }

        public LogMessage(LogMessageType type, string message)
        {
            time = DateTime.Now.ToString("hh:mm:ss.fff");

            if(type == LogMessageType.STATUS) bgColor = new SolidColorBrush(Colors.Transparent);
            else if(type == LogMessageType.SUCCESS) bgColor = new SolidColorBrush(Colors.Green);
            else if (type == LogMessageType.WARNING) bgColor = new SolidColorBrush(Colors.Orange);
            else bgColor = new SolidColorBrush(Colors.Red);
           
            this.message = message;
        }

    }
}
