using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WNMS.Application.Utility.MediaServer
{
    public class ResponseModel<T>
    {
        public int code { get; set; }

        public string msg { get; set; }

        public List<T> data { get; set; }
    }

    public class MediaInfo
    {
        //应用名
        public string app { get; set; }
        //本协议观看人数
        public int readerCount { get; set; }
        //观看总人数，包括hls/rtsp/rtmp/http-flv/ws-flv
        public int totalReaderCount { get; set; }
        //协议
        public string schema { get; set; }
        //流 id
        public string stream { get; set; }
        //产生源类型
        public string originType { get; set; }

        public string originTypeStr { get; set; }
        //产生源的url
        public string originUrl { get; set; }
        //GMT unix系统时间戳，单位秒
        public long createStamp { get; set; }
        //存活时间，单位秒
        public int aliveSecond { get; set; }
        //数据产生速度，单位byte/s
        public int bytesSpeed { get; set; }
    }

    public class CloseReuslt
    {
        public int code { get; set; }
        //筛选命中的流个数
        public int count_hit { get; set; }
        //被关闭的流个数，可能小于count_hit
        public int count_closed { get; set; }
    }
}
