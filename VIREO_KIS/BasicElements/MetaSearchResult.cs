using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VIREO_KIS.Properties;

namespace VIREO_KIS
{
    public class MetaSearchResult
    {
        static string META_RES = Settings.Default.META_RESULT_META;
        static string OCR_RES = Settings.Default.META_RESULT_OCR;
        static string ASR_RES = Settings.Default.META_RESULT_ASR;

        static int NUM_SHOT = Settings.Default.DATASET_NUM_SHOTS;
        static string LNK_SHOT = Settings.Default.DATASET_LINK_SHOT_COUNT;

        double[] Meta_Result = new double[NUM_SHOT];
        double[] ASR_Result = new double[NUM_SHOT];
        double[] OCR_Result = new double[NUM_SHOT];

        List<int> shotCount = new List<int>();
        bool hasMeta = false;
        bool hasOCR = false;
        bool hasASR = false;

        public bool isUsingMeta = false;
        public bool isUsingASR = false;
        public bool isUsingOCR = false;

        public void Init()
        {

        }

        public MetaSearchResult()
        {
            var toInit = File.ReadAllLines(LNK_SHOT);
            for (int i = 0; i < toInit.Length; i++)
            {
                int Shot = Convert.ToInt32(toInit[i].Split(' ')[1]);
                if (i == 0)
                {
                    shotCount.Add(Shot);
                }
                else
                {
                    shotCount.Add(Shot + shotCount[i - 1]);
                }
            }
        }

        public void Clear()
        {
            Array.Clear(Meta_Result, 0, NUM_SHOT);
            Array.Clear(ASR_Result, 0, NUM_SHOT);
            Array.Clear(OCR_Result, 0, NUM_SHOT);
            hasMeta = false;
            hasOCR = false;
            hasASR = false;
            isUsingMeta = false;
            isUsingOCR = false;
            isUsingASR = false;
        }

        public void UpdateResult()
        {
            Array.Clear(Meta_Result, 0, NUM_SHOT);
            Array.Clear(ASR_Result, 0, NUM_SHOT);
            Array.Clear(OCR_Result, 0, NUM_SHOT);

            if (isUsingMeta && File.Exists(META_RES))
            {
                var Meta_lines = File.ReadAllLines(META_RES);
                foreach (var l in Meta_lines)
                {
                    var val = l.Split(' ');
                    var shot_id = val[0];
                    shot_id = Globals.marine2trec[shot_id].ToString();
                    //int videoID = Convert.ToInt32(shot_id.Split('_')[0]) - 1;
                    //int shotID = Convert.ToInt32(shot_id.Split('_')[1]) - 1;
                    int videoID = Convert.ToInt32(shot_id.Split('_')[0]) - 1;
                    int shotID = Convert.ToInt32(shot_id.Split('_')[1]) - 1;
                    double score = Convert.ToDouble(val[1]);
                    if (videoID >= 1)
                    {
                        Meta_Result[shotCount[videoID - 1] + shotID] = score;
                    }
                    else
                    {
                        Meta_Result[shotID] = score;
                    }
                }
                hasMeta = true;
            }
            else
            {
                hasMeta = false;
            }

            //Console.WriteLine("OCR Result:", OCR_RES, File.Exists(OCR_RES));
            if (isUsingOCR && File.Exists(OCR_RES))
            {
                
                var OCR_lines = File.ReadAllLines(OCR_RES);
                foreach (var l in OCR_lines)
                {
                    var val = l.Split(' ');
                    var shot_id = val[0].Split('.')[0];
                    int videoID = Convert.ToInt32(shot_id.Split('_')[0]) - 1;
                    int shotID = Convert.ToInt32(shot_id.Split('_')[1]) - 1;
                    double score = Convert.ToDouble(val[1]);
                    if (videoID >= 1)
                    {
                        OCR_Result[shotCount[videoID - 1] + shotID] = score;
                    }
                    else
                    {
                        OCR_Result[shotID] = score;
                    }
                }
                hasOCR = true;
            }
            else
            {
                hasOCR = false;
            }

            if (isUsingASR && File.Exists(ASR_RES))
            {
                var ASR_lines = File.ReadAllLines(ASR_RES);
                foreach (var l in ASR_lines)
                {
                    var val = l.Split(' ');
                    var shot_id = val[0].Split('.')[0];
                    int videoID = Convert.ToInt32(shot_id.Split('_')[0]) - 1;
                    int shotID = Convert.ToInt32(shot_id.Split('_')[1]) - 1;
                    double score = Convert.ToDouble(val[1]);
                    if (videoID >= 1)
                    {
                        ASR_Result[shotCount[videoID - 1] + shotID] = score;
                    }
                    else
                    {
                        ASR_Result[shotID] = score;
                    }
                }
                hasASR = true;
            }
            else
            {
                hasASR = false;
            }
        }

        public double[] Get()
        {
            double[] Final_Result = new double[NUM_SHOT];
            int val_Meta = isUsingMeta ? 1 : 0;
            int val_ASR = isUsingASR ? 1 : 0;
            int val_OCR = isUsingOCR ? 1 : 0;

            Parallel.For(0, NUM_SHOT, (j) =>
            {
                Final_Result[j] = (ASR_Result[j] * val_ASR + OCR_Result[j] * val_OCR + Meta_Result[j] * val_Meta)/3.0 ;
            });

            return Final_Result;
        }

        public void SwitchMeta()
        {
            isUsingMeta = !isUsingMeta;
        }

        public void SwitchOCR()
        {
            isUsingOCR = !isUsingOCR;
            Console.WriteLine("OCR: " + isUsingOCR);
        }

        public void SwitchASR()
        {
            isUsingASR = !isUsingASR;
        }

        public bool HasResult()
        {
            return (hasMeta && isUsingMeta) || (hasASR && isUsingASR) || (hasOCR && isUsingOCR);
        }

        public bool HasMeta()
        {
            return (hasMeta && isUsingMeta);
        }

        public bool HasASR()
        {
            return (hasASR && isUsingASR);
        }

        public bool HasOCR()
        {
            return (hasOCR && isUsingOCR);
        }
    }
}
