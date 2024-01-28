using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.IO;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VIREO_KIS.Properties;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;

namespace VIREO_KIS
{
    /// <summary>
    /// Interaction logic for VideoPlayer.xaml
    /// </summary>
    public partial class VideoPlayer : UserControl
    {
        MainWindow mainWnd;
        long miliSecToEnd;
        long miliSecToBeg;
        int curFrame;
        int startFrame;
        int stopFrame;
        string LINK_SHOT = Settings.Default.DATASET_LINK_SHOT_BOUNDARY;
        //string LINK_VID = Settings.Default.DATASET_LINK_VIDEOS;
        string LINK_VID = "";
        string LINK_VID_V3C1 = Settings.Default.DATASET_LINK_VIDEOS_V3C1;
        string LINK_VID_V3C2 = Settings.Default.DATASET_LINK_VIDEOS_V3C2;
        static int VIDEOS_NUM_V3C1 = Settings.Default.DATASET_NUM_VIDEOS_V3C1;

        string LINK_VID_NAME = Settings.Default.DATASET_LINK_VIDEO_NAME;
        List<int> frameCount = new List<int>();
        List<string> videoName = new List<string>();
        int shotID;
        string vidID;
        public void UpdateParent(MainWindow _mainWnd)
        {
            mainWnd = _mainWnd;
        }
        public VideoPlayer()
        {
            InitializeComponent();
            videoName = File.ReadAllLines(LINK_VID_NAME).ToList();
            mePlayer.SpeedRatio = 1;
            mePlayer.Stretch = Stretch.Fill;
            mePlayer.Volume = 0;
            mePlayer.MediaOpened += MePlayer_MediaOpened;
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        private void MePlayer_MediaOpened(object sender, RoutedEventArgs e)
        {
            MediaElement x = sender as MediaElement;
            //var lines = File.ReadAllLines(LINK_SHOT + vidID.ToString().PadLeft(5, '0') + ".tsv");
            var lines = File.ReadAllLines(LINK_SHOT + Globals.idx2vid[Convert.ToInt32(vidID)].ToString() + ".tsv");
            startFrame = Convert.ToInt32(lines[shotID].Split('\t')[0]);
            stopFrame = Convert.ToInt32(lines[shotID].Split('\t')[2]);
            miliSecToBeg = Convert.ToInt64(Convert.ToDouble(lines[shotID].Split('\t')[1]) * 1000);
            miliSecToEnd = Convert.ToInt64(Convert.ToDouble(lines[shotID].Split('\t')[3]) * 1000);
            //miliSecToBeg = 0;
            //miliSecToEnd = 3000;
            x.Position = new TimeSpan(0, Convert.ToInt32(miliSecToBeg / 3600000), Convert.ToInt32((miliSecToBeg % 3600000) / 60000),
                                        Convert.ToInt32((miliSecToBeg % 60000) / 1000), Convert.ToInt32(miliSecToBeg % 1000));
            mediaPlayerIsPlaying = true;
        }

        //public void PlayVideo(string linkToVideo)
        //{
        //    if ((linkToVideo != null) && (linkToVideo != "None"))
        //    {
        //        mePlayer.Stop();
        //        shotID = Convert.ToInt32(linkToVideo.Split('-')[1]);
        //        string newVidID = linkToVideo.Split('-')[0].PadLeft(5, '0');
        //        string vidShot = Globals.trec2marine[newVidID + '_' + shotID].ToString();
        //        Console.WriteLine("Video Details================================");
        //        Console.WriteLine("newVidID: " + Globals.idx2vid[Convert.ToInt32(newVidID)].ToString());
        //        //Console.WriteLine("shotID: " + shotID);
        //        if (vidShot.Equals(vidID))
        //        {
        //            //mePlayer.Source = new Uri(LINK_VID + vidShot + ".mp4");
        //            startFrame = Convert.ToInt32(lines[shotID].Split('\t')[0]);
        //            stopFrame = Convert.ToInt32(lines[shotID].Split('\t')[2]);
        //            miliSecToBeg = Convert.ToInt64(Convert.ToDouble(lines[shotID].Split('\t')[1]) * 1000);
        //            miliSecToEnd = Convert.ToInt64(Convert.ToDouble(lines[shotID].Split('\t')[3]) * 1000);
        //            mePlayer.Position = new TimeSpan(0, Convert.ToInt32(miliSecToBeg / 3600000), Convert.ToInt32((miliSecToBeg % 3600000) / 60000),
        //                                                Convert.ToInt32((miliSecToBeg % 60000) / 1000), Convert.ToInt32(miliSecToBeg % 1000));
        //            mePlayer.Play();
        //            mediaPlayerIsPlaying = true;
        //            mePlayer.Play();
        //            mediaPlayerIsPlaying = true;
        //        }
        //        else
        //        {
        //            vidID = vidShot;
        //            //var ividID = Convert.ToInt32(vidID) - 1;
        //            //var vid_link = linkToVideo.Split('-')[0].PadLeft(5, '0');

        //            //if (Convert.ToInt32(vidID) <= VIDEOS_NUM_V3C1)
        //            //    LINK_VID = LINK_VID_V3C1;
        //            //else
        //            LINK_VID = LINK_VID_V3C2;


        //            // mePlayer.Source = new Uri(LINK_VID + vid_link + "\\" + videoName[ividID]);
        //            // mePlayer.Source = new Uri(LINK_VID + "\\" + videoName[ividID]);
        //            mePlayer.Source = new Uri(LINK_VID + "\\" + vidShot + ".mp4");
        //            mePlayer.Play();
        //            mediaPlayerIsPlaying = true;
        //        }
        //    }
        //}

        public void PlayVideo(string linkToVideo)
        {
            if ((linkToVideo != null) && (linkToVideo != "None"))
            {
                mePlayer.Stop();
                shotID = Convert.ToInt32(linkToVideo.Split('-')[1]);
                string newVidID = linkToVideo.Split('-')[0];
                Console.WriteLine("Video Details================================");
                Console.WriteLine("newVidID: " + Globals.idx2vid[Convert.ToInt32(newVidID)].ToString());
                //Console.WriteLine("shotID: " + shotID);
                if (newVidID.Equals(vidID))
                {
                    //var lines = File.ReadAllLines(LINK_SHOT + vidID.ToString().PadLeft(5, '0') + ".tsv");
                    var lines = File.ReadAllLines(LINK_SHOT + Globals.idx2vid[Convert.ToInt32(vidID)].ToString() + ".tsv");
                    startFrame = Convert.ToInt32(lines[shotID].Split('\t')[0]);
                    stopFrame = Convert.ToInt32(lines[shotID].Split('\t')[2]);
                    miliSecToBeg = Convert.ToInt64(Convert.ToDouble(lines[shotID].Split('\t')[1]) * 1000);
                    miliSecToEnd = Convert.ToInt64(Convert.ToDouble(lines[shotID].Split('\t')[3]) * 1000);
                    mePlayer.Position = new TimeSpan(0, Convert.ToInt32(miliSecToBeg / 3600000), Convert.ToInt32((miliSecToBeg % 3600000) / 60000),
                                                        Convert.ToInt32((miliSecToBeg % 60000) / 1000), Convert.ToInt32(miliSecToBeg % 1000));
                    mePlayer.Play();
                    mediaPlayerIsPlaying = true;
                }
                else
                {
                    vidID = newVidID;
                    var ividID = Convert.ToInt32(vidID) - 1;
                    var vid_link = linkToVideo.Split('-')[0].PadLeft(5, '0');

                    if (Convert.ToInt32(vidID) <= VIDEOS_NUM_V3C1)
                        LINK_VID = LINK_VID_V3C1;
                    else
                        LINK_VID = LINK_VID_V3C2;

                    
                    // mePlayer.Source = new Uri(LINK_VID + vid_link + "\\" + videoName[ividID]);
                    // mePlayer.Source = new Uri(LINK_VID + "\\" + videoName[ividID]);
                    mePlayer.Source = new Uri(LINK_VID + "\\" + Globals.idx2vid[Convert.ToInt32(vidID)].ToString() + ".mp4");
                    mePlayer.Play();
                    mediaPlayerIsPlaying = true;
                }
            }
        }

        private bool mediaPlayerIsPlaying = false;
        private bool userIsDraggingSlider = false;

        private void timer_Tick(object sender, EventArgs e)
        {
            if ((mePlayer.Source != null) && (mePlayer.NaturalDuration.HasTimeSpan) && (!userIsDraggingSlider) && mediaPlayerIsPlaying)
            {
                sliProgress.Minimum = 0;
                sliProgress.Maximum = mePlayer.NaturalDuration.TimeSpan.TotalMilliseconds;
                var ratio = ((mePlayer.Position.TotalMilliseconds - miliSecToBeg) * 1.0) / (miliSecToEnd - miliSecToBeg);
                var range = ratio * (stopFrame - startFrame);
                if (Double.IsNaN(range))
                {
                    range = 0;
                }
                curFrame = Convert.ToInt32(startFrame + range);
                sliProgress.Value = mePlayer.Position.TotalMilliseconds;
                if (mePlayer.Position.TotalMilliseconds >= miliSecToEnd)
                {
                    mediaPlayerIsPlaying = false;
                    mePlayer.Pause();
                }
            }
        }

        private void Submit_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            string submit = vidID + "-" + shotID.ToString();
            mainWnd.SubmitResult(submit, SUBMIT_TYPE.FRAME_ID, SUBMIT_FROM.VIDEO_PLAYER, curFrame);
        }

        private void Submit_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (mePlayer != null) && (mePlayer.Source != null);
        }

        private void Play_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (mePlayer != null) && (mePlayer.Source != null);
        }

        private void Play_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            mePlayer.Play();
            mediaPlayerIsPlaying = true;
        }

        private void Pause_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = mediaPlayerIsPlaying;
        }

        private void Pause_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            mePlayer.Pause();
        }

        private void sliProgress_DragStarted(object sender, DragStartedEventArgs e)
        {
            userIsDraggingSlider = true;
        }

        private void sliProgress_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            userIsDraggingSlider = false;
            mePlayer.Position = TimeSpan.FromMilliseconds(sliProgress.Value);
        }

        private void sliProgress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            lblProgressStatus.Text = curFrame.ToString();
        }
    }
}
