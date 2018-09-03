using System;
using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;
using static CueController3.Controller.Network.OscCtrl;

namespace CueController3.Model
{
    class OscCmd
    {
        public string oscAddress, stringVal;
        public float floatVal;
        public int intVal;
        public int id;
        public DataType type;

        public OscCmd(int id, string oscAddress, string value)
        {
            this.id = id;
            this.oscAddress = oscAddress;
            type = DataType.STRING;
            stringVal = value;
        }

        public OscCmd(int id, string oscAddress, int value)
        {
            this.id = id;
            this.oscAddress = oscAddress;
            type = DataType.INT;
            intVal = value;
        }

        public OscCmd(int id, string oscAddress, float value)
        {
            this.id = id;
            this.oscAddress = oscAddress;
            type = DataType.FLOAT;
            floatVal = value;
        }

        static public OscCmd GetOscCmd(int id, string cmd)
        {
            string[] array = cmd.Trim().Split(' ');
            if (array.Length > 1 && array[0].StartsWith("/"))
            {
                int intVal;
                Match match = Regex.Match(array[1], @"^\d+(\.\d?)+$");
                if (match.Success)
                {
                    float floatVal;
                    if (float.TryParse(array[1], NumberStyles.AllowDecimalPoint, new CultureInfo("en-US"), out floatVal))
                        return new OscCmd(id, array[0], floatVal);
                }
                else if (int.TryParse(array[1], out intVal))
                    return new OscCmd(id, array[0], intVal);

                else return new OscCmd(id, array[0], array[1]);
            }
            return null;
        }
    }
}
