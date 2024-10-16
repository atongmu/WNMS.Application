using System;
using System.Collections.Generic;
using System.Text;

namespace WNMS.Utility
{
    public class StaticConstraint
    {
        public static void Init(Func<string, string> func)
        {
            WNMSConnection = func.Invoke("ConnectionStrings:WNMSConnectionString");
            Service_Ip = func.Invoke("WinFormService");
            MongoDBConn = func.Invoke("MongoDBConn");
            MongoDBName = func.Invoke("MongoDBName");
            TimeValue = func.Invoke("TimeValue");
            CameraService = func.Invoke("CameraService");
            CameraLivePort = func.Invoke("CameraLivePort");
            CameraControlPort = func.Invoke("CameraControlPort");
            JpushSoftKey = func.Invoke("JpushSoftKey");
            JpushAppKey = func.Invoke("JpushAppKey");
            JpushMasterSecret = func.Invoke("JpushMasterSecret");
            SignalrHubs = func.Invoke("SignalrHubs");
            WebSpeech = func.Invoke("WebSpeech");
            ArcgisUrl = func.Invoke("ArcgisUrl");
            PipeUrl = func.Invoke("PipeUrl");
            Threed = func.Invoke("Threed");
            MeidaServerHttp = func.Invoke("MeidaServerHttp");

            //循环--反射的方式初始化多个
        }


        /// <summary>
        /// 以前直接读配置文件
        /// ConnectionStrings:JDDbConnectionString
        /// </summary>
        public static string WNMSConnection = null;

        public static string Service_Ip = null;
        public static string MongoDBConn = null;
        public static string MongoDBName = null;
        public static string TimeValue = null;
        public static string CameraService = null;
        public static string CameraLivePort = null;
        public static string CameraControlPort = null;
        public static string JpushSoftKey = null;
        public static string JpushAppKey = null;
        public static string JpushMasterSecret = null;
        public static string SignalrHubs = null;
        public static string WebSpeech = null;
        public static string ArcgisUrl = null;
        public static string PipeUrl = null;
        public static string Threed = null;
        public static string MeidaServerHttp = null;
    }
}
