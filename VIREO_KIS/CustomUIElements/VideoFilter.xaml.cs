using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;
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
using VIREO_KIS.Properties;

namespace VIREO_KIS
{
    /// <summary>
    /// Interaction logic for MetadataSearcher.xaml
    /// </summary>
    public partial class VideoFilter : UserControl
    {
        MainWindow mainWnd;

        string linkToFolder = Settings.Default.FILTER_FOLDER;
        int numItem = 0;
        List<bool> check = new List<bool>();
        List<string> name;
        public void Clear()
        {
            if (!mainWnd.GetFilterMode())
            {
                mainWnd.SetFilterMode();
                lbl_Filter.Content = "Filter:";
            }
            for (var i = 0; i < check.Count; i++)
            {
                check[i] = false;
                CheckBox temp = (CheckBox)stp_CheckboxList.FindName("chk_" + i.ToString());
                temp.IsChecked = false;
            }
        }

        public void UpdateParent(MainWindow _mainWnd)
        {
            mainWnd = _mainWnd;
        }

        public VideoFilter()
        {
            InitializeComponent();
        }

        public void AddAllFilters()
        {
            if (Directory.Exists(linkToFolder))
            {
                var files = Directory.GetFiles(linkToFolder);
                check = new List<bool>();
                name = new List<string>();
                numItem = 0;
                foreach (var f in files)
                {
                    CheckBox temp = new CheckBox();
                    temp.Name = "chk_" + numItem.ToString();
                    temp.Content = System.IO.Path.GetFileName(f).Split('.')[0];
                    //temp.FontSize = 14;
                    temp.Margin = new Thickness(7);
                    temp.IsChecked = false;
                    temp.Checked += Temp_Checked;
                    temp.Unchecked += Temp_Unchecked;
                    stp_CheckboxList.Children.Add(temp);
                    stp_CheckboxList.RegisterName("chk_" + numItem.ToString(), temp);

                    var lines = File.ReadAllLines(f);
                    List<int> vID = new List<int>();
                    foreach (var l in lines)
                    {
                        vID.Add(Convert.ToInt32(l));
                    }
                    mainWnd.AddFilter(vID, numItem);
                    check.Add(false);
                    name.Add(System.IO.Path.GetFileName(f).Split('.')[0]);
                    numItem++;
                }
            }
        }

        public bool IsUsed()
        {
            for (var i = 0; i < check.Count; i++)
            {
                if (check[i])
                    return true;
            }
            return false;
        }

        private string GetLog()
        {
            string log = "";
            bool isFiltering = mainWnd.GetFilterMode();
            if (isFiltering)
            {
                log += "filter,";
            }
            else
            {
                log += "findin,";
            }
            for (var i = 0; i < check.Count; i++)
            {
                log += name[i] + ":" + check[i].ToString();
                if (i != check.Count - 1)
                {
                    log += ",";
                }
            }
            return log;
        }

        private void Temp_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            int id = Convert.ToInt32(chk.Name.Split('_')[1]);
            mainWnd.SetFilter(id, false);
            check[id] = false;
            mainWnd.UpdateResultViewer(RESULT_UPDATE.FROM_FILTER, GetLog());
        }

        private void Temp_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            int id = Convert.ToInt32(chk.Name.Split('_')[1]);
            mainWnd.SetFilter(id, true);
            check[id] = true;
            mainWnd.UpdateResultViewer(RESULT_UPDATE.FROM_FILTER, GetLog());
        }

        private void Label_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            bool isFiltering = mainWnd.SetFilterMode();
            if (isFiltering)
            {
                lbl_Filter.Content = "Filter:";
                mainWnd.UpdateResultViewer(RESULT_UPDATE.FROM_FILTER, GetLog());
            }
            else
            {
                lbl_Filter.Content = "Find in:";
                mainWnd.UpdateResultViewer(RESULT_UPDATE.FROM_FILTER, GetLog());
            }
        }
    }
}
