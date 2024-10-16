using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Command
{
    public class HttpHelper
    {
        public CookieContainer cookie = new CookieContainer();
        /// <summary>
        /// post请求返回html
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="postDataStr"></param>
        /// <returns></returns>
        public string HttpPost(string Url, string postDataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            //request.AllowAutoRedirect = false; //禁止自动重定向
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            //request.ContentLength = Encoding.UTF8.GetByteCount(postDataStr);
            request.CookieContainer = cookie; //cookie信息由CookieContainer自行维护
            //Stream myRequestStream = request.GetRequestStream();
            //StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("gb2312"));
            //myStreamWriter.Write(postDataStr);
            //myStreamWriter.Close();
            HttpWebResponse response = null;
            try
            {
                this.SetCertificatePolicy();
                response = (HttpWebResponse)request.GetResponse();
            }
#pragma warning disable CS0168 // 声明了变量“ex”，但从未使用过
            catch (System.Exception ex)
#pragma warning restore CS0168 // 声明了变量“ex”，但从未使用过
            {
            }
            //获取重定向地址
            //string url1 = response.Headers["Location"];
            if (response != null)
            {
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();
                return retString;
            }
            else
            {
                return "error"; //post请求返回为空
            }
        }

        public string HttpPost(string Url, string postDataStr, string contentType)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            //request.AllowAutoRedirect = false; //禁止自动重定向
            request.Method = "POST";
            request.ContentType = contentType;
            request.ContentLength = Encoding.UTF8.GetByteCount(postDataStr);
            request.CookieContainer = cookie; //cookie信息由CookieContainer自行维护
            Stream myRequestStream = request.GetRequestStream();
            StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("gb2312"));
            myStreamWriter.Write(postDataStr);
            myStreamWriter.Close();
            HttpWebResponse response = null;
            try
            {
                this.SetCertificatePolicy();
                response = (HttpWebResponse)request.GetResponse();
            }
#pragma warning disable CS0168 // 声明了变量“ex”，但从未使用过
            catch (System.Exception ex)
#pragma warning restore CS0168 // 声明了变量“ex”，但从未使用过
            {
            }
            //获取重定向地址
            //string url1 = response.Headers["Location"];
            if (response != null)
            {
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();
                return retString;
            }
            else
            {
                return "error"; //post请求返回为空
            }
        }
        /// <summary>
        /// get请求获取返回的html
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="postDataStr"></param>
        /// <returns></returns>
        public string HttpGet(string Url, string Querydata, out int StatusCode)
        {
            int Code = 0;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (Querydata == "" ? "" : "?") + Querydata);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            request.CookieContainer = cookie;
            this.SetCertificatePolicy();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Code = 200;
            }
            StatusCode = Code;
            // response.Cookies = cookie.GetCookies(response.ResponseUri);
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            return retString;
        }

        public string HttpGet(string Url, string Querydata)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (Querydata == "" ? "" : "?") + Querydata);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            request.CookieContainer = cookie;
            this.SetCertificatePolicy();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            // response.Cookies = cookie.GetCookies(response.ResponseUri);
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            return retString;
        }

        public async Task<string> HttpGetAsync(string Url, string Querydata)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (Querydata == "" ? "" : "?") + Querydata);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            request.CookieContainer = cookie;
            this.SetCertificatePolicy();
            HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            return retString;
        }
        /// <summary>
        /// 获得响应中的图像
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public Stream GetResponseImage(string url)
        {
            Stream resst = null;
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.KeepAlive = true;
                req.Method = "GET";
                req.AllowAutoRedirect = true;
                req.CookieContainer = cookie;
                req.ContentType = "application/x-www-form-urlencoded";
                req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                req.Timeout = 50000;
                Encoding myEncoding = Encoding.GetEncoding("UTF-8");
                this.SetCertificatePolicy();
                HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                resst = res.GetResponseStream();
                return resst;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 正则获取匹配的第一个值
        /// </summary>
        /// <param name="html"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public string getStringByRegex(string html, string pattern)
        {
            Regex re = new Regex(pattern, RegexOptions.IgnoreCase);
            MatchCollection matchs = re.Matches(html);
            if (matchs.Count > 0)
            {
                return matchs[0].Groups[1].Value;
            }
            else
                return "";
        }
        /// <summary>
        /// 正则验证返回的response是否正确
        /// </summary>
        /// <param name="html"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public bool verifyResponseHtml(string html, string pattern)
        {
            Regex re = new Regex(pattern);
            return re.IsMatch(html);
        }
        //注册证书验证回调事件，在请求之前注册
        private void SetCertificatePolicy()
        {
            ServicePointManager.ServerCertificateValidationCallback
                       += RemoteCertificateValidate;
        }
        /// <summary>  
        /// 远程证书验证，固定返回true 
        /// </summary>  
        private static bool RemoteCertificateValidate(object sender, X509Certificate cert,
            X509Chain chain, SslPolicyErrors error)
        {
            return true;
        }

        //解析系统枚举-是否停产
        public static string AnalysiContinueValue(string strValue)
        {
            if (string.IsNullOrEmpty(strValue))
                return string.Empty;
            string str = EnumParser.GetEnumFiledName(typeof(SysDicEnum.IsContinue), strValue);
            return str;
        }

        //解析系统枚举-字典类型
        public static string AnalysiDicValue(string strValue)
        {
            if (string.IsNullOrEmpty(strValue))
                return string.Empty;

            return EnumParser.GetEnumFiledName(typeof(SysDicEnum.DicClass), strValue);
        }

        //解析系统枚举-联系方式
        public static string AnalysiContactType(string strValue)
        {
            if (string.IsNullOrEmpty(strValue))
                return string.Empty;

            return EnumParser.GetEnumFiledName(typeof(SysDicEnum.ContactType), strValue);
        }

        //解析系统枚举-设备分类
        public static string AnalyasiClassify(string strValue)
        {
            if (string.IsNullOrEmpty(strValue))
                return string.Empty;

            return EnumParser.GetEnumFiledName(typeof(SysDicEnum.ClassType), strValue);
        }

        //解析系统枚举-有效状态
        public static string AnalyasiValidState(string strValue)
        {
            if (string.IsNullOrEmpty(strValue))
                return string.Empty;

            return EnumParser.GetEnumFiledName(typeof(SysDicEnum.IsValid), strValue);
        }

        //解析系统枚举-设备状态
        public static string AnalyasiEquipmentState(string strValue)
        {
            if (string.IsNullOrEmpty(strValue))
                return string.Empty;

            return EnumParser.GetEnumFiledName(typeof(SysDicEnum.EquipmentState), strValue);
        }

        //解析系统枚举-器件类型
        public static string DeviceType(string strValue)
        {
            if (string.IsNullOrEmpty(strValue))
                return string.Empty;

            return EnumParser.GetEnumFiledName(typeof(SysDicEnum.DevicetType), strValue);
        }

        public static string AnalyasiIsFrozen(string strValue)
        {
            if (string.IsNullOrEmpty(strValue))
                return string.Empty;

            return EnumParser.GetEnumFiledName(typeof(SysDicEnum.IsFrozen), strValue);
        }

        public static string AnalasiOrderSate(string strValue)
        {
            return EnumParser.GetEnumFiledName(typeof(SysDicEnum.OrderState), strValue);
        }
        public static string AnalasiSiteTypeName(string strValue)
        {
            return EnumParser.GetEnumFiledName(typeof(SysDicEnum.SiteType), strValue);
        }

        /// <summary>
        /// 清除Html标签样式
        /// </summary>
        /// <param name="Htmlstring"></param>
        /// <returns></returns>
        public static string RemoveHTML(string Htmlstring)
        {
            //删除脚本   
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML   
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", "   ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);

            Htmlstring.Replace("<", "");
            Htmlstring.Replace(">", "");
            Htmlstring.Replace("\r\n", "");
            //Htmlstring = HttpContext.Current.Server.HtmlEncode(Htmlstring).Trim();

            return Htmlstring;
        }
    }
}
