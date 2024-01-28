using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using VIREO_KIS.Properties;
using System.Windows.Media;
using System.Windows;

namespace VIREO_KIS
{
    public class ColorSearchResult
    {
        static int NUM_SHOT = Settings.Default.DATASET_NUM_SHOTS;        
        static string COLOR_LINK = Settings.Default.COLOR_LINK;

        static int NUM_ROW = Settings.Default.COLOR_GRID_NUM_ROW;
        static int NUM_COL = Settings.Default.COLOR_GRID_NUM_COL;

        double[] finalAVG = new double[NUM_SHOT];
        double[] toReturnAVG = new double[NUM_SHOT];

        int num_query = 0;

        public ColorSearchResult()
        {
            Array.Clear(finalAVG, 0, NUM_SHOT);
            Array.Clear(toReturnAVG, 0, NUM_SHOT);
        }

        public void Init()
        {

        }

        public double[] Get(bool isMIN)
        {
            double max_avg = finalAVG.Max();


            if (max_avg > 0)
            {
                Parallel.For(0, NUM_SHOT, (j) =>
                {
                    toReturnAVG[j] = (1 - (finalAVG[j] / max_avg));
                });

                return toReturnAVG;
            }

            return finalAVG;
        }

        public void AddQuery(ColorCellQuery query)
        {
            int TempBufferSize = 8 * NUM_SHOT;
           
            string qr_file_avg = COLOR_LINK + query.B + "_" + query.G + "_" + query.R + "_" + query.Pos + ".txt";
            if (File.Exists(qr_file_avg))
            {
                BinaryReader br_avg = new BinaryReader(new FileStream(qr_file_avg, FileMode.Open));
                double[] arr_avg = new double[NUM_SHOT];
                byte[] tempBuffer_avg = br_avg.ReadBytes(TempBufferSize);
                Buffer.BlockCopy(tempBuffer_avg, 0, arr_avg, 0, TempBufferSize);
                br_avg.Close();

                for (int i = 0; i < NUM_SHOT; i++)
                {
                    finalAVG[i] += arr_avg[i];
                }
            }
            num_query++;
        } 

        public void RemoveQuery(ColorCellQuery query)
        {
            if (num_query > 1)
            {
                int TempBufferSize = 8 * NUM_SHOT;
                
                string qr_file_avg = COLOR_LINK + query.B + "_" + query.G + "_" + query.R + "_" + query.Pos + ".txt";
                if (File.Exists(qr_file_avg))
                {
                    BinaryReader br_avg = new BinaryReader(new FileStream(qr_file_avg, FileMode.Open));
                    double[] arr_avg = new double[NUM_SHOT];
                    byte[] tempBuffer_avg = br_avg.ReadBytes(TempBufferSize);
                    Buffer.BlockCopy(tempBuffer_avg, 0, arr_avg, 0, TempBufferSize);
                    br_avg.Close();

                    for (int i = 0; i < NUM_SHOT; i++)
                    {
                        finalAVG[i] -= arr_avg[i];
                    }
                }
                num_query--;
            }
            else
            {
                Clear();
            }
        }

        public bool HasResult()
        {
            return num_query > 0;
        }

        public void Clear()
        {
            Array.Clear(finalAVG, 0, NUM_SHOT);
            Array.Clear(toReturnAVG, 0, NUM_SHOT);
            num_query = 0;
        }       
    }
}
