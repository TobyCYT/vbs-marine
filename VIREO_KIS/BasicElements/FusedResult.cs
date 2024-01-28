using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VIREO_KIS.Properties;

namespace VIREO_KIS
{
    public class FusedResult
    {
        int NUM_SHOT = Settings.Default.DATASET_NUM_SHOTS;

        double[] final;
        double[] embeddingScore;
        double[] metaScore;
        double[] colorScore;

        bool hasMetaScore;
        bool hasColorScore;
        bool hasEmbeddingScore;        

        public void Clear()
        {
            Array.Clear(embeddingScore, 0, NUM_SHOT);
            Array.Clear(metaScore, 0, NUM_SHOT);
            Array.Clear(colorScore, 0, NUM_SHOT);
            Array.Clear(final, 0, NUM_SHOT);
            hasMetaScore = false;
            hasColorScore = false;
            hasEmbeddingScore = false;
        }

        public FusedResult()
        {
            final = new double[NUM_SHOT];
            embeddingScore = new double[NUM_SHOT];
            metaScore = new double[NUM_SHOT];
            colorScore = new double[NUM_SHOT];
            hasMetaScore = false;
            hasColorScore = false;
            hasEmbeddingScore = false;
        }

        public void Update(double[] res, RESULT_UPDATE ru, bool hasResult)
        {
            switch (ru)
            {
                case RESULT_UPDATE.FROM_COLOR:
                    if (hasResult)
                    {
                        Array.Copy(res, colorScore, NUM_SHOT);
                        hasColorScore = true;
                    }
                    else
                    {                        
                        Array.Clear(colorScore, 0, NUM_SHOT);
                        hasColorScore = false;
                    }
                    break;
                case RESULT_UPDATE.FROM_EMBEDDING:
                    if (hasResult)
                    {
                        Array.Copy(res, embeddingScore, NUM_SHOT);
                        hasEmbeddingScore = true;
                    }
                    else
                    {
                        Array.Clear(embeddingScore, 0, NUM_SHOT);
                        hasEmbeddingScore = false;
                    }
                    break;
                case RESULT_UPDATE.FROM_METADATA:
                    if (hasResult)
                    {
                        Array.Copy(res, metaScore, NUM_SHOT);
                        hasMetaScore = true;
                    }
                    else
                    {
                        Array.Clear(metaScore, 0, NUM_SHOT);
                        hasMetaScore = false;
                    }
                    break;
            }
        }

        public bool HasResult()
        {
            return hasMetaScore || hasEmbeddingScore || hasColorScore;
        }

        public double[] CalculateFinalScore(double w_color = 1, double w_meta = 1, double w_concept = 1)
        {
            Array.Clear(final, 0, NUM_SHOT);

            if (hasEmbeddingScore)
            {
                Parallel.For(0, NUM_SHOT, (i) =>
                {
                    final[i] += embeddingScore[i];
                });
            }
            if (hasMetaScore)
            {
                Parallel.For(0, NUM_SHOT, (i) =>
                {
                    final[i] += metaScore[i];
                });
            }
            if (hasColorScore)
            {
                Parallel.For(0, NUM_SHOT, (i) =>
                {
                    final[i] += colorScore[i];
                });
            }

            // normallize
            double min = final.Min();
            double range = final.Max() - min;
            if (range != 0)
            {
                for (int i = 0; i < NUM_SHOT; i++)
                {
                    final[i] = (final[i] - min) / range;
                }
            }
            return final;
        }
    }
}