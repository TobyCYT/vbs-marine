using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VIREO_KIS.BasicElements;
using VIREO_KIS.Properties;

namespace VIREO_KIS
{
    public class ConceptSearchResult
    {
        static public string LNK_TO_LIST = Settings.Default.EMBEDDING_LINK_DICTIONARY;

        string EMBEDDING_PYTHON_LINK = Settings.Default.EMBEDDING_PYTHON_CALL;
        string EMBEDDING_LINK_RESULT = Settings.Default.EMBEDDING_LINK_RESULT;
        //string CONCEPT_PHASE_FOR_QUERY_RES = Settings.Default.CONCEPT_PHASE_FOR_QUERY_RES;
        //string CONCEPT_PHASE_CALL_LINK = Settings.Default.CONCEPT_PHASE_CALL_LINK;
        //string CONCEPT_RES_PATH = Settings.Default.CONCEPT_RES_PATH;
        string CONCEPT_PHASE_FOR_QUERY_RES;
        string CONCEPT_PHASE_CALL_LINK;
        string CONCEPT_RES_PATH;
        string PYTHON_LINK = Settings.Default.EMBEDDING_LINK_PYTHON;
        string INVERTED_LINK = Settings.Default.EMBEDDING_LINK_INVERTED_INDEX;

        static int NUM_SHOT = Settings.Default.DATASET_NUM_SHOTS;
        static int NUM_LOGIC = Settings.Default.EMBEDDING_NUM_LOGIC_MAX;

        double[][] partialResult = new double[NUM_LOGIC][];
        double[] finalResult = new double[NUM_SHOT];

        double CONCEPT_FUSION_RATIO = 0; // 0 for embedding seach,10 for concept seach

        double[] embeddingResult = new double[NUM_SHOT];
        double[] conceptResult = new double[NUM_SHOT];
        //string[] listConcept = File.ReadAllLines(LNK_TO_LIST);
        string[] listConcept = new string[0];

        public MODEL_TYPE model_Type { get; set; }
        public SEARCH_TYPE search_Type { get; set; }

    //Hashtable htConcept = new Hashtable();

    TcpSocket socket_client = new TcpSocket("localhost", 1122);

        bool hasResult = false;
        List<string> qrs = new List<string>();
        List<string> ids = new List<string>();

        AllSearchPanel containerPanel;

        public string CONCEPT_PHASE_FOR_QUERY_RES1 { get => CONCEPT_PHASE_FOR_QUERY_RES; set => CONCEPT_PHASE_FOR_QUERY_RES = value; }

        public void UpdateParent(AllSearchPanel _mainWnd)
        {
            containerPanel = _mainWnd;
        }

        public void Init() { }

        public void Clear()
        {
            for (int i = 0; i < NUM_LOGIC; i++)
            {
                Array.Clear(partialResult[i], 0, NUM_SHOT);
            }
            Array.Clear(finalResult, 0, NUM_SHOT);
            qrs.Clear();
            ids.Clear();
            hasResult = false;
        }

        public ConceptSearchResult()
        {
            for (int i = 0; i < NUM_LOGIC; i++)
            {
                partialResult[i] = new double[NUM_SHOT];
            }
            //for (int i = 0; i < listConcept.Length; i++) {
            //    htConcept.Add(listConcept[i], i);
            //}
            Array.Clear(finalResult, 0, NUM_SHOT);
            qrs.Clear();
            ids.Clear();
            hasResult = false;
        }


        public double[] Get()
        {
            return finalResult;
        }

        private void GetQueries(string toAdd, LOGIC lg)
        {
            List<string> results = new List<string>();
            List<string> newids = new List<string>();
            if (lg == LOGIC.AND)
            {
                var values = toAdd.Split(',').ToList();
                for (int i = 0; i < qrs.Count; i++)
                {
                    for (int j = 0; j < values.Count; j++)
                    {
                        results.Add(qrs[i] + " " + values[j]);
                        newids.Add(ids[i]);
                    }
                }
            }
            if (lg == LOGIC.OR)
            {
                var values = toAdd.Split('/').ToList();
                for (int j = 0; j < values.Count; j++)
                {
                    for (int i = 0; i < qrs.Count; i++)
                    {
                        results.Add(qrs[i] + " " + values[j]);
                        newids.Add(ids[i] + j.ToString());
                    }
                }
            }
            if (lg == LOGIC.NONE)
            {
                for (int i = 0; i < qrs.Count; i++)
                {
                    results.Add(qrs[i] + " " + toAdd);
                    newids.Add(ids[i]);
                }
            }
            qrs = results;
            ids = newids;
        }

        private void UpdateITVResultEmbedding_prev(string query)
        {
            Array.Clear(embeddingResult, 0, NUM_SHOT);

            if (query != "")
            {


                // delete existing result
                if (File.Exists(EMBEDDING_LINK_RESULT))
                {
                    File.Delete(EMBEDDING_LINK_RESULT);
                }
                var watch_1 = new System.Diagnostics.Stopwatch();
                watch_1.Start();

                //socket_client.sendQuery(query, ref embeddingResult);

                if (File.Exists(PYTHON_LINK))
                {
                    if (File.Exists(EMBEDDING_PYTHON_LINK))
                    {

                        // run new process and wait for finishing
                        string excute = EMBEDDING_PYTHON_LINK + " " + query + " ";
                        var processInfo = new ProcessStartInfo(PYTHON_LINK, excute)
                        {
                            CreateNoWindow = true,
                            UseShellExecute = false
                        };
                        Process proc;
                        if ((proc = Process.Start(processInfo)) == null)
                        {
                            throw new InvalidOperationException("??");
                        }
                        proc.WaitForExit();
                        proc.Close();


                    }
                    else
                    {
                        MessageBox.Show("Can not find Python coded file!");
                    }
                }
                else
                {
                    MessageBox.Show("Can not find Python excutable file!");
                }

                // load result from new file
                if (File.Exists(EMBEDDING_LINK_RESULT))
                {
                    hasResult = true;
                    var lines = File.ReadAllLines(EMBEDDING_LINK_RESULT);
                    foreach (var line in lines)
                    {
                        int index = Convert.ToInt32(line.Split(' ')[0]);
                        double score = Convert.ToDouble(line.Split(' ')[1]);
                        embeddingResult[index] += score;
                    }

                    System.Diagnostics.Debug.WriteLine($"Execution Time for calculating embeddings: {watch_1.ElapsedMilliseconds} ms");
                    // normallize
                    double min = embeddingResult.Min();
                    double range = embeddingResult.Max() - min;
                    if (range != 0)
                    {
                        for (int i = 0; i < NUM_SHOT; i++)
                        {
                            embeddingResult[i] = (embeddingResult[i] - min) / range;
                        }
                    }
                }


                // normallize
                //double min = embeddingResult.Min();
                //double range = embeddingResult.Max() - min;
                //if (range != 0)
                //{
                //    for (int i = 0; i < NUM_SHOT; i++)
                //    {
                //        embeddingResult[i] = (embeddingResult[i] - min) / range;
                //    }
                //}
                //socket_client.disconnect();
                //Console.WriteLine($"Execution Time for calculating embeddings: {watch_1.ElapsedMilliseconds} ms");
            }
        }

        private void UpdateITVResultEmbedding(string query)
        {
            Array.Clear(embeddingResult, 0, NUM_SHOT);

            if (query != "" && socket_client.connect())
            {
                socket_client.sendQuery(model_Type, search_Type, query, ref embeddingResult);


                // normallize
                double min = embeddingResult.Min();
                double range = embeddingResult.Max() - min;
                if (range != 0)
                {
                    for (int i = 0; i < NUM_SHOT; i++)
                    {
                        embeddingResult[i] = (embeddingResult[i] - min) / range;
                    }
                }
                socket_client.disconnect();
                //Console.WriteLine($"Execution Time for calculating embeddings: {watch_1.ElapsedMilliseconds} ms");
            }
        }


        private void UpdateITVResultConcept(string query)
        {
            Array.Clear(conceptResult, 0, NUM_SHOT);
            var subStrings = query.Split(' ');
            var watch_2 = new System.Diagnostics.Stopwatch();
            watch_2.Start();

            for (int i = 0; i < subStrings.Length; i++)
            {
                //if (htConcept.ContainsKey(subStrings[i]))
                //{
                //    string link_To_File = INVERTED_LINK + (htConcept[subStrings[i]]).ToString() + ".txt";
                //    if (File.Exists(link_To_File))
                //    {
                //        hasResult = true;
                //        var lines = File.ReadAllLines(link_To_File);
                //        foreach (var line in lines)
                //        {
                //            int index = Convert.ToInt32(line.Split(' ')[0]);
                //            double score = Convert.ToDouble(line.Split(' ')[1]);
                //            conceptResult[index] += score;
                //        }
                //    }

                //}
                // TODO: improve to actual server/database, now is bottlenecked here with around 380-490ms
                for (int j = 0; j < listConcept.Length; j++)
                {
                    if (subStrings[i] == listConcept[j])
                    {
                        string link_To_File = INVERTED_LINK + j.ToString() + ".txt";
                        if (File.Exists(link_To_File))
                        {
                            hasResult = true;
                            var lines = File.ReadAllLines(link_To_File);
                            foreach (var line in lines)
                            {
                                int index = Convert.ToInt32(line.Split(' ')[0]);
                                double score = Convert.ToDouble(line.Split(' ')[1]);
                                conceptResult[index] += score;
                            }
                        }
                        /*else
                        {
                            MessageBox.Show("Can not find inverted index for concept search result!");
                        }*/
                    }
                }
            }

            Console.WriteLine($"Execution Time for getting concepts: {watch_2.ElapsedMilliseconds} ms");
            watch_2.Stop();
            var watch_3 = new System.Diagnostics.Stopwatch();
            watch_3.Start();
            // normallize
            double min = conceptResult.Min();
            double range = conceptResult.Max() - min;
            if (range != 0)
            {
                for (int i = 0; i < NUM_SHOT; i++)
                {
                    conceptResult[i] = (conceptResult[i] - min) / range;
                }
            }

            Console.WriteLine($"Execution Time for calculation of normalization of concepts: {watch_3.ElapsedMilliseconds} ms");
            watch_3.Stop();

        }

        private void UpdateResultConceptPhaseSearch(string query)
        {
            Array.Clear(conceptResult, 0, NUM_SHOT);

            if (query != "")
            {
                // delete existing result
                if (File.Exists(CONCEPT_PHASE_FOR_QUERY_RES1))
                {
                    File.Delete(CONCEPT_PHASE_FOR_QUERY_RES1);
                }

                if (File.Exists(PYTHON_LINK))
                {
                    if (File.Exists(CONCEPT_PHASE_CALL_LINK))
                    {
                        // run new process and wait for finishing
                        string excute = CONCEPT_PHASE_CALL_LINK + " " + query + " ";
                        var processInfo = new ProcessStartInfo(PYTHON_LINK, excute)
                        {
                            CreateNoWindow = true,
                            UseShellExecute = false
                        };
                        Process proc;
                        if ((proc = Process.Start(processInfo)) == null)
                        {
                            throw new InvalidOperationException("??");
                        }
                        proc.WaitForExit();
                        proc.Close();
                    }
                    else
                    {
                        MessageBox.Show("Can not find Concept Python coded file!");
                    }
                }
                else
                {
                    MessageBox.Show("Can not find Concept Python excutable file!");
                }
                // load result from new file
                if (File.Exists(CONCEPT_PHASE_FOR_QUERY_RES1))
                {
                    hasResult = true;
                    var res = File.ReadAllLines(CONCEPT_PHASE_FOR_QUERY_RES1);
                    if (res.Length > 0)
                    {
                        var subStrings = res[0].Split(',');
                        for (int i = 0; i < subStrings.Length; i++)
                        {
                            for (int j = 0; j < listConcept.Length; j++)
                            {
                                if (subStrings[i] == listConcept[j])
                                {
                                    string link_To_File = INVERTED_LINK + j.ToString() + ".txt";
                                    if (File.Exists(link_To_File))
                                    {
                                        hasResult = true;
                                        var lines = File.ReadAllLines(link_To_File);
                                        foreach (var line in lines)
                                        {
                                            int index = Convert.ToInt32(line.Split(' ')[0]);
                                            double score = Convert.ToDouble(line.Split(' ')[1]);
                                            conceptResult[index] += score;
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Can not find inverted index for concept search result!");
                                    }
                                }
                            }
                        }

                        // normallize
                        double min = conceptResult.Min();
                        double range = conceptResult.Max() - min;
                        if (range != 0)
                        {
                            for (int i = 0; i < NUM_SHOT; i++)
                            {
                                conceptResult[i] = (conceptResult[i] - min) / range;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show(query + " not in concept bank!");
                    }
                }
            }
        }
        private double[] NonePhraseResult(string query)
        {

            double[] result = new double[NUM_SHOT];
            Array.Clear(result, 0, NUM_SHOT);
            Array.Clear(embeddingResult, 0, NUM_SHOT);
            Array.Clear(conceptResult, 0, NUM_SHOT);

            //TODO: Optimize without using concept search
            //UpdateResultConceptPhaseSearch(query);
            UpdateITVResultEmbedding(query);
            //UpdateITVResultConcept(query);
            for (int i = 0; i < NUM_SHOT; i++)
            {
                result[i] =  embeddingResult[i];
            }
            // normallize
            double min = result.Min();
            double range = result.Max() - min;
            if (range != 0)
            {
                for (int i = 0; i < NUM_SHOT; i++)
                {
                    result[i] = (result[i] - min) / range;
                }
            }
            return result;
        }

        private double[] AndPhrasesResult(List<string> queries)
        {
            double[] result = new double[NUM_SHOT];
            Array.Clear(result, 0, NUM_SHOT);

            foreach (string query in queries)
            {
                double[] score = new double[NUM_SHOT];
                Array.Clear(score, 0, NUM_SHOT);
                Array.Clear(embeddingResult, 0, NUM_SHOT);
                Array.Clear(conceptResult, 0, NUM_SHOT);

                //UpdateResultConceptPhaseSearch(query);
                UpdateITVResultEmbedding(query);
                //UpdateITVResultConcept(query);
                for (int i = 0; i < NUM_SHOT; i++)
                {
                    score[i] = embeddingResult[i];
                    result[i] += 1 + score[i];
                }
               
            }
            // normallize
            double min = result.Min();
            double range = result.Max() - min;
            if (range != 0)
            {
                for (int i = 0; i < NUM_SHOT; i++)
                {
                    result[i] = (result[i] - min) / range;
                }
            }
            return result;
        }

        private double[] OrPhrasesResult(List<string> queries)
        {
            double[] result = new double[NUM_SHOT];
            Array.Clear(result, 0, NUM_SHOT);


            foreach (string query in queries)
            {
                double[] score = new double[NUM_SHOT];
                Array.Clear(score, 0, NUM_SHOT);
                Array.Clear(embeddingResult, 0, NUM_SHOT);
                Array.Clear(conceptResult, 0, NUM_SHOT);

                //UpdateResultConceptPhaseSearch(query);
                UpdateITVResultEmbedding(query);
                //UpdateITVResultConcept(query);
                for (int i = 0; i < NUM_SHOT; i++)
                {
                    score[i] = embeddingResult[i];
                    result[i] = Math.Max(result[i], score[i]);
                }

            }
            // normallize
            double min = result.Min();
            double range = result.Max() - min;
            if (range != 0)
            {
                for (int i = 0; i < NUM_SHOT; i++)
                {
                    result[i] = (result[i] - min) / range;
                }
            }
            return result;
        }

        private void FilterResult(string query)
        {
            double[] score = new double[NUM_SHOT];
            Array.Clear(score, 0, NUM_SHOT);
            Array.Clear(embeddingResult, 0, NUM_SHOT);
            Array.Clear(conceptResult, 0, NUM_SHOT);

            //UpdateResultConceptPhaseSearch(query);
            UpdateITVResultEmbedding(query);
            //UpdateITVResultConcept(query);
            for (int i = 0; i < NUM_SHOT; i++)
            {
                score[i] = embeddingResult[i];
                if (score[i] > 0)
                {
                    finalResult[i] = 0;
                }
            }
            // normallize
            double min = finalResult.Min();
            double range = finalResult.Max() - min;
            if (range != 0)
            {
                for (int i = 0; i < NUM_SHOT; i++)
                {
                    finalResult[i] = (finalResult[i] - min) / range;
                }
            }
        }

        public void Search(List<string> query, List<LOGIC> logic)
        {
            bool hasOR = logic.Contains(LOGIC.OR);
            bool hasAND = logic.Contains(LOGIC.AND);

            qrs.Clear();
            qrs.Add("");
            ids.Clear();
            ids.Add("0");

            for (int i = 0; i < query.Count; i++)
            {
                if (logic[i] != LOGIC.NOT)
                {
                    GetQueries(query[i], logic[i]);
                }
            }

            // do AND first, OR later
            if (hasOR && hasAND)
            {
                int idx = 0;
                List<string> id_dist = ids.Distinct().ToList();
                for (int i = 0; i < id_dist.Count; i++)
                {
                    List<string> subqrs = new List<string>();
                    for (int j = 0; j < ids.Count; j++)
                    {
                        if (id_dist[i] == ids[j])
                        {
                            subqrs.Add(qrs[j]);
                        }
                    }
                    partialResult[idx] = AndPhrasesResult(subqrs);
                    idx++;
                }
                Array.Copy(partialResult[0], finalResult, NUM_SHOT);
                for (int i = 1; i < idx; i++)
                {
                    for (int j = 0; j < NUM_SHOT; j++)
                    {
                        finalResult[j] = Math.Max(finalResult[j], partialResult[i][j]);
                    }
                }
                // normallize
                double min = finalResult.Min();
                double range = finalResult.Max() - min;
                if (range != 0)
                {
                    for (int i = 0; i < NUM_SHOT; i++)
                    {
                        finalResult[i] = (finalResult[i] - min) / range;
                    }
                }
            }
            // get Max score among all OR
            if (hasOR && !hasAND)
            {
                finalResult = OrPhrasesResult(qrs);
            }
            // get common part of all AND
            if (!hasOR && hasAND)
            {
                finalResult = AndPhrasesResult(qrs);
            }
            if (!hasOR && !hasAND)
            {
                finalResult = NonePhraseResult(qrs[0]);
            }

            bool hasNOT = logic.Contains(LOGIC.NOT);
            // filtering
            if (hasNOT)
            {
                FilterResult(query[query.Count - 1]);
            }
            hasResult = true;
        }

        public bool HasResult()
        {
            return hasResult;
        }
    }
}
