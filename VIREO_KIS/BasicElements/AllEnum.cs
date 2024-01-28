using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VIREO_KIS
{
    public enum LOGIC
    {
        AND,
        OR,
        NOT,
        NONE
    }
    public struct ColorCellQuery
    {
        public byte Pos;
        public byte R;
        public byte G;
        public byte B;
    }

    public enum SUBMIT_TYPE
    {
        SHOT_ID,
        FRAME_ID
    }

    public enum SUBMIT_FROM
    {
        RANKLIST_MASTERSHOT_VIEWER,
        VIDEO_MASTERSHOT_VIEWER,
        MASTERSHOT_NEAREST1000_VIEWER,
        VIDEO_PLAYER,
        DECOY
    }

    public enum RESULT_UPDATE
    {
        FROM_COLOR,
        FROM_METADATA,
        FROM_EMBEDDING,
        FROM_FILTER,
        FROM_NOTHING,
        FROM_RESET,
        FROM_START,
        FROM_CHANGE_RANKING_VIEW,
        FROM_CLEAR_PANEL,
        FROM_FUSION_WEIGHT
    }

    public enum MODEL_TYPE
    { 
        ITV=0,
        CLIP=1,
        FUSION=2
    }
    public enum SEARCH_TYPE
    {
        INDEX = 0,
        COSINE = 1
    }

}
