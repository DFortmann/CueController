using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace CueController3.Controller.Cues
{
    class FollowCtrl
    {
        public static DoubleAnimation myDoubleAnimation = new DoubleAnimation();
        private static Storyboard sb = new Storyboard();
        private static int index = -1;

        public static void Init()
        {
            Core.win.cueTable.SelectionChanged += CueTable_SelectionChanged;
            Core.win.followBar.MouseDown += FollowBar_MouseDown;
            myDoubleAnimation.Completed += MyDoubleAnimation_Completed;
            myDoubleAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(1));
            sb.Begin();

            myDoubleAnimation.From = 0.0;
            myDoubleAnimation.To = 100.0;
            Storyboard.SetTarget(myDoubleAnimation, Core.win.followBar);
            Storyboard.SetTargetProperty(myDoubleAnimation, new PropertyPath(ProgressBar.ValueProperty));
            sb.Children.Add(myDoubleAnimation);
        }

        private static void MyDoubleAnimation_Completed(object sender, System.EventArgs e)
        {
            Stop();
            GoCtrl.Go(index);
        }

        private static void FollowBar_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Stop();
        }

        private static void CueTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Stop();
        }

        public static void Start(int _index, long followTime)
        {
            index = _index;
            myDoubleAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(followTime));
            sb.Begin();
        }

        public static void Stop()
        {
            sb.Stop();
            Core.win.followBar.Value = 0;
        }
    }
}
