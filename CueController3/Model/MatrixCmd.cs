using System;
using System.Text;
using System.Text.RegularExpressions;

namespace CueController3.Model
{
    class MatrixCmd
    {
        private static ASCIIEncoding asen = new ASCIIEncoding();

        public string name, description, response, successText, failText;
        public byte[] message;

        private MatrixCmd(string name, string description, byte[] message, string response, string successText, string failText)
        {
            this.name = name;
            this.description = description;
            this.message = message;
            this.response = response;
            this.successText = successText;
            this.failText = failText;
        }

        public static MatrixCmd getMatrixCmd(String cmd)
        {
            //Load Preset
            Match match = Regex.Match(cmd, @"P(\d{1,2})", RegexOptions.IgnoreCase);
            if (match.Success && match.Groups.Count == 2)
            {
                int presetNr = int.Parse(match.Groups[1].Value);
                string description = "Matrix: Load Preset " + presetNr;
                byte[] byteCmd = asen.GetBytes("{%" + presetNr + "}");
                string response = "(LPR" + presetNr.ToString("D2") + ")";
                string successText = "Matrix: Loaded Preset " + presetNr;
                string failText = "Matrix: Error loading preset " + presetNr;
                return new MatrixCmd(cmd, description, byteCmd, response, successText, failText);
            }

            //Switch input to outout
            match = Regex.Match(cmd, @"S(\d{1,2})>(\d{1,2})", RegexOptions.IgnoreCase);
            if (match.Success && match.Groups.Count == 3)
            {
                int inputNr = int.Parse(match.Groups[1].Value);
                int outputNr = int.Parse(match.Groups[2].Value);
                string description = "Matrix: Switch In " + inputNr + " to Out " + outputNr;
                byte[] byteCmd = asen.GetBytes("{" + inputNr + "@" + outputNr + "}");
                string response = "(O" + outputNr.ToString("D2") + " I" + inputNr.ToString("D2") + ")";
                string successText = "Matrix: Switched In " + inputNr + " to Out " + outputNr;
                string failText = "Matrix: Error switching In " + inputNr + " to Out " + outputNr;
                return new MatrixCmd(cmd, description, byteCmd, response, successText, failText);
            }

            //Mute output
            match = Regex.Match(cmd, @"M(\d{1,2})", RegexOptions.IgnoreCase);
            if (match.Success && match.Groups.Count == 2)
            {
                int outputNr = int.Parse(match.Groups[1].Value);
                string description = "Matrix: Mute Out " + outputNr;
                byte[] byteCmd = asen.GetBytes("{#" + outputNr + "}");
                string response = "(1MT" + outputNr.ToString("D2") + ")";
                string successText = "Matrix: Muted Out " + outputNr;
                string failText = "Matrix: Error muting Out " + outputNr;
                return new MatrixCmd(cmd, description, byteCmd, response, successText, failText);
            }

            //Unmute output
            match = Regex.Match(cmd, @"U(\d{1,2})", RegexOptions.IgnoreCase);
            if (match.Success && match.Groups.Count == 2)
            {
                int outputNr = int.Parse(match.Groups[1].Value);
                string description = "Matrix: Unmute Out " + outputNr;
                byte[] byteCmd = asen.GetBytes("{+" + outputNr + "}");
                string response = "(0MT" + outputNr.ToString("D2") + ")";
                string successText = "Matrix: Unmuted Out " + outputNr;
                string failText = "Matrix: Error unmuting Out " + outputNr;
                return new MatrixCmd(cmd, description, byteCmd, response, successText, failText);
            }

            //Switch in to all outs
            match = Regex.Match(cmd, @"S(\d{1,2})>ALL", RegexOptions.IgnoreCase);
            if (match.Success && match.Groups.Count == 2)
            {
                int inputNr = int.Parse(match.Groups[1].Value);
                string description = "Matrix: Switch In " + inputNr + " to all Outs";
                byte[] byteCmd = asen.GetBytes("{" + inputNr + "@O}");
                string response = "(I" + inputNr.ToString("D2") + " ALL)";
                string successText = "Matrix: Switched In " + inputNr + " to all Outs";
                string failText = "Matrix: Error switching In " + inputNr + " to all Outs";
                return new MatrixCmd(cmd, description, byteCmd, response, successText, failText);
            }

            //Disconnect all ins from out
            match = Regex.Match(cmd, @"D(\d{1,2})", RegexOptions.IgnoreCase);
            if (match.Success && match.Groups.Count == 2)
            {
                int outputNr = int.Parse(match.Groups[1].Value);
                string description = "Matrix: Disconnect all Ins from Out " + outputNr;
                byte[] byteCmd = asen.GetBytes("{0@" + outputNr + "}");
                string response = "(O" + outputNr.ToString("D2") + " I00)";
                string successText = "Matrix: Disconnected all Ins from Out " + outputNr;
                string failText = "Matrix: Error disconnecting Ins from Out " + outputNr;
                return new MatrixCmd(cmd, description, byteCmd, response, successText, failText);
            }
              
            return null;
        }
    }
}
