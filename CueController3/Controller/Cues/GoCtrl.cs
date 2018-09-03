using CueController3.Controller.Beamer;
using CueController3.Controller.MyMidi;
using CueController3.Controller.Network;
using CueController3.Controller.Scripts;
using CueController3.Model;
using System.Diagnostics;

namespace CueController3.Controller.Cues
{
    class GoCtrl
    {
        private static readonly Stopwatch stopwatch = new Stopwatch();

        public static void Init()
        {
            Core.win.goButton.Click += GoButton_Click;
            Core.win.backButton.Click += BackButton_Click;
            stopwatch.Start();
        }

        private static void BackButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            GoBack();
        }

        public static void GoBack()
        {
            if (CuelistCtrl.SelectPrevCue())
            {
                Cue cue = Core.win.cueTable.SelectedItem as Cue;
                if (cue != null && cue.send != null && cue.send.type == SendType.SC)
                    Auto.MoveSequenceToCue(cue.send.pbCue[0], cue.send.pbCue[1]);
            }
        }

        private static void GoButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            GoNextCue();
        }

        public static void GoWithThresh()
        {
            if (stopwatch.ElapsedMilliseconds >= 250)
            {
                stopwatch.Restart();
                GoNextCue();
            }
        }

        public static void GoNextCue()
        {
            Go(Core.win.cueTable.SelectedIndex);
        }

        public static void Go(int index)
        {
            if (index >= 0 && index < CuelistCtrl.cues.Count)
            {
                Cue cue = CuelistCtrl.cues[index];
                ExecuteCueSend(cue);

                if (++index < CuelistCtrl.cues.Count)
                {
                    CuelistCtrl.SelectIndex(index);

                    cue = CuelistCtrl.cues[index];

                    if (cue.trigger != null && cue.trigger.type == TriggerType.FOLLOW)
                        if (cue.trigger.followTime <= 0)
                            Go(index);
                        else FollowCtrl.Start(index, cue.trigger.followTime);
                }

                if (index < CuelistCtrl.cues.Count - 5)
                    Core.win.cueTable.ScrollIntoView(CuelistCtrl.cues[index + 4]);
                else Core.win.cueTable.ScrollIntoView(CuelistCtrl.cues[CuelistCtrl.cues.Count - 1]);
            }
        }

        public static void ExecuteCueSend(Cue cue)
        {
            if (cue.send != null)
                switch (cue.send.type)
                {
                    case SendType.NOTE:
                        MidiOutputCtrl.SendNote(cue.send.note);
                        break;

                    case SendType.MATRIX:
                        MatrixCtrl.SendMatrixCmd(cue.send.matrixCmd);
                        break;

                    case SendType.MSC:
                        MidiOutputCtrl.SendMSC(cue.send.mscCmd);
                        break;

                    case SendType.MUTE:
                        switch (cue.send.mute.Item1)
                        {
                            case MuteType.ALL:
                                MidiInputCtrl.NoteMute(cue.send.mute.Item2);
                                MidiInputCtrl.MscMute(cue.send.mute.Item2);
                                OscCtrl.OscMute(cue.send.mute.Item2);
                                break;
                            case MuteType.MSC:
                                MidiInputCtrl.MscMute(cue.send.mute.Item2);
                                break;
                            case MuteType.NOTE:
                                MidiInputCtrl.NoteMute(cue.send.mute.Item2);
                                break;
                            case MuteType.OSC:
                                OscCtrl.OscMute(cue.send.mute.Item2);
                                break;
                        }
                        break;

                    case SendType.SC:
                        PbCtrl.Play(cue.send.pbCue);
                        break;

                    case SendType.SCRIPT:
                        ScriptlistCtrl.ExecuteScript(cue.send.scriptNr);
                        break;

                    case SendType.SHUTTER:
                        BeamerlistCtrl.Shutter(cue.send.beamerShutter.Item1, cue.send.beamerShutter.Item2);
                        break;

                    case SendType.PB:
                        PbCtrl.ExecuteCmd(cue.send.pbCmd);
                        break;

                    case SendType.OSC:
                        OscCtrl.Send(cue.send.oscCmd);
                        break;
                }
        }
    }
}