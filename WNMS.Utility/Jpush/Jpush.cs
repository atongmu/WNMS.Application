using System;
using System.Collections.Generic;
using System.Text;
using Jiguang.JPush;
using Jiguang.JPush.Model;

namespace WNMS.Utility.Jpush
{
    public class Jpush
    {
        static string app_key = StaticConstraint.JpushAppKey;
        static string master_secret = StaticConstraint.JpushMasterSecret;
        private static JPushClient client = new JPushClient(app_key, master_secret);

        //普通推送
        public void ExcutePush(PushPayload pload)
        {
            var response = client.SendPush(pload);
            Console.WriteLine(response.Content);
        }


        //创建可重复执行定时任务
        public HttpResponse ExecuteSchedule(PushPayload pl, Trigger trg,string name)
        {
            var response = client.Schedule.CreatePeriodicalScheduleTask(name, pl, trg);
            return response;
        }

        //修改可重复执行的定时任务
        public void EditPeriodicalSchedule(string scheduleId, string name, bool? enabled,
            Trigger trigger, PushPayload pushPayload)
        {
            var response = client.Schedule.UpdatePeriodicalScheduleTask(scheduleId, name, enabled, trigger, pushPayload);
            Console.WriteLine(response);
        }

        //按照id获取定时任务
        public HttpResponse GetSchedule(string scheduleid)
        {
            var response = client.Schedule.GetScheduleTask(scheduleid);
            return response;
        }

        //删除定时任务
        public HttpResponse DeleteSchedule(string scheduleid)
        {
            var response= client.Schedule.DeleteScheduleTask(scheduleid);
            return response;
        }

        #region 举例
        private static void ExecutePushExample()
        {
            PushPayload pushPayload = new PushPayload()
            {
                Platform = new List<string> { "android", "ios" },
                //Audience = { "alias":["4314","892","4531"] },
                Audience = new Audience
                {
                    Alias = { "1", "892", "4531" }
                },
                Notification = new Notification
                {
                    Alert = "hello jpush",
                    Android = new Android
                    {
                        Alert = "android alert",
                        Title = "title"
                    },
                    IOS = new IOS
                    {
                        Alert = "ios alert",
                        Badge = "+1"
                    }
                },
                Message = new Message
                {
                    Title = "message title",
                    Content = "message content",
                    Extras = new Dictionary<string, string>
                    {
                        ["key1"] = "value1"
                    }
                },
                Options = new Options
                {
                    IsApnsProduction = true // 设置 iOS 推送生产环境。不设置默认为开发环境。
                }
            };
            var response = client.SendPush(pushPayload);
            Console.WriteLine(response.Content);
        }

        private static void ExecuteBatchPushExample()
        {
            SinglePayload singlePayload = new SinglePayload()
            {
                Platform = new List<string> { "android", "ios" },
                Target = "flink",
                Notification = new Notification
                {
                    Alert = "hello jpush",
                    Android = new Android
                    {
                        Alert = "android alert",
                        Title = "title"
                    },
                    IOS = new IOS
                    {
                        Alert = "ios alert",
                        Badge = "+1"
                    }
                },
                Message = new Message
                {
                    Title = "message title",
                    Content = "message content",
                    Extras = new Dictionary<string, string>
                    {
                        ["key1"] = "value1"
                    }
                },
                Options = new Options
                {
                    IsApnsProduction = true // 设置 iOS 推送生产环境。不设置默认为开发环境。
                }
            };
            List<SinglePayload> singlePayloads = new List<SinglePayload>();
            singlePayloads.Add(singlePayload);
            Console.WriteLine("start send");
            var response = client.BatchPushByAlias(singlePayloads);
            Console.WriteLine(response.Content);
        }

        private static void ExecuteDeviceExample()
        {
            var registrationId = "12145125123151";
            var devicePayload = new DevicePayload
            {
                Alias = "alias1",
                Mobile = "12300000000",
                Tags = new Dictionary<string, object>
                {
                    { "add", new List<string>() { "tag1", "tag2" } },
                    { "remove", new List<string>() { "tag3", "tag4" } }
                }
            };
            var response = client.Device.UpdateDeviceInfo(registrationId, devicePayload);
            Console.WriteLine(response.Content);
        }

        private static void ExecuteReportExample()
        {
            var response = client.Report.GetMessageReport(new List<string> { "1251231231" });
            Console.WriteLine(response.Content);
        }

        private static void ExecuteReceivedDetailReportExample()
        {
            var response = client.Report.GetReceivedDetailReport(new List<string> { "1251231231" });
            Console.WriteLine(response.Content);
        }

        private static void ExecuteMessagesDetialReportExample()
        {
            var response = client.Report.GetMessagesDetailReport(new List<string> { "1251231231" });
            Console.WriteLine(response.Content);
        }

        private static void ExecuteScheduleExample()
        {
            var pushPayload = new PushPayload
            {
                Platform = "all",
                Notification = new Notification
                {
                    Alert = "Hello JPush"
                }
            };
            var trigger = new Trigger
            {
                StartDate = "2017-08-03 12:00:00",
                EndDate = "2017-12-30 12:00:00",
                TriggerTime = "12:00:00",
                TimeUnit = "week",
                Frequency = 2,
                TimeList = new List<string>
                {
                    "wed", "fri"
                }
            };
            var response = client.Schedule.CreatePeriodicalScheduleTask("task1", pushPayload, trigger);
            Console.WriteLine(response.Content);
        }
        #endregion
    }
}
