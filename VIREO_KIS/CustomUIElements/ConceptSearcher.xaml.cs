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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VIREO_KIS.Properties;
using System.Windows.Controls.Primitives;

using System.Text.Json;
using VIREO_KIS.BasicElements;
using System.Threading;
using IO.Swagger.Model;


namespace VIREO_KIS
{
    /// <summary>
    /// Interaction logic for ConceptSearcher.xaml
    /// </summary>
    public partial class ConceptSearcher : UserControl
    {
        string LNK_TO_LIST = Settings.Default.EMBEDDING_LINK_DICTIONARY;

        AllSearchPanel containerPanel;
        ConceptSearchResult result;

        string[] lstConcept;

        string test_performance_path = Settings.Default.PERFORMANCE_DATA_PATH;
        private Dictionary<string, string> competitionDict = new Dictionary<string, string>();


        public object LayoutRoot { get; private set; }

        public void UpdateParent(AllSearchPanel _mainPanel)
        {
            containerPanel = _mainPanel;
        }

        public void Clear()
        {
            result.Clear();
            ClearInput();
            InitCurEvaluationRuns();
        }

        public void InitCurEvaluationRuns()
        {
            ThreadPool.QueueUserWorkItem(o =>
            {
                // Get the current tasks
                List<ApiClientEvaluationInfo> cur_eval_info_list =
                    containerPanel.mainWnd.evaluation_client_api.GetApiV2ClientEvaluationList(containerPanel.mainWnd.dres_session_id);
                foreach (ApiClientEvaluationInfo eval_info in cur_eval_info_list)
                {
                    competitionDict[eval_info.Name] = eval_info.Id;
                };
            });

            submission_task.ItemsSource = competitionDict;
            submission_task.DisplayMemberPath = "Key";
            submission_task.SelectedValuePath = "Value";
        }


        public ConceptSearcher()
        {
            result = new ConceptSearchResult();
            //lstConcept = File.ReadAllLines(LNK_TO_LIST);
            lstConcept = new string[0];
            InitializeComponent();
            model_Type.ItemsSource = Enum.GetValues(typeof(MODEL_TYPE));
            search_Type.ItemsSource = Enum.GetValues(typeof(SEARCH_TYPE));
        }
        
        private void OnModelTypeChange(object sender, SelectionChangedEventArgs e) 
        {
            result.model_Type = (MODEL_TYPE)model_Type.SelectedItem;
        }

        private void OnSearchTypeChange(object sender, SelectionChangedEventArgs e)
        {
            result.search_Type = (SEARCH_TYPE)search_Type.SelectedItem;
        }

        public bool HasResult()
        {
            return result.HasResult();
        }

        private string WelformString(string s)
        {
            if (s == null || s == "")
            {
                return "";
            }
            else
            {
                while (s.Contains("  "))
                {
                    s = s.Replace("  ", " ");
                }
                s = s.Trim();
            }
            return s;
        }

        void ClearInput()
        {
            //if (!(txt_Quantity.Editor == null))
            //    txt_Quantity.Editor.Text = "";
            if (!(txt_Subject.Editor == null))
                txt_Subject.Editor.Text = "";
            //if (!(txt_Object.Editor == null))
            //    txt_Object.Editor.Text = "";
            //if (!(txt_NOT.Editor == null))
            //    txt_NOT.Editor.Text = "";
            //if (!(txt_Time.Editor == null))
            //    txt_Time.Editor.Text = "";
            //if (!(txt_Location.Editor == null))
            //    txt_Location.Editor.Text = "";
            //if (!(txt_Predicate.Editor == null))
            //    txt_Predicate.Editor.Text = "";
        }


        private bool HasValue(string s)
        {
            if (s == null || s == "")
                return false;
            else
                return true;
        }

        private void btn_Search_Click(object sender, RoutedEventArgs e)
        {
            List<string> query = new List<string>();
            List<LOGIC> logic = new List<LOGIC>();

            //TODO:rever this line
            //string s_Quantity = WelformString(model_Type.Text);
            //string test_query = "cat and dog";
            //string s_Quantity = WelformString(test_query);
            //if (HasValue(s_Quantity))
            //{
            //    query.Add(s_Quantity);
            //    if (s_Quantity.Contains('/') && s_Quantity.Contains(','))
            //    {
            //        MessageBox.Show("Can not process both AND (\"/\") and OR (\",\") in Quantity phrase!");
            //        return;
            //    }
            //    if (s_Quantity.Contains('/') && !s_Quantity.Contains(','))
            //    {
            //        logic.Add(LOGIC.OR);
            //    }
            //    if (s_Quantity.Contains(',') && !s_Quantity.Contains('/'))
            //    {
            //        logic.Add(LOGIC.AND);
            //    }
            //    if (!s_Quantity.Contains('/') && !s_Quantity.Contains(','))
            //    {
            //        logic.Add(LOGIC.NONE);
            //    }
            //}

            string s_Subject = WelformString(txt_Subject.Editor.Text);
            if (HasValue(s_Subject))
            {
                query.Add(s_Subject);
                if (s_Subject.Contains('/') && s_Subject.Contains(','))
                {
                    MessageBox.Show("Can not process both AND (\"/\") and OR (\",\") in Subject phrase!");
                    return;
                }
                if (s_Subject.Contains('/') && !s_Subject.Contains(','))
                {
                    logic.Add(LOGIC.OR);
                }
                if (s_Subject.Contains(',') && !s_Subject.Contains('/'))
                {
                    logic.Add(LOGIC.AND);
                }
                if (!s_Subject.Contains('/') && !s_Subject.Contains(','))
                {
                    logic.Add(LOGIC.NONE);
                }
            }

            //string s_Predicate = WelformString(txt_Predicate.Editor.Text);
            //if (HasValue(s_Predicate))
            //{
            //    query.Add(s_Predicate);
            //    if (s_Predicate.Contains('/') && s_Predicate.Contains(','))
            //    {
            //        MessageBox.Show("Can not process both AND (\"/\") and OR (\",\") in Predicate phrase!");
            //        return;
            //    }
            //    if (s_Predicate.Contains('/') && !s_Predicate.Contains(','))
            //    {
            //        logic.Add(LOGIC.OR);
            //    }
            //    if (s_Predicate.Contains(',') && !s_Predicate.Contains('/'))
            //    {
            //        logic.Add(LOGIC.AND);
            //    }
            //    if (!s_Predicate.Contains('/') && !s_Predicate.Contains(','))
            //    {
            //        logic.Add(LOGIC.NONE);
            //    }
            //}

            //string s_Object = WelformString(txt_Object.Editor.Text);
            //if (HasValue(s_Object))
            //{
            //    query.Add(s_Object);
            //    if (s_Object.Contains('/') && s_Object.Contains(','))
            //    {
            //        MessageBox.Show("Can not process both AND (\"/\") and OR (\",\") in Object phrase!");
            //        return;
            //    }
            //    if (s_Object.Contains('/') && !s_Object.Contains(','))
            //    {
            //        logic.Add(LOGIC.OR);
            //    }
            //    if (s_Object.Contains(',') && !s_Object.Contains('/'))
            //    {
            //        logic.Add(LOGIC.AND);
            //    }
            //    if (!s_Object.Contains('/') && !s_Object.Contains(','))
            //    {
            //        logic.Add(LOGIC.NONE);
            //    }
            //}

            //string s_Time = WelformString(txt_Time.Editor.Text);
            //if (HasValue(s_Time))
            //{
            //    query.Add(s_Time);
            //    if (s_Time.Contains('/') && s_Time.Contains(','))
            //    {
            //        MessageBox.Show("Can not process both AND (\"/\") and OR (\",\") in Time phrase!");
            //        return;
            //    }
            //    if (s_Time.Contains('/') && !s_Time.Contains(','))
            //    {
            //        logic.Add(LOGIC.OR);
            //    }
            //    if (s_Time.Contains(',') && !s_Time.Contains('/'))
            //    {
            //        logic.Add(LOGIC.AND);
            //    }
            //    if (!s_Time.Contains('/') && !s_Time.Contains(','))
            //    {
            //        logic.Add(LOGIC.NONE);
            //    }
            //}

            //string s_Location = WelformString(txt_Location.Editor.Text);
            //if (HasValue(s_Location))
            //{
            //    query.Add(s_Location);
            //    if (s_Location.Contains('/') && s_Location.Contains(','))
            //    {
            //        MessageBox.Show("Can not process both AND (\"/\") and OR (\",\") in Location phrase!");
            //        return;
            //    }
            //    if (s_Location.Contains('/') && !s_Location.Contains(','))
            //    {
            //        logic.Add(LOGIC.OR);
            //    }
            //    if (s_Location.Contains(',') && !s_Location.Contains('/'))
            //    {
            //        logic.Add(LOGIC.AND);
            //    }
            //    if (!s_Location.Contains('/') && !s_Location.Contains(','))
            //    {
            //        logic.Add(LOGIC.NONE);
            //    }
            //}

            string s_NOT = WelformString(txt_NOT.Editor.Text);
            if (HasValue(s_NOT))
            {
                query.Add(s_NOT);
                logic.Add(LOGIC.NOT);
            }

            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();

            result.Search(query, logic);

            Console.WriteLine($"Embedding search execution time: {watch.ElapsedMilliseconds} ms");
            watch.Stop();
            var watch1 = new System.Diagnostics.Stopwatch();
            watch1.Start();

            containerPanel.UpdateResult(result.Get(), RESULT_UPDATE.FROM_EMBEDDING, result.HasResult(), string.Join(" ", query));
            Console.WriteLine($"Post process execution time: {watch1.ElapsedMilliseconds} ms");

            watch1.Stop();

        }

        private void btn_Search_Clicks_debug(object sender, RoutedEventArgs e)
        {
            string jsonString = File.ReadAllText(test_performance_path+ "query_string_test.json");
            string performancePath = test_performance_path + "performance_hnsw_socket_new.csv";
            File.WriteAllText(performancePath, "query|embedding_time|postprocess_time"+Environment.NewLine);
            List<TestQuery> queries = JsonSerializer.Deserialize<List<TestQuery>>(jsonString);

            //for(int i = 0; i < 1; i++)
            foreach (TestQuery query_obj in queries)
            {
                string test_query = query_obj.query;
                //string test_query = "cat and dog running by the waterfall during night it is rainy";
                List<string> query = new List<string>();
                List<LOGIC> logic = new List<LOGIC>();

                //TODO:rever this line
                //string s_Quantity = WelformString(txt_Quantity.Editor.Text);
                string s_Quantity = WelformString(test_query);
                if (HasValue(s_Quantity))
                {
                    query.Add(s_Quantity);
                    if (s_Quantity.Contains('/') && s_Quantity.Contains(','))
                    {
                        MessageBox.Show("Can not process both AND (\"/\") and OR (\",\") in Quantity phrase!");
                        return;
                    }
                    if (s_Quantity.Contains('/') && !s_Quantity.Contains(','))
                    {
                        logic.Add(LOGIC.OR);
                    }
                    if (s_Quantity.Contains(',') && !s_Quantity.Contains('/'))
                    {
                        logic.Add(LOGIC.AND);
                    }
                    if (!s_Quantity.Contains('/') && !s_Quantity.Contains(','))
                    {
                        logic.Add(LOGIC.NONE);
                    }
                }

                string s_Subject = WelformString(txt_Subject.Editor.Text);
                if (HasValue(s_Subject))
                {
                    query.Add(s_Subject);
                    if (s_Subject.Contains('/') && s_Subject.Contains(','))
                    {
                        MessageBox.Show("Can not process both AND (\"/\") and OR (\",\") in Subject phrase!");
                        return;
                    }
                    if (s_Subject.Contains('/') && !s_Subject.Contains(','))
                    {
                        logic.Add(LOGIC.OR);
                    }
                    if (s_Subject.Contains(',') && !s_Subject.Contains('/'))
                    {
                        logic.Add(LOGIC.AND);
                    }
                    if (!s_Subject.Contains('/') && !s_Subject.Contains(','))
                    {
                        logic.Add(LOGIC.NONE);
                    }
                }

                //string s_Predicate = WelformString(txt_Predicate.Editor.Text);
                //if (HasValue(s_Predicate))
                //{
                //    query.Add(s_Predicate);
                //    if (s_Predicate.Contains('/') && s_Predicate.Contains(','))
                //    {
                //        MessageBox.Show("Can not process both AND (\"/\") and OR (\",\") in Predicate phrase!");
                //        return;
                //    }
                //    if (s_Predicate.Contains('/') && !s_Predicate.Contains(','))
                //    {
                //        logic.Add(LOGIC.OR);
                //    }
                //    if (s_Predicate.Contains(',') && !s_Predicate.Contains('/'))
                //    {
                //        logic.Add(LOGIC.AND);
                //    }
                //    if (!s_Predicate.Contains('/') && !s_Predicate.Contains(','))
                //    {
                //        logic.Add(LOGIC.NONE);
                //    }
                //}

                //string s_Object = WelformString(txt_Object.Editor.Text);
                //if (HasValue(s_Object))
                //{
                //    query.Add(s_Object);
                //    if (s_Object.Contains('/') && s_Object.Contains(','))
                //    {
                //        MessageBox.Show("Can not process both AND (\"/\") and OR (\",\") in Object phrase!");
                //        return;
                //    }
                //    if (s_Object.Contains('/') && !s_Object.Contains(','))
                //    {
                //        logic.Add(LOGIC.OR);
                //    }
                //    if (s_Object.Contains(',') && !s_Object.Contains('/'))
                //    {
                //        logic.Add(LOGIC.AND);
                //    }
                //    if (!s_Object.Contains('/') && !s_Object.Contains(','))
                //    {
                //        logic.Add(LOGIC.NONE);
                //    }
                //}

                //string s_Time = WelformString(txt_Time.Editor.Text);
                //if (HasValue(s_Time))
                //{
                //    query.Add(s_Time);
                //    if (s_Time.Contains('/') && s_Time.Contains(','))
                //    {
                //        MessageBox.Show("Can not process both AND (\"/\") and OR (\",\") in Time phrase!");
                //        return;
                //    }
                //    if (s_Time.Contains('/') && !s_Time.Contains(','))
                //    {
                //        logic.Add(LOGIC.OR);
                //    }
                //    if (s_Time.Contains(',') && !s_Time.Contains('/'))
                //    {
                //        logic.Add(LOGIC.AND);
                //    }
                //    if (!s_Time.Contains('/') && !s_Time.Contains(','))
                //    {
                //        logic.Add(LOGIC.NONE);
                //    }
                //}

                //string s_Location = WelformString(txt_Location.Editor.Text);
                //if (HasValue(s_Location))
                //{
                //    query.Add(s_Location);
                //    if (s_Location.Contains('/') && s_Location.Contains(','))
                //    {
                //        MessageBox.Show("Can not process both AND (\"/\") and OR (\",\") in Location phrase!");
                //        return;
                //    }
                //    if (s_Location.Contains('/') && !s_Location.Contains(','))
                //    {
                //        logic.Add(LOGIC.OR);
                //    }
                //    if (s_Location.Contains(',') && !s_Location.Contains('/'))
                //    {
                //        logic.Add(LOGIC.AND);
                //    }
                //    if (!s_Location.Contains('/') && !s_Location.Contains(','))
                //    {
                //        logic.Add(LOGIC.NONE);
                //    }
                //}

                string s_NOT = WelformString(txt_NOT.Editor.Text);
                if (HasValue(s_NOT))
                {
                    query.Add(s_NOT);
                    logic.Add(LOGIC.NOT);
                }

                var watch = new System.Diagnostics.Stopwatch();
                watch.Start();

                result.Search(query, logic);

                //Console.WriteLine($"Embedding search execution time: {watch.ElapsedMilliseconds} ms");
                long embedding_time = watch.ElapsedMilliseconds;
                watch.Stop();
                var watch1 = new System.Diagnostics.Stopwatch();
                watch1.Start();

                containerPanel.UpdateResult(result.Get(), RESULT_UPDATE.FROM_EMBEDDING, result.HasResult(), string.Join(" ", query));
                //Console.WriteLine($"Post process execution time: {watch1.ElapsedMilliseconds} ms");

                long postprocess_time = watch1.ElapsedMilliseconds;


                watch1.Stop();

                File.AppendAllText(performancePath, $"{test_query}|{embedding_time}|{postprocess_time}" + Environment.NewLine);

            }

        }

        private void OnSubmissionTaskChange(object sender, SelectionChangedEventArgs e)
        {
            containerPanel.SetSubmissionTask((string)submission_task.SelectedValue);
        }

        public void btn_Submit_KIS_Click(object sender, EventArgs e)
        {
            //containerPanel.mainWnd.SubmitSelectedShot();
        }


        private void btn_Answer_Click(object sender, RoutedEventArgs e)
        {
            //search_panel.SubmitQAAnswer(txt_answer.Editor.Text);
        }

        private void btn_refresh_task_Click(object sender, RoutedEventArgs e)
        {
            InitCurEvaluationRuns();
        }

    }
}
