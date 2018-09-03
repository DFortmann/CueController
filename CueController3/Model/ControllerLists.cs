using System;
using System.Collections.Generic;

namespace CueController3.Model
{
    class ControllerLists
    {
        static string cmds = @"
---CueController--
Go
Back
Up
Down
Mute All
Unmute All
Mute Note
Unmute Note
Mute MSC
Unmute MSC
Script 1
Script 2
Script 3
Script 4
Script 5
Script 6
Script 7
Script 8
Script 9
Script 10

---Pandoras Box---
AutoSetParamDouble,siteNum As Integer,deviceNum As Integer,ParamName As String,value As Double
AutoSetParamDoubleExtended,siteNum As Integer,deviceNum As Integer,ParamName As String,value As Double,silent As Boolean,direct As Boolean
AutoSetParamInSelectionDouble,ParamName As String,value As Double
AutoSetSequenceTransportMode,sequenceNum As Integer,ModeName As String
AutoMoveSequenceToCue,sequenceNum As Integer,cueId As Integer
AutoMoveSequenceToTime,sequenceNum As Integer,hours As Integer,minutes As Integer,seconds As Integer,frames As Integer
AutoMoveSequenceToLastNextFrame,sequenceNum As Integer,isNext As Boolean
AutoMoveSequenceToLastNextCue,sequenceNum As Integer,isNext As Boolean
AutoSetSequenceTransparency,seqNum As Integer,transparency As Integer
AutoResetAll
AutoResetSite,siteNum As Integer
AutoResetDevice,siteNum As Integer,deviceNum As Integer
AutoResetParam,siteNum As Integer,deviceNum As Integer,ParamName As String
AutoActivateAll
AutoActivateSite,siteNum As Integer
AutoActivateDevice,siteNum As Integer,deviceNum As Integer
AutoActivateParam,siteNum As Integer,deviceNum As Integer,ParamName As String
AutoClearAllActive
AutoClearActiveSite,siteNum As Integer
AutoClearActiveDevice,siteNum As Integer,deviceNum As Integer
AutoClearActiveParam,siteNum As Integer,deviceNum As Integer,ParamName As String
AutoToggleFullscreen,siteNum As Integer
AutoSetParamRelativeDouble,siteNum As Integer,deviceNum As Integer,ParamName As String,value As Double
AutoSetParamRelativeDoubleExtended,siteNum As Integer,deviceNum As Integer,ParamName As String,value As Double,silent As Boolean,direct As Boolean
AutoSetParamRelativeInSelectionDouble,ParamName As String,value As Double
AutoSpreadAll
AutoRemoveInconsistent
AutoStoreActive,seqNum As Integer
AutoStoreActiveToTime,seqNum As Integer,hours As Integer,minutes As Integer,seconds As Integer,frames As Integer
AutoClearSelection
AutoSetSequenceCuePlayMode,seqNum As Integer,cueId As Integer,playMode As Param.CuePlayMode
AutoSetNextSequenceCuePlayMode,seqNum As Integer,playMode As Integer
AutoSetIgnoreNextSequenceCue,seqNum As Integer,doIgnore As Boolean
AutoSaveProject
AutoSetSiteIpById,siteNum As Integer,Ip As String
AutoSetDeviceSelection,siteNum As Integer,deviceNum As Integer,selectionMode As Integer
AutoBackupMode,enable As Boolean
AutoApplyView,viewNum As Integer";

        public static string[] GetCmdList()
        {
            return cmds.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
        }

        public static string[] GetCcCmdList()
        {
            return new string[] { "Go", "Back", "Up", "Down", "Mute All", "Unmute All", "Mute Note", "Unmute Note", "Mute MSC", "Unmute MSC", "Script 1",
                    "Script 2", "Script 3", "Script 4", "Script 5", "Script 6", "Script 7", "Script 8", "Script 9", "Script 10" };
        }

        public static List<string> GetCtrlNames()
        {
            List<string> ctrlList = new List<string>();

            for (int i = 1; i < 9; ++i)
                ctrlList.Add("Fader " + i);

            for (int i = 1; i < 9; ++i)
                ctrlList.Add("Knob " + i);

            ctrlList.Add("Play Button");
            ctrlList.Add("Stop Button");
            ctrlList.Add("RWD Button");
            ctrlList.Add("FWD Button");
            ctrlList.Add("Record Button");
            ctrlList.Add("Cycle Button");

            ctrlList.Add("Track Left");
            ctrlList.Add("Track Right");
            ctrlList.Add("Set Marker");
            ctrlList.Add("Marker Left");
            ctrlList.Add("Marker Right");

            for (int i = 1; i < 9; ++i)
                ctrlList.Add("S Button " + i);

            for (int i = 1; i < 9; ++i)
                ctrlList.Add("M Button " + i);

            for (int i = 1; i < 9; ++i)
                ctrlList.Add("R Button " + i);

            return ctrlList;
        }
    }
}
