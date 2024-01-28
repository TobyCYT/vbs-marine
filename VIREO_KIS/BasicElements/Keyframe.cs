using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace VIREO_KIS
{
    class Keyframe:Image
    {
        MainWindow mainWnd;
        public void UpdateParent(MainWindow _mainWnd)
        {
            mainWnd = _mainWnd;
        }

        public int VideoPos
        {
            get; set;
        }

        public string VideoName
        {
            get; set;
        }
        public int VideoID
        {
            get; set;
        }
        public int ShotID
        {
            get; set;
        }

        public static readonly DependencyProperty VideoProperty = DependencyProperty.Register("VideoID", typeof(int), typeof(Keyframe), new PropertyMetadata(new PropertyChangedCallback(ChangeVideoID)));

        private static void ChangeVideoID(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Keyframe kf = d as Keyframe;
            kf.VideoID = Convert.ToInt32(e.NewValue);
        }

        public static readonly DependencyProperty ShotProperty = DependencyProperty.Register("ShotID", typeof(int), typeof(Keyframe), new PropertyMetadata(new PropertyChangedCallback(ChangeShotID)));

        private static void ChangeShotID(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Keyframe kf = d as Keyframe;
            kf.ShotID = Convert.ToInt32(e.NewValue);
        }

        public static readonly DependencyProperty PosProperty = DependencyProperty.Register("VideoPos", typeof(int), typeof(Keyframe), new PropertyMetadata(new PropertyChangedCallback(ChangePos)));

        private static void ChangePos(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Keyframe kf = d as Keyframe;
            kf.VideoPos = Convert.ToInt32(e.NewValue);
        }

        public static readonly DependencyProperty VideoNameProperty = DependencyProperty.Register("VideoName", typeof(string), typeof(Keyframe), new PropertyMetadata(new PropertyChangedCallback(ChangeVideoName)));

        private static void ChangeVideoName(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Keyframe kf = d as Keyframe;
            kf.VideoName = e.NewValue as string;
        }
    }
}
