using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using System.Diagnostics;
using System.Timers;
using System.ComponentModel;
using Newtonsoft.Json;
using System.Threading;

using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;


namespace VIREO_KIS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public class LogATOM
    {
        public long timestamp;
        public string category;
        public string type;
        public string value;
    }

    public class LogFinal
    {        
        public string teamId;
        public int memberId;
        public long timestamp;
        public string type;
        public List<LogATOM> events;
    }

    public class LogItem
    {
        public string video;
        public string shot;
        public double score;
        public int rank;
    }

    public class LogRank
    {
        public string teamId;
        public int memberId;
        public long timestamp;        
        public List<string> usedCategories;
        public List<string> usedTypes;
        public List<string> sortType;
        public string resultSetAvailability = "all";
        public List<LogItem> results;
        public string type = "result";
    }

    public class RenderItem
    {
        public int ID;
        public double score;

        public RenderItem()
        {
            ID = 0;
            score = 0;
        }

        public RenderItem(int _ID, double _score)
        {
            ID = _ID;
            score = _score;
        }
    }

    public class SubmittedItem
    {
        public int vidID;
        public int shotID;

        public SubmittedItem()
        {
            vidID = 0;
            shotID = 0;
        }

        public SubmittedItem(int _vID, int _sID)
        {
            vidID = _vID;
            shotID = _sID;
        }

        public SubmittedItem(string link)
        {
            vidID = Convert.ToInt32(link.Split('-')[0]);
            shotID = Convert.ToInt32(link.Split('-')[1]);
        }

        public bool isEqual()
        {
            return true;
        }
    }

    public partial class MainWindow : Window
    {
        TemporalResult fResult;

        bool isAVSranking = false;

        int NUM_RES = Settings.Default.DISPLAY_NUM_RESULTS_TO_SHOW;
        bool SUBMISSION_ENABLE = Settings.Default.SUBMISSION_ENABLE;
        bool SEND_LOG = Settings.Default.SUBMISSION_SEND_LOG;
        string URL_BASE = Settings.Default.SUBMISSION_URL;
        string URL_LOGS = Settings.Default.SUBMISSION_URL_LOG;
        string LINK_LOG_INTERACTION = Settings.Default.LOGS_INTERACTION;
        string LINK_LOG_RANKLIST = Settings.Default.LOGS_RANKLIST;
        string TEAM_ID = Settings.Default.SUBMISSION_TEAM_ID;
        string MEMBER_ID = Settings.Default.SUBMISSION_MEMBER_ID;

        LogFinal logJSON = new LogFinal();

        List<MasterShot> ResKfrLst;
        
        List<SubmittedItem> SubmitedList = new List<SubmittedItem>();
        List<LogATOM> logList;

        int PosFrameView = 0;
        string linkToCurVideo = "";

        // DRES variables
        public string dres_session_id;
        public string dres_user_name;
        public string dres_user_role;
        public string dres_cur_task_id;
        public string dres_cur_task_name;
        public string dres_cur_evaluation_id;
        public UserApi user_api;  // UserApi is used to login
        public SubmissionApi submission_api;  // SubmissionApi is used to submit results
        public LogApi log_api;  // LogApi is used to submit log
        public EvaluationClientApi evaluation_client_api;  // EvaluationClientApi is used to get the evaluation list
        public Dictionary<string, MasterShotInfo> ms_info_dict;
        public Dictionary<string, double> video_fps_dict;
        public string cur_selected_video_id;
        public string cur_selected_shot_id;
        string DRES_USER_ID = Settings.Default.DRES_USER_ID;
        string DRES_PASSWORD = Settings.Default.DRES_PASSWORD;

        public delegate void InvokeDelegate();
        public MainWindow()
        {
            InitializeComponent();
            InitStaticVariables();
            InitializeOpenApiClient();


            src_Filter.UpdateParent(this);            
            src_Panel_T1.UpdateParent(this);
            src_Panel_T1.src_Concept.InitCurEvaluationRuns();
            src_Panel_T2.UpdateParent(this);
            src_Panel_T2.src_Concept.InitCurEvaluationRuns();
            src_Button.UpdateParent(this);
            vid_Player.UpdateParent(this);
            rnk_Viewer.UpdateParent(this);
            vdv_Viewer.UpdateParent(this);

            fResult = new TemporalResult();

            this.KeyDown += MainWindow_KeyDown;
            this.KeyUp += MainWindow_KeyUp;

            logList = new List<LogATOM>();
            ResKfrLst = new List<MasterShot>();     
            UpdateResultViewer(RESULT_UPDATE.FROM_NOTHING);

            src_Filter.AddAllFilters();
        }

        public void InitializeOpenApiClient()
        {
            Configuration config = new Configuration
            {
                BasePath = "https://vbs.videobrowsing.org"
            };

            string dres_user_id = DRES_USER_ID;
            string dres_password = DRES_PASSWORD;

            user_api = new UserApi(config);
            submission_api = new SubmissionApi(config);
            log_api = new LogApi(config);
            evaluation_client_api = new EvaluationClientApi(config);

            // Login request
            ApiUser api_user = null;
            try
            {
                api_user = user_api.PostApiV2Login(new LoginRequest(dres_user_id, dres_password));
            }
            catch (ApiException ex)
            {
                MessageBox.Show("Could not log in DRES due to exception: " + ex.Message);
            }

            // Login successful
            dres_session_id = api_user.SessionId.ToString();
            dres_user_name = api_user.Username.ToString();
            dres_user_role = api_user.Role.ToString();
        }


        private void InitStaticVariables()
        {

            // store index to id conversion in static variable
            string LNK_SHOT = Settings.Default.MARINE_SHOT_MAPPING_FILE;
            string MARINE_MAP = Settings.Default.MARINE_MAPPING_FILE;
            var toInit = File.ReadAllLines(MARINE_MAP);

            //toInit = File.ReadAllLines(MARINE_MAP);
            int video_counter = 1;
            for (int i = 0; i < toInit.Length; i++)
            {
                string[] tmp = toInit[i].Split(',');

                string _vidID = tmp[0].Split('+')[0];
                if (!Globals.vid2idx.ContainsKey(_vidID))
                {
                    Globals.vid2idx.Add(_vidID, video_counter);
                    Globals.idx2vid.Add(video_counter, _vidID);
                    video_counter++;
                }

                Globals.trec2marine.Add(tmp[1], tmp[0]);
                Globals.marine2trec.Add(tmp[0], tmp[1]);
            }

            // Load video fps info
            string link_video_fps = Settings.Default.DATASET_LINK_VIDEO_FPS;
            video_fps_dict = new Dictionary<string, double>();
            var video_fps_lines = File.ReadAllLines(link_video_fps);
            for (int i = 0; i < video_fps_lines.Length; i++)
            {
                string video_id = video_fps_lines[i].Split('\t')[0];
                double fps = Convert.ToDouble(video_fps_lines[i].Split('\t')[1]);
                video_fps_dict[video_id] = fps;
            }
        }

        ///
        /// Get list of items to pass into VideoViewer for key-frames rendering
        /// The original result list is just a list of candidate key-frames
        /// 
        List<RenderItem> GetFramesFromVideo()
        {
            linkToCurVideo = ResKfrLst[PosFrameView].VideoID.ToString() + "-" + ResKfrLst[PosFrameView].ShotID.ToString();
            List<RenderItem> candidateShot = new List<RenderItem>();
            for (int i = 0; i < ResKfrLst.Count; i++)
            {
                if (ResKfrLst[i].VideoID == ResKfrLst[PosFrameView].VideoID)
                {
                    // Pass in the score because some items in the ranking list may have score = 0 (due to take top k not by thresholding the scores)
                    RenderItem temp_Item = new RenderItem(ResKfrLst[PosFrameView].ShotID, ResKfrLst[PosFrameView].FinalScore);
                    candidateShot.Add(temp_Item);
                }
            }
            return candidateShot;
        }


        public void UpdateResultViewer(RESULT_UPDATE rs, string add_log = "", string noted = "")
        {
            bool hasChangeScore = true;
            long unixTimestamp = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;
            switch (rs)
            {
                case RESULT_UPDATE.FROM_COLOR:
                    LogATOM lg0 = new LogATOM();
                    lg0.category = "sketch";
                    lg0.type = "color";
                    lg0.value = add_log;
                    lg0.timestamp = unixTimestamp;
                    logList.Add(lg0);
                    break;
                case RESULT_UPDATE.FROM_CLEAR_PANEL:
                    LogATOM lg11 = new LogATOM();
                    lg11.category = "temporal";
                    lg11.type = "remove";
                    lg11.value = add_log;
                    lg11.timestamp = unixTimestamp;
                    logList.Add(lg11);
                    break;
                case RESULT_UPDATE.FROM_CHANGE_RANKING_VIEW:
                    LogATOM lg10 = new LogATOM();
                    lg10.category = "browsing";
                    lg10.type = "exploration";
                    lg10.value = add_log;
                    lg10.timestamp = unixTimestamp;
                    logList.Add(lg10);
                    hasChangeScore = false;
                    break;
                case RESULT_UPDATE.FROM_EMBEDDING:
                    LogATOM lg14 = new LogATOM();
                    lg14.category = "text";
                    lg14.type = "jointEmbedding";
                    lg14.value = add_log;
                    lg14.timestamp = unixTimestamp;
                    logList.Add(lg14);
                    break;
                case RESULT_UPDATE.FROM_METADATA:
                    LogATOM lg3 = new LogATOM();
                    lg3.category = "text";
                    lg3.type = noted;
                    lg3.value = add_log;
                    lg3.timestamp = unixTimestamp;
                    logList.Add(lg3);
                    break;
                case RESULT_UPDATE.FROM_RESET:
                    LogATOM lg4 = new LogATOM();
                    lg4.category = "browsing";
                    lg4.type = "resetAll";
                    lg4.value = add_log;
                    lg4.timestamp = unixTimestamp;
                    logList.Add(lg4);
                    break;
                case RESULT_UPDATE.FROM_FILTER:
                    LogATOM lg7 = new LogATOM();
                    lg7.category = "filter";
                    lg7.type = "color";
                    lg7.value = add_log;
                    lg7.timestamp = unixTimestamp;
                    logList.Add(lg7);
                    break;
                case RESULT_UPDATE.FROM_START:
                    LogATOM lg6 = new LogATOM();
                    lg6.category = "browsing";
                    lg6.type = "startSearch";
                    lg6.value = add_log;
                    lg6.timestamp = unixTimestamp;
                    logList.Add(lg6);
                    break;
            }

            if (hasChangeScore)
            {
                // calculate search result
                if (src_Panel_T1.HasResult() && src_Panel_T2.HasResult())
                {
                    fResult.CalculateTemporalScore(src_Panel_T1.GetResult(), src_Panel_T2.GetResult());
                }
                else
                {
                    if (src_Panel_T1.HasResult())
                    {
                        fResult.CalculateSingleScore(src_Panel_T1.GetResult());
                    }
                    else
                    {
                        fResult.CalculateSingleScore(src_Panel_T2.GetResult());
                    }
                }
            }

            // get result and display
            ResKfrLst = fResult.GetList(isAVSranking).ToList();

            if (rs != RESULT_UPDATE.FROM_NOTHING)
            {
                this.SendRanklist();
            }

            PosFrameView = 0;
            this.RefreshRanklistViewer();            
        }

        public void ViewNearestShots()
        {
            string shotID = vdv_Viewer.GetSelectedItem();
            ExtraViewWindow wd = new ExtraViewWindow();
            wd.UpdateParent(this);
            wd.LoadCandidates(shotID, SubmitedList);
            // Keep the similarity window always on the top until close - by zhixin
            wd.Topmost = false;
            wd.Owner = this;
            // The main window will not be disable if you use wd.Show() instead of wd.ShowDialog(). - by zhixin
            wd.Show();

            long unixTimestamp = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;
            LogATOM lg = new LogATOM();
            lg.category = "image";
            lg.type = "globalFeatures";
            lg.value = "open," + shotID.ToString();
            lg.timestamp = unixTimestamp;
            logList.Add(lg);
        }

        public void CloseExtraView()
        {
            long unixTimestamp = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;
            LogATOM lg = new LogATOM();
            lg.category = "image";
            lg.type = "globalFeatures";
            lg.value = "close";
            lg.timestamp = unixTimestamp;
            logList.Add(lg);

            rnk_Viewer.LoadCandidates(ResKfrLst, SubmitedList, false);
            rnk_Viewer.SetBorder(PosFrameView);

            if ((linkToCurVideo != null) && (linkToCurVideo != ""))
            {
                int curVID = Convert.ToInt32(linkToCurVideo.Split('-')[0]);
                int curSID = Convert.ToInt32(linkToCurVideo.Split('-')[1]);

                List<RenderItem> candidateShot = new List<RenderItem>();
                RenderItem temp_Item = new RenderItem(curSID, 1);
                candidateShot.Add(temp_Item);
                vdv_Viewer.LoadCandidates(candidateShot, SubmitedList, curVID, curSID);
            }
        }

        public void LogScrolling(string _type, string _value)
        {
            long unixTimestamp = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;
            LogATOM lg = new LogATOM();
            lg.category = "browsing";
            lg.type = _type;
            //string vid_id = _value.Split('-')[0].PadLeft(5, '0') + "_" + _value.Split('-')[1];
            //lg.value = Globals.trec2marine[vid_id].ToString();
            lg.value = "scroll to " + _value;
            lg.timestamp = unixTimestamp;
            logList.Add(lg);
        }

        public void RefreshRanklistViewer()
        {
            rnk_Viewer.LoadCandidates(ResKfrLst, SubmitedList);
            rnk_Viewer.SetBorder(PosFrameView);
            vdv_Viewer.LoadCandidates(GetFramesFromVideo(), SubmitedList, ResKfrLst[PosFrameView].VideoID, ResKfrLst[PosFrameView].ShotID);
        }

        private void MainWindow_KeyUp(object sender, KeyEventArgs e)
        {            
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
        }

        public void PlayVideo(string linkToVideo, bool needRefreshVideoViewer = true, bool needUpdateSelectedItemRankView = true)
        {
            if ((linkToVideo != null) && (linkToVideo != "None"))
            {
                vid_Player.PlayVideo(linkToVideo);
                long unixTimestamp = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;
                LogATOM lg = new LogATOM();
                lg.category = "browsing";
                lg.type = "videoPlayer";
                string vid_id = linkToVideo.Split('-')[0].PadLeft(5, '0') + "_" + linkToVideo.Split('-')[1];
                lg.value = Globals.trec2marine[vid_id].ToString();
                lg.timestamp = unixTimestamp;
                logList.Add(lg);

                int curVID = Convert.ToInt32(linkToVideo.Split('-')[0]);
                int curSID = Convert.ToInt32(linkToVideo.Split('-')[1]);
                if (needUpdateSelectedItemRankView)
                {
                    for (int i = 0; i < NUM_RES; i++)
                    {   
                        if ((curVID == ResKfrLst[i].VideoID) && (curSID == ResKfrLst[i].ShotID))
                        {
                            PosFrameView = i;
                            break;
                        }
                    }
                }
                if (needRefreshVideoViewer)
                {
                    vdv_Viewer.LoadCandidates(GetFramesFromVideo(), SubmitedList, curVID, curSID);
                }
            }
        }

        public void PlayVideoFrom1000Nearest(string linkToVideo, bool needRefreshVideoViewer)
        {
            int curVID = Convert.ToInt32(linkToVideo.Split('-')[0]);
            int curSID = Convert.ToInt32(linkToVideo.Split('-')[1]);
            
            if ((linkToVideo != null) && (linkToVideo != "None"))
            {
                long unixTimestamp = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;
                LogATOM lg = new LogATOM();
                lg.category = "browsing";
                lg.type = "videoPlayer";
                string vid_id = linkToVideo.Split('-')[0].PadLeft(5, '0') + "_" + linkToVideo.Split('-')[1];
                lg.value = Globals.trec2marine[vid_id].ToString();
                lg.timestamp = unixTimestamp;
                logList.Add(lg);

                vid_Player.PlayVideo(linkToVideo);
                List<RenderItem> candidateShot = new List<RenderItem>();
                RenderItem temp_Item = new RenderItem(curSID, 1);
                candidateShot.Add(temp_Item);
                vdv_Viewer.LoadCandidates(candidateShot, SubmitedList, curVID, curSID);
                linkToCurVideo = linkToVideo;
            }
        }

        public void SendRanklist()
        {
            long unixTimestamp = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;
            LogRank lgF = new LogRank();
            lgF.teamId = TEAM_ID.ToString();
            lgF.memberId = Convert.ToInt16(MEMBER_ID);
            //lgF.memberId = MEMBER_ID.ToString();
            lgF.timestamp = unixTimestamp;
            lgF.usedCategories = new List<string>();
            lgF.usedTypes = new List<string>();
            var t1 = src_Panel_T1.GetUsing();
            if (t1[0] != "")
            {
                lgF.usedCategories.Add("T1,Text");
                lgF.usedTypes.Add("T1," + t1[0]);
            }
            if (t1[1] != "")
            {
                lgF.usedCategories.Add("T1,Sketch");
                lgF.usedTypes.Add("T1," + t1[1]);
            }
            var t2 = src_Panel_T2.GetUsing();
            if (t2[0] != "")
            {
                lgF.usedCategories.Add("T2,Text");
                lgF.usedTypes.Add("T2," + t2[0]);
            }
            if (t2[1] != "")
            {
                lgF.usedCategories.Add("T2,Sketch");
                lgF.usedTypes.Add("T2," + t2[1]);
            }
            //if (src_Filter.IsUsed())
            //{
            //    lgF.usedCategories.Add("Filter");
            //    lgF.usedTypes.Add("Filter");
            //}
            if (isAVSranking)
            {
                lgF.usedCategories.Add("Ranking");
                lgF.usedTypes.Add("AVS");
            }
            else
            {
                lgF.usedCategories.Add("Ranking");
                lgF.usedTypes.Add("Normal");
            }
            lgF.sortType = new List<string>();
            lgF.sortType.Add("All");

            List<LogItem> logrank = new List<LogItem>();
            for (int i = 0; i < NUM_RES; i++)
            {
                LogItem itm = new LogItem();

                // winson's version 1
                //itm.video = ResKfrLst[i].VideoID.ToString();
                //itm.shot = ResKfrLst[i].ShotID;

                // winson's version 2
                //string video_shot_name = Globals.trec2marine[ResKfrLst[i].VideoID.ToString().PadLeft(5, '0') + "_" + ResKfrLst[i].ShotID].ToString();
                //itm.video = video_shot_name.Split('+')[0];
                //itm.shot = video_shot_name.Split('+')[1].Replace('-', ';');

                // new version
                itm.video = ResKfrLst[i].VideoID.ToString().PadLeft(5, '0');
                itm.shot = ResKfrLst[i].ShotID.ToString();

                itm.rank = i + 1;
                itm.score = ResKfrLst[i].FinalScore;
                logrank.Add(itm);
            }
            lgF.results = logrank;

            SendLogOpenApi(null, lgF);

            //string JSONfinal = JsonConvert.SerializeObject(lgF);
            //List<string> file_log = new List<string>();
            //file_log.Add(JSONfinal);
            //File.AppendAllLines(LINK_LOG_RANKLIST, file_log); //Nikki Add
            //if (SEND_LOG)
            //{
            //    try
            //    {
            //        string url = URL_LOGS + "?team=" + TEAM_ID + "&member=" + MEMBER_ID;
            //        var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            //        httpWebRequest.ContentType = "application/json";
            //        httpWebRequest.Method = "POST";
            //        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            //        {
            //            streamWriter.Write(JSONfinal);
            //            streamWriter.Flush();
            //            streamWriter.Close();
            //        }
            //        ThreadPool.QueueUserWorkItem(o =>
            //        {
            //            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            //            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            //            {
            //                var result = streamReader.ReadToEnd();

            //            }
            //        });
            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine(e.Message);
            //    }
            //}
        }

        public List<RankedAnswer> getOpenApiRankLog(LogRank log_rank)
        {
            List<RankedAnswer> res_rank_list = new List<RankedAnswer>();
            if (log_rank == null)
            {
                return res_rank_list;
            }

            for (int j = 0; j < log_rank.results.Count; j++)
            {
                string vid_id = log_rank.results[j].video;
                string shot_id = Convert.ToString(log_rank.results[j].shot);
                int _rank = log_rank.results[j].rank;
                long _time_in_sec = video_shot_to_time(vid_id, shot_id);

                res_rank_list.Add(new RankedAnswer(
                    answer: new ApiClientAnswer(
                        text: null,
                        mediaItemName: log_rank.results[j].video,
                        mediaItemCollectionName: null,
                        start: (_time_in_sec - 1) * 1000,
                        end: (_time_in_sec + 1) * 1000
                        ),
                    rank: null
                ));
            }

            return res_rank_list;
        }

        public List<QueryEvent> getOpenApiEventLog(List<LogATOM> log_events)
        {
            List<QueryEvent> res_event_list = new List<QueryEvent>();
            if (log_events == null)
            {
                return res_event_list;
            }

            for (int i = 0; i < log_events.Count; i++)
            {
                res_event_list.Add(new QueryEvent(
                    timestamp: log_events[i].timestamp,
                    category: new QueryEventCategory(),
                    type: log_events[i].type,
                    value: log_events[i].value
                ));
            }
            return res_event_list;
        }


        public void SendLogOpenApi(LogFinal log_final, LogRank log_rank)
        {
            long unixTimestamp = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;
            string resultSetAvailability; // not sure what it means

            // Convert Event log files to OpenAPI class
            List<QueryEvent> openapi_log_events = getOpenApiEventLog(logList);
            if (log_final != null)
            {
                resultSetAvailability = "False";
                // Write log to local file
                string JSONfinal = JsonConvert.SerializeObject(log_final);
                List<string> file_log = new List<string>();
                file_log.Add(JSONfinal);
                File.AppendAllLines(LINK_LOG_INTERACTION, file_log);
            }

            // Convert Rank log files to OpenAPI class
            List<RankedAnswer> openapi_log_ranks = getOpenApiRankLog(log_rank);
            if (log_rank != null)
            {
                resultSetAvailability = "True";
                // write local log
                string JSONfinal = JsonConvert.SerializeObject(log_rank);
                List<string> file_log = new List<string>();
                file_log.Add(JSONfinal);
                File.AppendAllLines(LINK_LOG_RANKLIST, file_log);
            }

            // Sent by OpenApi
            ThreadPool.QueueUserWorkItem(o =>
            {
                try
                {
                    log_api.PostApiV2LogResultByEvaluationId(
                        session: dres_session_id,
                        evaluationId: dres_cur_evaluation_id,
                        body: new QueryResultLog(
                            timestamp: unixTimestamp,
                            sortType: "All",
                            resultSetAvailability: "",
                            results: openapi_log_ranks,
                            events: openapi_log_events
                            )
                        );
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            });

            // Clear 
            logList.Clear();
        }

        public (string, string) video_shot_sys_to_full_marine(string vid_id_sys, string shot_id_sys)
        {
            string video_shot = Globals.trec2marine[vid_id_sys.PadLeft(5, '0') + "_" + shot_id_sys].ToString();

            string vid_shot_name_full = video_shot.Split('+')[0];
            string vid_time_code = video_shot.Split('+')[1].Replace("-", ":");
            return (vid_shot_name_full, vid_time_code);

        }

        public long video_shot_to_time(string vid_id, string shot_id)
        {
            Dictionary<string, long> time_dict = new Dictionary<string, long>();

            (string vid_id_full, string vid_time_code) = video_shot_sys_to_full_marine(vid_id, shot_id);

            string[] time_code = vid_time_code.Split(':');
            long _mid_time_in_sec =
                Convert.ToInt64(time_code[0]) * 3600 +
                Convert.ToInt64(time_code[1]) * 60 +
                Convert.ToInt64(time_code[2]);

            return _mid_time_in_sec;
        }

        public void SubmitResult(string linkToVideo, SUBMIT_TYPE smt, SUBMIT_FROM frm, int frame_num = 0)
        {

            if (linkToVideo == "")
            {
                return;
            }

            //if (logList.Count > 0)
            //{
            //    long unixTimestamp = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;
            //    LogFinal lgF = new LogFinal();
            //    lgF.teamId = TEAM_ID.ToString();
            //    lgF.memberId = Convert.ToInt16(MEMBER_ID);
            //    lgF.type = "interaction";
            //    lgF.timestamp = unixTimestamp;
            //    lgF.events = logList;

            //    string JSONfinal = JsonConvert.SerializeObject(lgF);
            //    List<string> file_log = new List<string>();
            //    file_log.Add(JSONfinal);
            //    File.AppendAllLines(LINK_LOG_INTERACTION, file_log);
            //    logList.Clear();

            //    if (SEND_LOG)
            //    {
            //        try
            //        {
            //            //---for ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            //            //ServicePointManager.Expect100Continue = true; //Nikki add
            //            //ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072; //Nikki add
            //            //---
            //            string url = URL_LOGS + "?team=" + TEAM_ID + "&member=" + MEMBER_ID;
            //            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            //            httpWebRequest.ContentType = "application/json";
            //            httpWebRequest.Method = "GET";
            //            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            //            {
            //                streamWriter.Write(JSONfinal);
            //                streamWriter.Flush();
            //                streamWriter.Close();
            //            }
            //            ThreadPool.QueueUserWorkItem(o =>
            //            {
            //                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            //                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            //                {
            //                    var result = streamReader.ReadToEnd();
            //                //MessageBox.Show(result);
            //            }
            //            });
            //        }
            //        catch (Exception e)
            //        {
            //        }
            //    }
            //}

            // Record the current submission interaction
            long unixTimestamp = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;
            LogATOM lg = new LogATOM();
            lg.category = "submit";
            if (smt == SUBMIT_TYPE.SHOT_ID)
            {
                lg.type = "shot";
            }
            else if (smt == SUBMIT_TYPE.FRAME_ID)
            {
                lg.type = "frame";
            }
            lg.value = linkToVideo;
            lg.timestamp = unixTimestamp;
            logList.Add(lg);

            // Record the current submission interaction
            LogFinal lgF = new LogFinal();
            lgF.teamId = TEAM_ID.ToString();
            lgF.memberId = Convert.ToInt16(MEMBER_ID);
            lgF.type = "interaction";
            lgF.timestamp = unixTimestamp;
            lgF.events = logList; 

            // Sent event log
            SendLogOpenApi(lgF, null);

            // Submit result to DRES
            SubmitedList.Add(new SubmittedItem(linkToVideo));

            if (frm == SUBMIT_FROM.RANKLIST_MASTERSHOT_VIEWER)
            {
                vdv_Viewer.UpdateSubmitted(new SubmittedItem(linkToVideo));
            }
            else if (frm == SUBMIT_FROM.VIDEO_MASTERSHOT_VIEWER)
            {
                rnk_Viewer.UpdateSubmitted(new SubmittedItem(linkToVideo));
            }

            string vidID = linkToVideo.Split('-')[0];
            string shotID = linkToVideo.Split('-')[1];
            (string vid_id_full, string vid_time_code) = video_shot_sys_to_full_marine(vidID, shotID);
            long _mid_time_in_sec = video_shot_to_time(vidID, shotID);

            List<ApiClientAnswerSet> answerSets = null;
            if (smt == SUBMIT_TYPE.SHOT_ID)
            {
                //MasterShotInfo msi = ms_info_dict[$"{vid_full}_{shot_id_full}"];

                answerSets = new List<ApiClientAnswerSet>()
                {
                    new ApiClientAnswerSet(
                    answers: new List<ApiClientAnswer>
                    {
                        new ApiClientAnswer(
                            mediaItemName: vid_id_full,
                            start: Convert.ToInt64((_mid_time_in_sec - 1) * 1000),
                            end: Convert.ToInt64((_mid_time_in_sec + 1) * 1000)
                )})};

            }
            else if (smt == SUBMIT_TYPE.FRAME_ID)
            {
                int frame_no = frame_num;
                double fps = video_fps_dict[vid_id_full];
                if (fps > 24) { fps = 24; }  // the used video is transcoded and the fps is reduced to 24
                long _frame_time_in_ms = Convert.ToInt64((frame_no / fps) * 1000);

                answerSets = new List<ApiClientAnswerSet>()
                {
                    new ApiClientAnswerSet(
                        answers: new List<ApiClientAnswer>
                        {
                            new ApiClientAnswer(
                                mediaItemName: vid_id_full,
                                start: _frame_time_in_ms,
                                end: _frame_time_in_ms
                )})};
            }

            ThreadPool.QueueUserWorkItem(o =>
            {
                try
                {
                    SuccessfulSubmissionsStatus submission_status =
                        submission_api.PostApiV2SubmitByEvaluationId(
                            new ApiClientSubmission(answerSets), dres_cur_evaluation_id, dres_session_id);
                    //Console.WriteLine(submission_status);
                }
                catch (ApiException x){MessageBox.Show(x.Message);}
            });

            //string fmt = "00000";
            //string vidID = linkToVideo.Split('-')[0];
            //string shotID = linkToVideo.Split('-')[1];
            //string vidID_pre = Convert.ToInt16(vidID).ToString(fmt);
            //long unixTimestamp = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;
                    
            //LogATOM lg = new LogATOM();
            //lg.category = "submit";
            //if (smt == SUBMIT_TYPE.SHOT_ID)
            //{
            //    lg.type = "shot";
            //}
            //else if (smt == SUBMIT_TYPE.FRAME_ID)
            //{
            //    lg.type = "frame";
            //}

            //string video_shot = Globals.trec2marine[vidID_pre.PadLeft(5, '0') + "_" + shotID].ToString();
            ////string url = URL_BASE + "?User=" + TEAM_ID + "&sessionId=" + MEMBER_ID + "&item=" + vidID;
            //string vid_shot_name = video_shot.Split('+')[0];
            //string vid_time_code = video_shot.Split('+')[1].Replace("-", ":");

            ////lg.value = linkToVideo;
            //lg.value = vid_shot_name + "+" + vid_time_code;
            //lg.timestamp = unixTimestamp;
            //logList.Add(lg);

            //LogFinal lgF = new LogFinal();
            //lgF.teamId = TEAM_ID.ToString();
            //lgF.memberId = Convert.ToInt16(MEMBER_ID);
            //lgF.type = "submission";
            //lgF.timestamp = unixTimestamp;
            //lgF.events = logList;

            //string JSONfinal = JsonConvert.SerializeObject(lgF);
            //List<string> file_log = new List<string>();
            //file_log.Add(JSONfinal);
            //File.AppendAllLines(LINK_LOG_INTERACTION, file_log);
            //logList.Clear();

            //try
            //{
            //    vid_time_code = vid_time_code.Split(':')[0] + ':' + vid_time_code.Split(':')[1] + ':' + vid_time_code.Split(':')[2] + ':' + "15";
            //    string url = URL_BASE + "?session=" + TEAM_ID + "&item=" + vid_shot_name + "&timecode=" + vid_time_code; //For VBS2021 DRES system
            //    //string url = URL_BASE + "?session=" + TEAM_ID + "&item=" + vidID_pre; //For VBS2021 DRES system
            //    Console.WriteLine(url);
            //    //string url = URL_BASE + "?item=" + vidID_pre; //For VBS2021 DRES system
            //    //if (smt == SUBMIT_TYPE.SHOT_ID)
            //    //{
            //    //    url = url + "&shot=" + shotID;
            //    //}
            //    //else if (smt == SUBMIT_TYPE.FRAME_ID)
            //    //{
            //    //    url = url + "&frame=" + shotID;
            //    //}

            //    var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            //    httpWebRequest.Method = "GET";
            //    httpWebRequest.ContentType = "application/x-www-form-urlencoded";

            //    ThreadPool.QueueUserWorkItem(o =>
            //    {
            //        try
            //        {
            //            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            //            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            //            {
            //                var result = streamReader.ReadToEnd();
            //            MessageBox.Show(result);
            //            }
            //        }
            //        catch (Exception x)
            //        {
            //            String xstr = x.Message;
            //            if (xstr.Contains("401")){ 
            //                    MessageBox.Show("There was an authentication error during submission. Check the session id.");
            //            }
            //            else if (xstr.Contains("404")){ 
            //                    MessageBox.Show("There is currently no active task which would accept submissions.");
            //            }
            //            else { 
            //                    MessageBox.Show(xstr);
            //            }
            //        }
            //    });
            //}
            //catch (Exception e)
            //{
            //}
        }

        public void Reset()
        {
            fResult.Clear();
            src_Filter.Clear();
            src_Panel_T1.Clear();
            src_Panel_T2.Clear();
            PosFrameView = 0;            
        }

        public void ClearSearch()
        {
            this.Reset();
            UpdateResultViewer(RESULT_UPDATE.FROM_RESET);
        }

        public void EndSearch()
        {
            // send remaining logs
            if (logList.Count > 0)
            {
                SubmitResult("", SUBMIT_TYPE.SHOT_ID, SUBMIT_FROM.DECOY);
            }
            SubmitedList.Clear();
            this.Reset();
            UpdateResultViewer(RESULT_UPDATE.FROM_NOTHING);
        }

        public void NewSearch()
        {
            Reset();            
            SubmitedList.Clear();
            logList.Clear();
            UpdateResultViewer(RESULT_UPDATE.FROM_START); 
        }

        public bool SwitchRanking()
        {
            isAVSranking = !isAVSranking;
            UpdateResultViewer(RESULT_UPDATE.FROM_CHANGE_RANKING_VIEW, isAVSranking ? "AVS ranking" : "Normal ranking");
            return isAVSranking;
        }

        public bool GetFilterMode()
        {
            return fResult.GetFilterMode();
        }

        public void AddFilter(List<int> lst, int pos)
        {
            fResult.AddFilter(lst, pos);
        }

        public void SetFilter(int id, bool isSet)
        {
            fResult.SetFilter(id, isSet);
        }

        public void SetSubmissionTask(string task_id)
        {
            dres_cur_evaluation_id = task_id;
        }


        public bool SetFilterMode()
        {
            return fResult.SetFilterMode();
        }

        private void tab_Ctrl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void src_Panel_T2_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}