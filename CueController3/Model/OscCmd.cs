using System.Globalization;
using System.Text.RegularExpressions;
using static CueController3.Controller.Network.OscCtrl;

namespace CueController3.Model
{
    class OscCmd
    {
        public string oscAddress, stringVal;
        public float floatVal;
        public int intVal;
        public string keyword;
        public DataType type;

        public OscCmd(string keyword, string oscAddress, string value)
        {
            this.keyword = keyword;
            this.oscAddress = oscAddress;
            type = DataType.STRING;
            stringVal = value;
        }

        public OscCmd(string keyword, string oscAddress, int value)
        {
            this.keyword = keyword;
            this.oscAddress = oscAddress;
            type = DataType.INT;
            intVal = value;
        }

        public OscCmd(string keyword, string oscAddress, float value)
        {
            this.keyword = keyword;
            this.oscAddress = oscAddress;
            type = DataType.FLOAT;
            floatVal = value;
        }

        public static OscCmd GetOscCmd(string cmd)
        {
            string[] array = cmd.Trim().Split(' ');
            if (array.Length < 2) return null;
            if (array.Length == 2 && array[1].StartsWith("/")) return null;

            string address = "";
            string value = array[array.Length - 1];

            if (array.Length > 2)
            {
                if (!array[1].StartsWith("/")) return null;
                address = array[1];
            }

            Match match = Regex.Match(value, @"^\d+(\.\d?)+$");
            if (match.Success)
            {
                if (float.TryParse(value, NumberStyles.AllowDecimalPoint, new CultureInfo("en-US"), out float floatVal))
                    return new OscCmd(array[0], address, floatVal);
            }

            if (int.TryParse(value, out int intVal))
                return new OscCmd(array[0], address, intVal);

            return new OscCmd(array[0], address, value);
        }
    }
}
