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
    /// Interaction logic for WeightingSearcher.xaml
    /// </summary>
    public partial class WeightingSearcher : UserControl
    {
        MainWindow mainWnd;
        public void UpdateParent(MainWindow _mainWnd)
        {
            mainWnd = _mainWnd;
        }

        public void Clear()
        {
            sld_fusion_weight.Value = 3;
        }

        public void Enable()
        {
            sld_fusion_weight.IsEnabled = true;
        }

        public void Disable()
        {
            sld_fusion_weight.IsEnabled = false;
        }

        public WeightingSearcher()
        {
            InitializeComponent();
            sld_fusion_weight.ValueChanged += Slider_ValueChanged;
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            string toLog = "fusion weight:" + sld_fusion_weight.Value.ToString();
            //mainWnd.UpdateResultViewer(RESULT_UPDATE.FROM_FUSION_WEIGHT, toLog);
        }

        public double GetFusionWeight()
        {
            return sld_fusion_weight.Value;
        }

        private void sld_fusion_weight_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }
    }
}
