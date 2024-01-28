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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VIREO_KIS
{
    /// <summary>
    /// Interaction logic for ButtonSearcher.xaml
    /// </summary>
    public partial class ButtonSearcher : UserControl
    {
        MainWindow mainWnd;
        public void UpdateParent(MainWindow _mainWnd)
        {
            mainWnd = _mainWnd;
        }

        public ButtonSearcher()
        {
            InitializeComponent();
        }

        private void btn_New_Click(object sender, RoutedEventArgs e)
        {
            mainWnd.NewSearch();
        }

        private void btn_Clear_Click(object sender, RoutedEventArgs e)
        {
            mainWnd.ClearSearch();
        }

        private void btn_End_Click(object sender, RoutedEventArgs e)
        {
            mainWnd.EndSearch();
        }

        private void btn_AVSResult_Click(object sender, RoutedEventArgs e)
        {
            bool res = mainWnd.SwitchRanking();
            if (res)
            {
                btn_AVSResult.Content = "Ungroup shots";
            }
            else
            {
                btn_AVSResult.Content = "Group shots";
            }
        }

        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            mainWnd.Close();
        }

        private void btn_FindNearest_Click(object sender, RoutedEventArgs e)
        {
            mainWnd.ViewNearestShots();
        }
    }
}
