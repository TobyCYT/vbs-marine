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
    /// Interaction logic for AllSearchPanel.xaml
    /// </summary>
    public partial class AllSearchPanel : UserControl
    {
        FusedResult fusedResult;

        public AllSearchPanel()
        {
            InitializeComponent();
            src_Color.UpdateParent(this);
            src_Meta.UpdateParent(this);
            src_Concept.UpdateParent(this);            
            fusedResult = new FusedResult();
        }

        public void UpdateResult(double[] res, RESULT_UPDATE ru, bool hasResult, string add_log = "", string noted = "")
        {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            fusedResult.Update(res, ru, hasResult);
            Console.WriteLine($"fusedResult execution time: {watch.ElapsedMilliseconds} ms");
            //long embedding_time = watch.ElapsedMilliseconds;
            watch.Stop();

            var watch1 = new System.Diagnostics.Stopwatch();
            watch1.Start();
            mainWnd.UpdateResultViewer(ru, this.Name + "," + add_log, noted);
            Console.WriteLine($"UpdateResultViewer execution time: {watch1.ElapsedMilliseconds} ms");
            //long embedding_time = watch.ElapsedMilliseconds;
            watch1.Stop();
        }

        public double[] GetResult()
        {
            return fusedResult.CalculateFinalScore();
        }

        public bool HasResult()
        {
            return fusedResult.HasResult();
        }

        public MainWindow mainWnd;
        public void UpdateParent(MainWindow _mainWnd)
        {
            mainWnd = _mainWnd;
        }

        public void Clear()
        {      
            src_Concept.Clear();
            src_Meta.Clear();
            src_Color.Clear();
            fusedResult.Clear();
        }

        private void btn_Clear_Click(object sender, RoutedEventArgs e)
        {
            this.Clear();
            mainWnd.UpdateResultViewer(RESULT_UPDATE.FROM_CLEAR_PANEL, this.Name);
        }

        public List<string> GetUsing()
        {
            List<string> toReturn = new List<string>();
            string text = src_Meta.GetUsing();
            if (src_Concept.HasResult())
            {
                if (text == "")
                {
                    text = "EMBEDDING";
                }
                else
                {
                    text = text + ",EMBEDDING";
                }
            }
            toReturn.Add(text);
            string color = "";
            if (src_Color.HasResult())
            {
                color = "Color";
            }
            toReturn.Add(color);
            return toReturn;
        }

        private void src_Concept_Loaded(object sender, RoutedEventArgs e)
        {

        }

        public void SetSubmissionTask(string task_id)
        {
            mainWnd.SetSubmissionTask(task_id);
        }
    }
}
