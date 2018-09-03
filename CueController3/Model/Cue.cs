using System.Windows.Controls;
using System.Windows.Markup;

namespace CueController3.Model
{
    class Cue
    {
        public Trigger trigger;
        public Send send;
        public string comment = "";
        private string description = "";
        public Canvas Icon
        {
            get
            {
                if (trigger != null) return trigger.icon;
                else return XamlReader.Parse(Properties.Resources.empty) as Canvas;
            }

        }

        public string Nr { get; set; }

        public string Description
        {
            get { return description; }
            set
            {
                description = value.Trim();
                if (send != null && send.type == SendType.PB)
                    send = Send.GetSend("PB", description);
                else if (send != null && send.type == SendType.OSC)
                    send = Send.GetSend("OSC " + send.oscCmd.id, description);
            }
        }

        public string SendString
        {
            get
            {
                if (send != null) return send.name;
                else return "";
            }
            set
            {
                send = Send.GetSend(value, description);
            }
        }

        public string TriggerString
        {
            get
            {
                if (trigger != null) return trigger.name;
                else return "";
            }
            set
            {
                trigger = Trigger.GetTrigger(value);
            }
        }
    }
}
