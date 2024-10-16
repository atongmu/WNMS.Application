using cn.jpush.api;
using cn.jpush.api.common;
using cn.jpush.api.device;
using cn.jpush.api.push.mode;
using cn.jpush.api.push.notification;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WNMS.Utility;

namespace WNMS.Application.Utility.Jpush
{
    public class WorkOrderJpush
    {
        //Jpush 任务推送
        string soft_key = StaticConstraint.JpushSoftKey;
        string app_key = StaticConstraint.JpushAppKey;
        string master_secret = StaticConstraint.JpushMasterSecret;

        //woType工单类型：1维修工单，2巡检工单
        public void Jpush(string[] userID, string JpushContent, string JpushMark, int woType)
        {
            JPushClient client = new JPushClient(app_key, master_secret);
            for (int i = 0; i < userID.Length; i++)
            {
                userID[i] = soft_key + "_" + userID[i];
            }
            PushPayload payload = PushObject_all_Tag_alert(userID, JpushContent, JpushMark, woType);
            try
            {
                var result = client.SendPush(payload);
            }
            catch (Exception e)
            {

            }
        }
        public static PushPayload PushObject_all_Tag_alert(string[] userID, string JpushContent, string JpushMark, int woType)
        {
            PushPayload pushPayload_Tag = new PushPayload();
            try
            {
                pushPayload_Tag.platform = Platform.all();
                //pushPayload_Tag.audience = Audience.s_tag("tag2");
                //pushPayload_Tag.audience = Audience.s_alias(userID);
                pushPayload_Tag.audience = Audience.all();
                //pushPayload_Tag.audience = Audience.s_registrationId("");
                //pushPayload_Tag.notification = new Notification().setAlert(woID);
                pushPayload_Tag.notification = new Notification();
                string type = System.Enum.GetName(typeof(WNMS.Model.CustomizedClass.IncidentType), woType);
                if (woType == 1)
                {
                    pushPayload_Tag.notification.IosNotification = new IosNotification().setAlert(JpushContent)
                                                  .setBadge(5)
                                                  .setSound("happy")
                                                   .AddExtra("from", type)
                                                  .AddExtra("mark", JpushMark);
                    pushPayload_Tag.notification.AndroidNotification = new AndroidNotification().setAlert(JpushContent)
                                                            .AddExtra("from", type)
                                                  .AddExtra("mark", JpushMark);
                }
                else
                {
                    pushPayload_Tag.notification.IosNotification = new IosNotification().setAlert(JpushContent)
                                                  .setBadge(5)
                                                  .setSound("happy")
                                                   .AddExtra("from", type)
                                                  .AddExtra("mark", JpushMark);
                    pushPayload_Tag.notification.AndroidNotification = new AndroidNotification().setAlert(JpushContent)
                                                    .AddExtra("from", type)
                                                  .AddExtra("mark", JpushMark);
                }
            }
            catch (Exception ee)
            {

            }
            return pushPayload_Tag;
        }
        public TagListResult GetTageList()
        {
            String url = "https://device.jpush.cn" + "/v3/tags" + "/";
            String auth = GetBase64Encode(app_key + ":" + master_secret);
            ResponseWrapper response = new BaseHttpClient().sendGet(url, auth, null);
            return TagListResult.fromResponse(response);
        }
        public string GetBase64Encode(string str)
        {
            byte[] bytes = Encoding.Default.GetBytes(str);
            return Convert.ToBase64String(bytes);
        }
    }
}
