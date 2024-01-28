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
    public partial class VideoViewer : UserControl
    {
        static int NUM_RELATED = Settings.Default.DISPLAY_NUM_RELATED_SHOTS_TO_SHOW;
        static int NUM_ITEM_PER_LINE = Settings.Default.DISPLAY_NUM_ITEM_PER_LINE_TEMPORAL_VIEW;
        static int TOTAL_RELATED = NUM_RELATED * 2 + 1;
        //static string LNK_THUMB = Settings.Default.DATASET_LINK_THUMBNAILS_SHOTS;
        static string LNK_THUMB_V3C1 = Settings.Default.DATASET_LINK_THUMBNAILS_SHOTS_V3C1;
        static string LNK_THUMB_V3C2 = Settings.Default.DATASET_LINK_THUMBNAILS_SHOTS_V3C2;
        static int VIDEOS_NUM_V3C1 = Settings.Default.DATASET_NUM_VIDEOS_V3C1;

        static string LNK_BLACK_IMG = Settings.Default.IMG_BLACK;
        static string LNK_SUBMITTED_IMG = Settings.Default.IMG_SUBMITTED;
        static int THUMBNAIL_WIDTH = 200;
        static int THUMBNAIL_HEIGHT = 150;

        MainWindow mainWnd;
        BitmapImage blackImage;
        BitmapImage submitImage;

        int numCandidates = 0;
        int curFrame = -1;
        List<bool> selFrames = new List<bool>();
        List<bool> candidateFrames = new List<bool>();

        double offset_x = 0;

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
        public void UpdateParent(MainWindow _mainWnd)
        {
            mainWnd = _mainWnd;
        }

        // this function is relax function, the shotID=-1 by default (not passed) means show the video content
        // if shotID!=-1 means show the video content and put the candidate shot in the middle of the view
        public void LoadCandidates(List<RenderItem> listCandidates, List<SubmittedItem> listSubmitted, int vidID, int shotID = -1)
        {
            for (int i = 0; i < TOTAL_RELATED; i++)
            {
                selFrames[i] = false;
                candidateFrames[i] = false;
            }

            curFrame = 0;

            String LNK_THUMB = "";
            if (Convert.ToInt32(vidID) <= VIDEOS_NUM_V3C1)
                LNK_THUMB = LNK_THUMB_V3C1;
            else
                LNK_THUMB = LNK_THUMB_V3C2;

            string vidName = Globals.idx2vid[vidID].ToString();
            var files = Directory.GetFiles(LNK_THUMB, vidName + "*.jpg");

            int begSID = Math.Max(shotID - NUM_RELATED, 1);
            int endSID = Math.Min(begSID + 2*NUM_RELATED, files.Length);

            numCandidates = 0;
            for (int i = begSID; i <= endSID; i++)
            {
                string vid_shot = Globals.trec2marine[vidID.ToString().PadLeft(5, '0') + '_' + i].ToString();
                string linkToThumbnail = LNK_THUMB + vid_shot + ".jpg";

                if (File.Exists(linkToThumbnail))
                {
                    KfList[numCandidates].KfName = vidID + "-" + i;
                    KfList[numCandidates].KfVidID = Convert.ToInt32(vidID);
                    KfList[numCandidates].KfShotID = i;
                    KfList[numCandidates].KfVidPos = numCandidates;

                    if (i == shotID)
                    {
                        curFrame = numCandidates;                        
                    }

                    bool isSubmitted = false;
                    for (int j = 0; j < listSubmitted.Count; j++)
                    {
                        if ((KfList[numCandidates].KfVidID == listSubmitted[j].vidID) && (KfList[numCandidates].KfShotID == listSubmitted[j].shotID))
                        {
                            KfList[numCandidates].KfImage = submitImage;
                            isSubmitted = true;
                        }
                    }
                    if (!isSubmitted)
                    {
                        for (int j = 0; j < listCandidates.Count; j++)
                        {
                            if ((listCandidates[j].ID == i) && (listCandidates[j].score > 0))
                            {
                                candidateFrames[numCandidates] = true;
                                break;
                            }
                        }
                        ThreadPool.QueueUserWorkItem(LoadImage, new LoadingItem(linkToThumbnail, numCandidates));
                    }
                    numCandidates++;
                }
            }

            // update black image for empty area
            for (int i = numCandidates; i < TOTAL_RELATED; i++)
            {
                KfList[i].KfImage = blackImage;
                KfList[i].KfName = "None";
                KfList[i].KfVidID = -1;
                KfList[i].KfShotID = -1;
                KfList[i].KfVidPos = i;
            }

            // update border and scroll the view to top
            UpdateKeyframeBorder();
            int numline = TOTAL_RELATED / NUM_ITEM_PER_LINE;
            if (TOTAL_RELATED % NUM_ITEM_PER_LINE > 0)
            {
                numline++;
            }
            int curline = curFrame / NUM_ITEM_PER_LINE;
            scrViewer.ScrollToVerticalOffset((scrViewer.ExtentHeight/numline)*curline);
            offset_x = (scrViewer.VerticalOffset / (scrViewer.ExtentHeight - scrViewer.ViewportHeight));
            if (Double.IsNaN(offset_x))
            {
                offset_x = 0;
            }
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

        public VideoViewer()
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
            offset_x = 0;
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
            mainWnd.PlayVideo(clicked.VideoName, false, false);
            // Ctrl + Click -> add clicked one to selected set
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                if (clicked.VideoPos < numCandidates)
                {
                    selFrames[clicked.VideoPos] = !selFrames[clicked.VideoPos];
                    if (selFrames[clicked.VideoPos])
                        curFrame = clicked.VideoPos;
                }
            }
            // Shift + Click -> add a range of selection to selected set
            else if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            {
                if (curFrame == -1)
                {
                    if (clicked.VideoPos < numCandidates)
                    {
                        selFrames[clicked.VideoPos] = true;
                        curFrame = clicked.VideoPos;
                    }
                }
                else
                {
                    int max = Math.Max(curFrame, clicked.VideoPos);
                    max = Math.Min(max, numCandidates - 1);
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
                if (clicked.VideoPos < numCandidates)
                {
                    for (int i = 0; i < TOTAL_RELATED; i++)
                    {
                        selFrames[i] = false;
                    }
                    selFrames[clicked.VideoPos] = true;
                    curFrame = clicked.VideoPos;
                }
            }
            UpdateKeyframeBorder();
        }

        public void UpdateSubmitted(SubmittedItem Submitted)
        {
            for (int i = 0; i < TOTAL_RELATED; i++)
            {
                if ((KfList[i].KfVidID == Submitted.vidID) && (KfList[i].KfShotID == Submitted.shotID))
                {
                    KfList[i].KfImage = submitImage;
                }
            }
        }

        private void Keyframe_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            for (int j = 0; j < TOTAL_RELATED; j++)
            {
                if (selFrames[j])
                {
                    for (int i = 0; i < numCandidates; i++)
                    {
                        if (KfList[i].KfVidPos == j)
                        {
                            KfList[i].KfImage = submitImage;
                            mainWnd.SubmitResult(KfList[i].KfName, SUBMIT_TYPE.SHOT_ID, SUBMIT_FROM.VIDEO_MASTERSHOT_VIEWER);
                        }
                    }
                }
            }
        }

        public string GetSelectedItem()
        {
            for (int i = 0; i < numCandidates; i++)
            {
                if (KfList[i].KfVidPos == curFrame)
                {
                    return KfList[i].KfName;
                }
            }
            return "";
        }

        private void ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            double newoffset = (scrViewer.VerticalOffset / (scrViewer.ExtentHeight - scrViewer.ViewportHeight));
            if (Double.IsNaN(newoffset))
            {
                newoffset = 0;
            }
            if (Math.Abs(newoffset - offset_x) >= 0.01)
            {
                mainWnd.LogScrolling("videoContext", String.Format("{0:0.0000}", newoffset));
                offset_x = newoffset;
                if (Double.IsNaN(offset_x))
                {
                    offset_x = 0;
                }
            }
        }
    }
}
