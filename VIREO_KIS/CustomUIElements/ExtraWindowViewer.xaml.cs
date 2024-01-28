using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using VIREO_KIS.Properties;

namespace VIREO_KIS
{
    /// <summary>
    /// Interaction logic for VideoViewer.xaml
    /// </summary>
    public partial class ExtraWindowViewer : UserControl
    {
        public static int TOTAL_RELATED = 999;
        //public static string LNK_THUMB_SHOTS = Settings.Default.DATASET_LINK_THUMBNAILS_SHOTS;
        public static string LNK_THUMB_V3C1 = Settings.Default.DATASET_LINK_THUMBNAILS_SHOTS_V3C1;
        public static string LNK_THUMB_V3C2 = Settings.Default.DATASET_LINK_THUMBNAILS_SHOTS_V3C2;
        static int VIDEOS_NUM_V3C1 = Settings.Default.DATASET_NUM_VIDEOS_V3C1;

        public static string LNK_NEAREST_NEIGHBORS = Settings.Default.NEAREST_NEIGHBORS_LINK;
        public static string LNK_SHOT_BOUNDARY = Settings.Default.DATASET_LINK_SHOT_BOUNDARY;
        public static string LNK_BLACK_IMG = Settings.Default.IMG_BLACK;
        public static string LNK_SUBMITTED_IMG = Settings.Default.IMG_SUBMITTED;
        static int THUMBNAIL_WIDTH = 200;
        static int THUMBNAIL_HEIGHT = 150;

        ExtraViewWindow mainWnd;
        BitmapImage blackImage;
        BitmapImage submitImage;

        int curFrame = -1;
        List<bool> selFrames = new List<bool>();
        List<bool> candidateFrames = new List<bool>();
        double offset = 0;

        struct LoadingItem
        {
            public string link;
            public int id;

            public LoadingItem(string _link, int _id)
            {
                link = _link;
                id = _id;
            }
        }
        public class KfItem : INotifyPropertyChanged
        {
            private BitmapImage _KfImage;
            private string _KfName;
            private int _KfVidID;
            private int _KfShotID;
            private int _KfVidPos;

            public event PropertyChangedEventHandler PropertyChanged;
            public BitmapImage KfImage
            {
                get
                {
                    return this._KfImage;
                }
                set
                {
                    if (this._KfImage != value)
                    {
                        this._KfImage = value;
                        if (this.PropertyChanged != null)
                            this.PropertyChanged(this, new PropertyChangedEventArgs("KfImage"));
                    }
                }
            }

            public string KfName
            {
                get
                {
                    return this._KfName;
                }

                set
                {
                    if (this._KfName != value)
                    {
                        this._KfName = value;
                        if (this.PropertyChanged != null)
                            this.PropertyChanged(this, new PropertyChangedEventArgs("KfName"));
                    }
                }
            }

            public int KfVidID
            {
                get
                {
                    return this._KfVidID;
                }

                set
                {
                    if (this._KfVidID != value)
                    {
                        this._KfVidID = value;
                        if (this.PropertyChanged != null)
                            this.PropertyChanged(this, new PropertyChangedEventArgs("KfVidID"));
                    }
                }
            }

            public int KfVidPos
            {
                get
                {
                    return this._KfVidPos;
                }

                set
                {
                    if (this._KfVidPos != value)
                    {
                        this._KfVidPos = value;
                        if (this.PropertyChanged != null)
                            this.PropertyChanged(this, new PropertyChangedEventArgs("KfVidPos"));
                    }
                }
            }

            public int KfShotID
            {
                get
                {
                    return this._KfShotID;
                }

                set
                {
                    if (this._KfShotID != value)
                    {
                        this._KfShotID = value;
                        if (this.PropertyChanged != null)
                            this.PropertyChanged(this, new PropertyChangedEventArgs("KfShotID"));
                    }
                }
            }
        }
        public ObservableCollection<KfItem> KfList
        {
            get;
            set;
        }
        public void UpdateParent(ExtraViewWindow _mainWnd)
        {
            mainWnd = _mainWnd;
        }

        private void ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            string toLog = "earestNeighbors";

            double newoffset = (scrViewer.VerticalOffset / (scrViewer.ExtentHeight - scrViewer.ViewportHeight));
            if (Double.IsNaN(newoffset))
            {
                newoffset = 0;
            }
            if (Math.Abs(newoffset - offset) >= 0.01)
            {
                mainWnd.LogScrolling(toLog, String.Format("{0:0.0000}", newoffset));
                offset = newoffset;
                if (Double.IsNaN(offset))
                {
                    offset = 0;
                }
            }
        }

        public void LoadCandidates(string shotID, List<SubmittedItem> listSubmitted)
        {
            curFrame = -1;
            for (int i = 0; i < TOTAL_RELATED; i++)
            {
                selFrames[i] = false;
                candidateFrames[i] = false;
            }


            shotID = shotID.Replace('-', '_');
            string video_id = shotID.Split('_')[0];
            video_id = video_id.PadLeft(5, '0');
            shotID = video_id + "_" + shotID.Split('_')[1];
            List<string> files = new List<string>();

            files = new List<string>(File.ReadAllLines(LNK_NEAREST_NEIGHBORS + Globals.trec2marine[shotID].ToString()+".txt"));

            for (int i = 0; i < TOTAL_RELATED; i++)
            {
                string sID = files[i].Split(' ')[0];

                string LNK_THUMB_SHOTS = LNK_THUMB_V3C1;


                string linkToThumbnail = LNK_THUMB_SHOTS + sID + ".jpg";

                if (File.Exists(linkToThumbnail))
                {
                    sID = Globals.marine2trec[sID].ToString();
                    KfList[i].KfName = sID.Replace('_', '-');
                    KfList[i].KfVidID = Convert.ToInt32(sID.Split('_')[0]);
                    KfList[i].KfShotID = Convert.ToInt32(sID.Split('_')[1]);
                    KfList[i].KfVidPos = i;

                    bool check = false;
                    for (int j = 0; j < listSubmitted.Count; j++)
                    {
                        if ((KfList[i].KfVidID == listSubmitted[j].vidID) && (KfList[i].KfShotID == listSubmitted[j].shotID))
                        {
                            KfList[i].KfImage = submitImage;
                            check = true;
                            break;
                        }
                    }
                    candidateFrames[i] = false;
                    if (!check)
                    {
                        ThreadPool.QueueUserWorkItem(LoadImage, new LoadingItem(linkToThumbnail, i));
                    }

                }
            }
            // update border and scroll the view to top
            UpdateKeyframeBorder();
            scrViewer.ScrollToTop();
            offset = 0;
            curFrame = 0;
        }

        public void LoadImage(object obj)
        {
            LoadingItem item = (LoadingItem)obj;
            var bitmap = new BitmapImage();

            using (var stream = new FileStream(item.link, FileMode.Open, FileAccess.Read))
            {
                bitmap.BeginInit();
                bitmap.DecodePixelWidth = THUMBNAIL_WIDTH;
                bitmap.DecodePixelHeight = THUMBNAIL_HEIGHT;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = stream;
                bitmap.EndInit();
            }
            bitmap.Freeze();

            this.Dispatcher.Invoke(DispatcherPriority.Send, new Action<BitmapImage, int>(SetImage), bitmap, item.id);
        }

        public void SetImage(BitmapImage source, int pos)
        {
            KfList[pos].KfImage = source;
        }

        public void UpdateKeyframeBorder()
        {
            ItemsControl itm = this.FindName("KfThumbnails") as ItemsControl;
            for (int i = 0; i < TOTAL_RELATED; i++)
            {
                ContentPresenter kf_old = itm.ItemContainerGenerator.ContainerFromIndex(i) as ContentPresenter;
                if (kf_old != null)
                {
                    DataTemplate myDataTemplate = kf_old.ContentTemplate;
                    Border inner_border = (Border)myDataTemplate.FindName("brd_Inside", kf_old);
                    Border outer_border = (Border)myDataTemplate.FindName("brd_Outside", kf_old);
                    if (i == curFrame)
                    {
                        outer_border.BorderBrush = Brushes.Red;
                        outer_border.BorderThickness = new Thickness(4);
                    }
                    else
                    {
                        outer_border.BorderBrush = Brushes.Black;
                        outer_border.BorderThickness = new Thickness(0);
                    }

                    if (selFrames[i])
                    {
                        inner_border.BorderBrush = Brushes.Orange;
                        inner_border.BorderThickness = new Thickness(4);
                    }
                    else
                    {
                        if (candidateFrames[i])
                        {
                            inner_border.BorderBrush = Brushes.GreenYellow;
                            inner_border.BorderThickness = new Thickness(4);
                        }
                        else
                        {
                            inner_border.BorderBrush = Brushes.Black;
                            inner_border.BorderThickness = new Thickness(0);
                        }
                    }
                }
            }
        }

        public ExtraWindowViewer()
        {
            InitializeComponent();
            KfList = new ObservableCollection<KfItem>();
            blackImage = CreateImage(LNK_BLACK_IMG);
            submitImage = CreateImage(LNK_SUBMITTED_IMG);
            for (int i = 0; i < TOTAL_RELATED; i++)
            {
                KfItem temp = new KfItem();
                temp.KfImage = blackImage;
                temp.KfName = "None";
                KfList.Add(temp);
                selFrames.Add(false);
                candidateFrames.Add(false);
            }
            KfThumbnails.ItemsSource = KfList;
            offset = 0;
        }

        public BitmapImage CreateImage(string imagePath)
        {
            var bitmap = new BitmapImage();

            using (var stream = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
            {
                bitmap.BeginInit();
                bitmap.DecodePixelWidth = THUMBNAIL_WIDTH;
                bitmap.DecodePixelHeight = THUMBNAIL_HEIGHT;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = stream;
                bitmap.EndInit();
            }
            bitmap.Freeze();
            return bitmap;
        }

        private void Keyframe_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Keyframe clicked = sender as Keyframe;

            mainWnd.PlayVideo(clicked.VideoName, false);
            // Ctrl + Click -> add clicked one to selected set
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                selFrames[clicked.VideoPos] = !selFrames[clicked.VideoPos];
                if (selFrames[clicked.VideoPos])
                    curFrame = clicked.VideoPos;
            }
            // Shift + Click -> add a range of selection to selected set
            else if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            {
                if (curFrame == -1)
                {
                    selFrames[clicked.VideoPos] = true;
                    curFrame = clicked.VideoPos;
                }
                else
                {
                    int max = Math.Max(curFrame, clicked.VideoPos);
                    int min = Math.Min(curFrame, clicked.VideoPos);
                    for (int i = min; i <= max; i++)
                    {
                        selFrames[i] = true;
                    }
                    curFrame = clicked.VideoPos;
                }
            }
            // Single click -> reset all selection and select the last one
            else
            {
                for (int i = 0; i < TOTAL_RELATED; i++)
                {
                    selFrames[i] = false;
                }
                selFrames[clicked.VideoPos] = true;
                curFrame = clicked.VideoPos;
            }

            UpdateKeyframeBorder();
        }

        private void Keyframe_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            for (int j = 0; j < TOTAL_RELATED; j++)
            {
                if (selFrames[j])
                {
                    for (int i = 0; i < TOTAL_RELATED; i++)
                    {
                        if (KfList[i].KfVidPos == j)
                        {
                            KfList[i].KfImage = submitImage;
                            mainWnd.SubmitResult(KfList[i].KfName, SUBMIT_TYPE.SHOT_ID, SUBMIT_FROM.MASTERSHOT_NEAREST1000_VIEWER);
                        }
                    }
                }
            }
        }
    }
}
