using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WNMS.Utility;

namespace WNMS.Application.Utility.MediaServer
{
    public class BackCloseStreams : BackgroundService
    {
        IHttpClientFactory httpClientFactory;
        public BackCloseStreams(IHttpClientFactory clientFactory)
        {
            httpClientFactory = clientFactory;
        }
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {

            var client = httpClientFactory.CreateClient();

            string serverUrl = StaticConstraint.MeidaServerHttp;

            while (!stoppingToken.IsCancellationRequested)
            {

                //var responseMessage = await client.GetAsync("http://localhost:8001/index/api/getMediaList");
                //if (responseMessage.IsSuccessStatusCode)
                //{
                //    var msg = await responseMessage.Content.ReadAsStringAsync();

                //    var model = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseModel<MediaInfo>>(msg);
                //    if (model.data != null) 
                //    {
                //        foreach (var item in model.data) 
                //        {

                //        }
                //    }
                //}

                var responseMessage = await client.GetAsync($"{serverUrl}/index/api/close_streams?force=0");
                if (responseMessage.IsSuccessStatusCode)
                {
                    var msg = await responseMessage.Content.ReadAsStringAsync();
                    var model = Newtonsoft.Json.JsonConvert.DeserializeObject<CloseReuslt>(msg);
                    await Task.Delay(1000 * 60);
                    Console.WriteLine("流关闭服务开启");
                }
                else
                {
                    await Task.Delay(1000 * 60 * 10);
                    Console.WriteLine("请检查是否开启了流媒体服务");
                }

            }
        }
    }
}
