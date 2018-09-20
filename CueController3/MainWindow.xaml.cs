using CueController3.Controller;
using CueController3.Controller.Beamer;
using CueController3.Controller.Cues;
using CueController3.Controller.Dialog;
using CueController3.Controller.Files;
using CueController3.Controller.MyMidi;
using CueController3.Controller.Network;
using CueController3.Controller.Scripts;
using MahApps.Metro.Controls;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace CueController3
{
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            AddHotKeys();
            mainWindow.Loaded += MainWindow_Loaded;
            mainWindow.Closing += MainWindow_Closing;
            mainWindow.Closed += MainWindow_Closed;
            muteMidiButton.Click += MuteMidiButton_Click;
            unmuteMidiButton.Click += UnmuteMidiButton_Click;
            noteMuteButton.Click += NoteMuteButton_Click;
            mscMuteButton.Click += MscMuteButton_Click;
            oscMuteButton.Click += OscMuteButton_Click;
            exitButton.Click += ExitButton_Click;
            PreviewKeyDown += MainWindow_PreviewKeyDown;
            GotKeyboardFocus += MainWindow_GotKeyboardFocus;
            LostKeyboardFocus += MainWindow_LostKeyboardFocus;
        }

        private void MainWindow_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            goButton.Foreground = Brushes.Red;
            Core.win.goButton.BorderBrush = Brushes.Red;
        }

        private void MainWindow_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (CuelistCtrl.editMode == false)
            {
                Core.win.goButton.Foreground = Brushes.White;
                Core.win.goButton.BorderBrush = Brushes.White;
            }
        }

        private void AddHotKeys()
        {
            AddHotKey(Key.M, MuteMidiButton_Click);
            AddHotKey(Key.U, UnmuteMidiButton_Click);
        }

        public void AddHotKey(Key key, ExecutedRoutedEventHandler function)
        {
            RoutedCommand routedCmd = new RoutedCommand();
            routedCmd.InputGestures.Add(new KeyGesture(key, ModifierKeys.Control));
            CommandBindings.Add(new CommandBinding(routedCmd, function));
        }


        private void MuteMidiButton_Click(object sender, RoutedEventArgs e)
        {
            MidiInputCtrl.NoteMute(true);
            MidiInputCtrl.MscMute(true);
            OscCtrl.OscMute(true);
        }

        private void UnmuteMidiButton_Click(object sender, RoutedEventArgs e)
        {
            MidiInputCtrl.NoteMute(false);
            MidiInputCtrl.MscMute(false);
            OscCtrl.OscMute(false);
        }


        private void MscMuteButton_Click(object sender, RoutedEventArgs e)
        {
            if (MidiInputCtrl.mscMute) MidiInputCtrl.MscMute(false);
            else MidiInputCtrl.MscMute(true);
        }

        private void NoteMuteButton_Click(object sender, RoutedEventArgs e)
        {
            if (MidiInputCtrl.noteMute) MidiInputCtrl.NoteMute(false);
            else MidiInputCtrl.NoteMute(true);
        }

        private void OscMuteButton_Click(object sender, RoutedEventArgs e)
        {
            if (OscCtrl.oscMute) OscCtrl.OscMute(false);
            else OscCtrl.OscMute(true);
        }

        private void MainWindow_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Space && !CuelistCtrl.editMode)
            {
                GoCtrl.GoWithThresh();
            }
            else if (!CuelistCtrl.editMode && e.Key == Key.Up)
            {
                CuelistCtrl.SelectPrevCue();
                e.Handled = true;
            }
            else if (!CuelistCtrl.editMode && e.Key == Key.Down)
            {
                CuelistCtrl.SelectNextCue();
                e.Handled = true;
            }
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Core.SetWnd(this);
            CuelistCtrl.Init();
            FollowCtrl.Init();
            GoCtrl.Init();
            LogCtrl.Init();
            LogCtrl.Status("CueController 3.43b");
            MidiController.Init();
            MidiInputCtrl.Init();
            MidiOutputCtrl.Init();
            PbCtrl.Init();
            BeamerlistCtrl.Init();
            RecentFilesCtrl.Init();
            ReadWriteCtrl.Init();
            MatrixCtrl.Init();
            ScriptlistCtrl.Init();
            CopyCutCtrl.Init();
            OscCtrl.Init();
            OscListCtrl.Init();

            string[] args = Environment.GetCommandLineArgs();
            if (args.Length >= 2) ReadWriteCtrl.Read(args[1]);
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.Close();
        }

        /// <summary>
        /// If user closes window, check if cuelist is saved.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!CuelistCtrl.saved)
            {
                Nullable<bool> result = DialogCtrl.Show(DialogType.QUESTION, OptionType.YESNO, "Cuelist has changed.", "Do you wan't to save the Cuelist?");
                if (result == true && !ReadWriteCtrl.Write(false)) e.Cancel = true;
            }
        }

        /// <summary>
        /// Stop all runing processes, if programm is closed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_Closed(object sender, System.EventArgs e)
        {
            MidiController.Close();
            MidiInputCtrl.Close();
            PbCtrl.Disconnect();
            OscCtrl.Close();
        }
    }
}




