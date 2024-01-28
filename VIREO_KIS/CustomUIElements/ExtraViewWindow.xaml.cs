using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace VIREO_KIS
{
    /// <summary>
    /// Interaction logic for Top1000.xaml
    /// </summary>
    public partial class ExtraViewWindow : Window
    {
        MainWindow mainWnd;

        public ExtraViewWindow()
        {
            InitializeComponent();
            this.Top = 0;
            this.Left = 0;
            this.Width = System.Windows.SystemParameters.PrimaryScreenWidth * 0.8;
            this.Height = System.Windows.SystemParameters.PrimaryScreenHeight * 0.7;
            vdv_ExtraViewer.UpdateParent(this);
        }

        public void UpdateParent(MainWindow _mainWnd)
        {
            mainWnd = _mainWnd;
        }

        public void LogScrolling(string s1, string s2)
        {
            mainWnd.LogScrolling(s1, s2);
        }

        public void LoadCandidates(string shotID, List<SubmittedItem> lst_Submit)
        {
            vdv_ExtraViewer.LoadCandidates(shotID, lst_Submit);
        }

        public void PlayVideo(string linkToVideo, bool needRefreshVideoViewer)
        {
            mainWnd.PlayVideoFrom1000Nearest(linkToVideo, needRefreshVideoViewer);
        }

        public void SubmitResult(string linkToVideo, SUBMIT_TYPE smt, SUBMIT_FROM smf)
        {
            mainWnd.SubmitResult(linkToVideo, smt, smf);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            mainWnd.CloseExtraView();
            base.OnClosing(e);
        }
    }
}