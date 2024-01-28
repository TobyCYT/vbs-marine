using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VIREO_KIS
{
    public class MasterShot:IComparable
    {
        public int VideoID;
        public int ShotID;
        //public string TimeCode;

        public double FinalScore;

        public MasterShot()
        {
            VideoID = 0;
            ShotID = 0;
            FinalScore = 0;
            //TimeCode = "";
        }

        public MasterShot(int _VideoID, int _ShotID, double _Score)
        {
            VideoID = _VideoID;
            ShotID = _ShotID;
            FinalScore = _Score;
        }

        //public MasterShot(int _VideoID, string _TimeCode, double _Score)
        //{
        //    VideoID = _VideoID;
        //    TimeCode = _TimeCode;
        //    FinalScore = _Score;
        //}

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            MasterShot otherShot = obj as MasterShot;
            if (otherShot != null)
                return this.FinalScore.CompareTo(otherShot.FinalScore);
            else
                throw new ArgumentException("Object is not a MasterShot");
        }
    }

    public class MasterShotInfo
    {
        public double start_in_second;
        public double end_in_second;
        public int start_frame;
        public int end_frame;

        public MasterShotInfo(double _st_time_sec, double _ed_time_sec, int _st_frame, int _ed_frame)
        {
            start_in_second = _st_time_sec;
            end_in_second = _ed_time_sec;
            start_frame = _st_frame;
            end_frame = _ed_frame;
        }
    }
}
