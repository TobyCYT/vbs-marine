using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using VIREO_KIS.Properties;

namespace VIREO_KIS
{
    class TemporalResult
    {
        int NUM_SHOT = Settings.Default.DATASET_NUM_SHOTS;
        int NUM_VIDEOS = Settings.Default.DATASET_NUM_VIDEOS;
        int NUM_RES = Settings.Default.DISPLAY_NUM_RESULTS_TO_SHOW;
        string LNK_SHOT = Settings.Default.DATASET_LINK_SHOT_COUNT;
        string MARINE_MAP = Settings.Default.MARINE_MAPPING_FILE;

        List<MasterShot> final;
        List<List<bool>> nfilter;
        List<bool> isChecked;

        bool[] usedInAVS;
        int[] begPOS;
        int[] endPOS;

        //// added by zhixin
        //double[] fwd_score_t1;
        //double[] bkd_score_t2;
        //// add ends

        bool isFiltering = true;
                
        public void AddFilter(List<int> _toAdd, int _pos)
        {
            nfilter.Add(new List<bool>());
            var toInit = File.ReadAllLines(LNK_SHOT);
            for (int i = 0; i < toInit.Length; i++)
            {
                int VidID = Convert.ToInt32(toInit[i].Split(' ')[0]);
                int Shot = Convert.ToInt32(toInit[i].Split(' ')[1]);
                for (int j = 0; j < Shot; j++)
                {
                    nfilter[_pos].Add(false);
                }
            }
            for (int i = 0; i < _toAdd.Count; i++)
            {
                nfilter[_pos][_toAdd[i]] = true;
            }
            isChecked.Add(false);
        }

        public void SetFilter(int _id, bool _isSet)
        {
            isChecked[_id] = _isSet;
        }

        public bool SetFilterMode()
        {
            isFiltering = !isFiltering;
            return isFiltering;
        }

        public bool GetFilterMode()
        {
            return isFiltering;
        }

        public void Clear()
        {
            Array.Clear(usedInAVS, 0, NUM_VIDEOS+1);
        }

        public TemporalResult()
        {
            usedInAVS = new bool[NUM_VIDEOS+1];
            begPOS = new int[NUM_VIDEOS];
            endPOS = new int[NUM_VIDEOS];

            final = new List<MasterShot>();
            nfilter = new List<List<bool>>();
            isChecked = new List<bool>();

            int end = -1;

            var toInit = File.ReadAllLines(LNK_SHOT);

            for (int i = 0; i < toInit.Length; i++)
            {
                int VidID = Convert.ToInt32(toInit[i].Split(' ')[0]);
                int Shot = Convert.ToInt32(toInit[i].Split(' ')[1]);
                for (int j = 0; j < Shot; j++)
                {
                    final.Add(new MasterShot(VidID, j + 1, 0));
                }
                begPOS[i] = end + 1;
                endPOS[i] = end + Shot;
                end = end + Shot;
            }
            Console.WriteLine("TemporalResult: " + final.Count);
            //// added by zhixin
            //fwd_score_t1 = new double[end + 1];
            //bkd_score_t2 = new double[end + 1];
            //// add ends
        }

        public List<MasterShot> GetList(bool isAVSranking)
        {
            if (isAVSranking)
            {
                Array.Clear(usedInAVS, 0, NUM_VIDEOS+1);
                List<MasterShot> backup = final.OrderByDescending(o => o.FinalScore).ToList();
                List<MasterShot> ms = new List<MasterShot>();
                //List<MasterShot> store = new List<MasterShot>();
                int count = 0;
                int i = 0;
                // keep adding video segments having final scores > 0
                while ((count < NUM_RES))// && (backup[i].FinalScore > 0))
                {
                    //if (usedInAVS[backup[i].VideoID])
                    //{
                    //    store.Add(backup[i]);
                    //}
                    //else
                    if (!usedInAVS[backup[i].VideoID])
                    {
                        usedInAVS[backup[i].VideoID] = true;
                        ms.Add(backup[i]);
                        count++;
                    }
                    i++;
                }
                //// if the list is not full, go for the one that is positive 
                //int j = 0;
                //while ((count < NUM_RES) && (j < store.Count))
                //{
                //    ms.Add(store[j]);
                //    j++;
                //    count++;
                //}
                //// if the list is still not full, go for everything
                //while (count < NUM_RES)
                //{
                //    ms.Add(backup[i]);
                //    i++;
                //    count++;
                //}

                return ms;
            }
            else
            {
                List<MasterShot> backup = final.OrderByDescending(o => o.FinalScore).ToList();
                List<MasterShot> ms = backup.Take(NUM_RES).ToList();
                return ms;
            }
        }


        public void CalculateSingleScore(double[] score_t1)
        {
            Parallel.For(0, NUM_SHOT, (i) =>
            {
                // check whether the item is filtered
                bool check = false;
                for (int j = 0; j < isChecked.Count; j++)
                {
                    if ((isChecked[j]) && (nfilter[j][i]))
                    {
                        check = true;
                        break;
                    }
                }
                if (isFiltering)
                {
                    if (check)
                    {
                        final[i].FinalScore = -1;
                    }
                    else
                    {
                        //get the score here
                        final[i].FinalScore = score_t1[i];
                    }
                }
                else
                {
                    if (check)
                    {
                        //get the score here
                        final[i].FinalScore = score_t1[i];
                    }
                    else
                    {
                        final[i].FinalScore = -1;
                    }
                }
            });
        }

        public void CalculateTemporalScore(double[] score_t1, double[] score_t2)
        {
            for (int i = 0; i < NUM_VIDEOS; i++)
            {
                for (int j = begPOS[i] + 1; j < endPOS[i]; j++)
                {
                    score_t1[j] = Math.Max(score_t1[j], score_t1[j-1]);
                }

                //// original
                //for (int j = endPOS[i] - 2; j >= begPOS[i]; j--)
                //{
                //    score_t2[j] = Math.Max(score_t2[j] + 1, score_t2[j + 2]);
                //}

                // modified by zhixin
                for (int j = endPOS[i] - 1; j >= begPOS[i]; j--)
                {
                    score_t2[j] = Math.Max(score_t2[j], score_t2[j + 1]);
                }
                // modification ends

                score_t2[endPOS[i] - 1] = score_t2[endPOS[i]];
                score_t2[endPOS[i]] = 0;
            }

            Parallel.For(0, NUM_SHOT, (i) =>
            {
                // check whether the item is filtered
                bool check = false;
                for (int j = 0; j < isChecked.Count; j++)
                {
                    if ((isChecked[j]) && (nfilter[j][i]))
                    {
                        check = true;
                        break;
                    }
                }

                if (isFiltering)
                {
                    if (check)
                    {
                        final[i].FinalScore = -1;
                    }
                    else
                    {
                        // original
                        final[i].FinalScore = score_t1[i] + score_t2[i];
                    }
                }
                else
                {
                    if (check)
                    {
                        // original
                        final[i].FinalScore = score_t1[i] + score_t2[i];
                    }
                    else
                    {
                        final[i].FinalScore = -1;
                    }
                }
            });
        }
    }
}
