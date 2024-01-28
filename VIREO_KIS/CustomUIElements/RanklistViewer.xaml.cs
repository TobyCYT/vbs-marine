using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.ComponentModel;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using VIREO_KIS.Properties;
using System.Collections;

namespace VIREO_KIS
{
    /// <summary>
    /// Interaction logic for KeyframeViewer.xaml
    /// </summary>
    public partial class RanklistViewer : UserControl
    {
        static int NUM_RES = Settings.Default.DISPLAY_NUM_RESULTS_TO_SHOW;
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

        Border old_border = new Border();

        List<bool> selFrames = new List<bool>();

        int curFrame = 0;
        double offset = 0;
        
        public void UpdateParent(MainWindow _mainWnd)
        {
            mainWnd = _mainWnd;
        }
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

        public List<ImageSource> ListKf = new List<ImageSource>();

        public void LoadCandidates(List<MasterShot> _KfName, List<SubmittedItem> listSubmitted, bool scroll = true)
        {
            if (scroll)
            {
                scrViewer.ScrollToTop();
                offset = 0;
            }
            for (int i = 0; i < NUM_RES; i++)
            {
                selFrames[i] = false;

                String LNK_THUMB = "";
                if (Convert.ToInt32(_KfName[i].VideoID) <= VIDEOS_NUM_V3C1)
                    LNK_THUMB = LNK_THUMB_V3C1;
                else
                    LNK_THUMB = LNK_THUMB_V3C2;
                string shot_name = _KfName[i].VideoID.ToString().PadLeft(5, '0') + "_" + _KfName[i].ShotID;
                string linkToThumbnail = LNK_THUMB + Globals.trec2marine[shot_name] + ".jpg";
                KfList[i].KfName = _KfName[i].VideoID + "-" + _KfName[i].ShotID;
                KfList[i].KfVidID = _KfName[i].VideoID;
                KfList[i].KfShotID = _KfName[i].ShotID;
                KfList[i].KfVidPos = i;

                bool isSubmitted = false;
                for (int j = 0; j < listSubmitted.Count; j++)
                {
                    if ((KfList[i].KfVidID == listSubmitted[j].vidID) && (KfList[i].KfShotID == listSubmitted[j].shotID))
                    {
                        KfList[i].KfImage = submitImage;
                        isSubmitted = true;
                    }
                }

                if (!isSubmitted)
                {
                    ThreadPool.QueueUserWorkItem(LoadImage, new LoadingItem(linkToThumbnail, i));
                }                
            }
        }

        public void LoadImage(object obj)
        {
            LoadingItem item = (LoadingItem)obj;
            var bitmap = new BitmapImage();
            // check if path to item.link exists
            
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

        public RanklistViewer()
        {
            InitializeComponent();
            KfList = new ObservableCollection<KfItem>();
            blackImage = CreateImage(LNK_BLACK_IMG);
            submitImage = CreateImage(LNK_SUBMITTED_IMG);
            for (int i = 0; i < NUM_RES; i++)
            {
                KfItem temp = new KfItem();
                temp.KfImage = blackImage;
                temp.KfName = "None";
                temp.KfVidPos = i;
                temp.KfVidID = -1;
                temp.KfShotID = -1;
                KfList.Add(temp);
                selFrames.Add(false);
            }
            KfThumbnails.ItemsSource = KfList;
            offset = 0;
        }

        public void SetBorder(int newPos)
        {
            selFrames[newPos] = true;
            UpdateKeyframeBorder(newPos);
        }

        private void Keyframe_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Ctrl + Click -> add clicked one to selected set
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                Keyframe clicked = sender as Keyframe;
                mainWnd.PlayVideo(clicked.VideoName, true, true);
                if (clicked.VideoPos < NUM_RES)
                {
                    selFrames[clicked.VideoPos] = !selFrames[clicked.VideoPos];
                    if (selFrames[clicked.VideoPos])
                        curFrame = clicked.VideoPos;
                }
            }
            // Shift + Click -> add a range of selection to selected set
            else if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            {
                Keyframe clicked = sender as Keyframe;
                mainWnd.PlayVideo(clicked.VideoName, true, true);
                if (curFrame == -1)
                {
                    if (clicked.VideoPos < NUM_RES)
                    {
                        selFrames[clicked.VideoPos] = true;
                        curFrame = clicked.VideoPos;
                    }
                }
                else
                {
                    int max = Math.Max(curFrame, clicked.VideoPos);
                    max = Math.Min(max, NUM_RES - 1);
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
                Keyframe clicked = sender as Keyframe;
                mainWnd.PlayVideo(clicked.VideoName, true, true);
                if (clicked.VideoPos < NUM_RES)
                {
                    for (int i = 0; i < NUM_RES; i++)
                    {
                        selFrames[i] = false;
                    }
                    selFrames[clicked.VideoPos] = true;
                    curFrame = clicked.VideoPos;
                }
            }
            UpdateKeyframeBorder(curFrame);
        }

        private void Keyframe_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            for (int j = 0; j < NUM_RES; j++)
            {
                if (selFrames[j])
                {
                    for (int i = 0; i < NUM_RES; i++)
                    {
                        if (KfList[i].KfVidPos == j)
                        {
                            KfList[i].KfImage = submitImage;
                            mainWnd.SubmitResult(KfList[i].KfName, SUBMIT_TYPE.SHOT_ID, SUBMIT_FROM.RANKLIST_MASTERSHOT_VIEWER);
                        }
                    }
                }
            }
        }

        public void UpdateSubmitted(SubmittedItem Submitted)
        {
            for (int i = 0; i < NUM_RES; i++)
            {
                if ((KfList[i].KfVidID == Submitted.vidID) && (KfList[i].KfShotID == Submitted.shotID))
                {
                    KfList[i].KfImage = submitImage;
                }
            }
        }


        public void UpdateKeyframeBorder(int newPos)
        {
            curFrame = newPos;
            ItemsControl itm = this.FindName("KfThumbnails") as ItemsControl;
            for (int i = 0; i < NUM_RES; i++)
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
                        inner_border.BorderBrush = Brushes.Black;
                        inner_border.BorderThickness = new Thickness(0);
                    }
                }
            }
        }

        private void ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            double newoffset = (scrViewer.VerticalOffset / (scrViewer.ExtentHeight - scrViewer.ViewportHeight));
            if (Double.IsNaN(newoffset))
            {
                newoffset = 0;
            }
            if (Math.Abs(newoffset - offset) >= 0.01)
            {
                mainWnd.LogScrolling("rankedList", String.Format("{0:0.0000}", newoffset));
                offset = newoffset;
                if (Double.IsNaN(offset))
                {
                    offset = 0;
                }
            }
        }
    }
}