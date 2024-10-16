using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WNMS.Application.Utility.JsonHelper
{
    public class JsonFileHelper
    {
        private string _path;//根目录的相对途径
        private IConfiguration Configuration { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsonName">根目录的相对途径包含文件名</param>
        public JsonFileHelper(string jsonName)
        {
            if (!jsonName.EndsWith(".json"))
            {
                jsonName = $"{jsonName}.json";
            }

            _path = jsonName;
            if (!File.Exists(_path))
            {
                File.WriteAllText(_path, "{}");
            }

            //ReloadOnChange = true 当*.json文件被修改时重新加载            
            Configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(_path)
            //上面SetBasePath 和AddJsonFile是web网页的，下面是控制台下的
            //.Add(new JsonConfigurationSource { Path = _path, ReloadOnChange = true, Optional = true })
            .Build();
        }

        /// <summary>
        /// 读取Json返回实体对象
        /// </summary>
        /// <returns></returns>
        public T Read<T>() => Read<T>("");

        /// <summary>
        /// 根据节点读取Json返回实体对象
        /// </summary>
        /// <param name="section">根节点</param>
        /// <returns></returns>
        public T Read<T>(string section)
        {
            try
            {
                using (StreamReader file = new StreamReader(_path))
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    JToken secJt = JToken.ReadFrom(reader);
                    if (!string.IsNullOrWhiteSpace(section))
                    {//jObj[section].SelectToken("address").SelectToken("name")
                        string[] nodes = section.Split(':');
                        foreach (string node in nodes)
                        {
                            secJt = secJt[node];
                        }
                        if (secJt != null)
                        {
                            return JsonConvert.DeserializeObject<T>(secJt.ToString());
                        }
                    }
                    else
                    {
                        return JsonConvert.DeserializeObject<T>(secJt.ToString());
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return default(T);
        }

        /// <summary>
        /// 读取Json返回集合
        /// </summary>
        /// <returns></returns>
        public List<T> ReadList<T>() => ReadList<T>("");

        /// <summary>
        /// 根据节点读取Json返回集合
        /// </summary>
        /// <param name="section">根节点</param>
        /// <returns></returns>
        public List<T> ReadList<T>(string section)
        {
            try
            {
                using (StreamReader file = new StreamReader(_path))
                {
                    using (JsonTextReader reader = new JsonTextReader(file))
                    {
                        JToken secJt = JToken.ReadFrom(reader);
                        if (!string.IsNullOrWhiteSpace(section))
                        {
                            string[] nodes = section.Split(':');
                            foreach (string node in nodes)
                            {
                                secJt = secJt[node];
                            }
                            if (secJt != null)
                            {
                                return JsonConvert.DeserializeObject<List<T>>(secJt.ToString());
                            }
                        }
                        else
                        {
                            return JsonConvert.DeserializeObject<List<T>>(section.ToString());
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return default(List<T>);
        }

        /// <summary>
        /// 写入文件
        /// </summary>
        /// <typeparam name="T">自定义对象</typeparam>
        /// <param name="t"></param>
        public void Write<T>(T t) => Write("", t);

        /// <summary>
        /// 写入指定section文件
        /// </summary>
        /// <typeparam name="T">自定义对象</typeparam>
        /// <param name="t"></param>
        public bool Write<T>(string section, T t)
        {
            var flag = true;
            try
            {
                JObject jObj;
                using (StreamReader file = new StreamReader(_path))
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    jObj = (JObject)JToken.ReadFrom(reader);
                    var json = JsonConvert.SerializeObject(t);
                    if (string.IsNullOrWhiteSpace(section))
                    {
                        jObj = JObject.Parse(json);
                    }
                    else
                    {
                        jObj[section] = JObject.Parse(json);
                    }
                }

                using (StreamWriter writer = new StreamWriter(_path))
                {
                    using (JsonTextWriter jsonWriter = new JsonTextWriter(writer))
                    {
                        jObj.WriteTo(jsonWriter);
                    }
                }
            }
            catch (System.Exception ex)
            {
                flag = false;
                throw ex;
            }
            return flag;
        }

        /// <summary>
        /// 删除指定section节点
        /// </summary>
        /// <param name="section">根节点</param>
        public void Remove(string section)
        {
            try
            {
                JObject jObj;
                using (StreamReader file = new StreamReader(_path))
                {
                    using (JsonTextReader reader = new JsonTextReader(file))
                    {
                        jObj = (JObject)JToken.ReadFrom(reader);
                        string[] nodes = section.Split(':');
                        //tempJToken = null;// jObj.SelectToken(nodes[0]).SelectToken(nodes[1]);
                        if (nodes.Length == 1)
                        {
                            jObj.Remove(nodes[0]);
                        }
                        else if (nodes.Length == 2)
                        {
                            ((JObject)jObj.SelectToken(nodes[0])).Remove(nodes[1]);
                        }
                        else if (nodes.Length == 3)
                        {
                            ((JObject)jObj.SelectToken(nodes[0]).SelectToken(nodes[1])).Remove(nodes[2]);
                        }
                        else if (nodes.Length == 4)
                        {
                            ((JObject)jObj.SelectToken(nodes[0]).SelectToken(nodes[1]).SelectToken(nodes[2])).Remove(nodes[3]);
                        }
                        else if (nodes.Length == 5)
                        {
                            ((JObject)jObj.SelectToken(nodes[0]).SelectToken(nodes[1]).SelectToken(nodes[2]).SelectToken(nodes[3])).Remove(nodes[4]);
                        }
                    }
                }
                using (StreamWriter writer = new StreamWriter(_path))
                {
                    using (JsonTextWriter jsonWriter = new JsonTextWriter(writer))
                    {
                        jObj.WriteTo(jsonWriter);
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 获取单个节点内容
        /// </summary>
        /// <param name="section">根节点</param>
        /// <returns></returns>
        public string ReadSingleNode(string sections)
        {
            return Configuration[sections];
        }
    }
}
