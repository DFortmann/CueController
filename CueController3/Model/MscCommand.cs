using System;
using System.Text;
using System.Text.RegularExpressions;

namespace CueController3.Model
{
    /// <summary>
    // MscCommand holds the string describing a MSC Cue or Macro and the related byte message.
    /// </summary>
    class MscCommand
    {
        public string message { get; }
        public byte[] byteMessage { get; }
        static private readonly byte[] SENDHEADER = new byte[] { 240, 127, 1, 2, 1 };

        /// <summary>
        /// A MscCommand must be parsed from a string or a byte message,
        /// </summary>
        /// <param name="message"></param>
        /// <param name="byteMessage"></param>
        private MscCommand(string message, byte[] byteMessage)
        {
            this.message = message;
            this.byteMessage = byteMessage;
        }

        /// <summary>
        /// Tries to parse a MSC cue from a string containing the cue number.
        /// </summary>
        /// <param name="cueNrString"></param>
        /// <returns>Cue Command or null.</returns>
        static public MscCommand getCue(string cueNrString)
        {
            
            Match match = Regex.Match(cueNrString, @"^\d+(\.\d+)?$");
            if (match.Success)
            {
                byte[] numberBytes = Encoding.UTF8.GetBytes(cueNrString);
                byte[] byteCommand = new byte[SENDHEADER.Length + numberBytes.Length + 2];
                Array.Copy(SENDHEADER, byteCommand, 5);
                byteCommand[5] = 1;
                Array.Copy(numberBytes, 0, byteCommand, 6, numberBytes.Length);
                byteCommand[byteCommand.Length - 1] = 247;
                return new MscCommand("Cue " + cueNrString, byteCommand);
            }
            return null;
        }

        /// <summary>
        /// Tries to parse a MSC macro from a string containing the macro number.
        /// </summary>
        /// <param name="macroNrString"></param>
        /// <returns>MscCommand or null.</returns>
        static public MscCommand getMacro(string macroNrString)
        {
            int i;
            if (int.TryParse(macroNrString, out i))
            {
                if (i >= 0 && i <= 127)
                {
                    byte[] byteCommand = new byte[8];
                    Array.Copy(SENDHEADER, byteCommand, 5);
                    byteCommand[5] = 7;
                    byteCommand[6] = (byte)i;  //macroNumber;
                    byteCommand[7] = 247;
                    return new MscCommand("Macro " + macroNrString, byteCommand);
                }
            }
            return null;
        }


        /// <summary>
        /// Tries to decode byte message as MscCommand.
        /// </summary>
        /// <param name="byteMessage"></param>
        /// <returns>MscCommand or null</returns>
        static public MscCommand getMscCommand(byte[] msg)
        {
            if (msg.Length < 8) return null;

            //Check receive ID == 10
            if (msg[2] != 10) return null;

            //Get rid of cue list and cue path
            byte stopByte = 0;
            int index = Array.IndexOf(msg, stopByte);
            byte[] byteMessage;

            if (index > -1)
            {
                byteMessage = new byte[index + 1];
                Array.Copy(msg, byteMessage, index);
                byteMessage[byteMessage.Length - 1] = 247;
            }
            else byteMessage = msg;

            //Check if cmd is Cue (1) or Command (7) and create string message
            string messageString;

            if (byteMessage[5] == 1)
            {
                messageString = "Cue ";
                byte[] b = new byte[byteMessage.Length - 7];
                Array.Copy(byteMessage, 6, b, 0, byteMessage.Length - 7);
                string result = Encoding.UTF8.GetString(b);
                messageString += result;
            }
            else if (byteMessage[5] == 7)
            {
                messageString = "Macro " + byteMessage[6];
            }
            else return null;

            return new MscCommand(messageString, byteMessage);
        }

        public static bool Compare(MscCommand cmd1, MscCommand cmd2)
        {
            if (cmd1.byteMessage.Length != cmd2.byteMessage.Length) return false;

            for (int j = 5; j < cmd1.byteMessage.Length - 1; j++)
                if (cmd1.byteMessage[j] != cmd2.byteMessage[j]) return false;

            return true;
        }

    }
}
