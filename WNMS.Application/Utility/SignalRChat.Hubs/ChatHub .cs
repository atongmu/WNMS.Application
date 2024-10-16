using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
namespace WNMS.Application.Utility.SignalRChat.Hubs
{
    public class ChatHub:Hub
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventinfo">rtuid,eventSource   两者用逗号隔开</param>
        /// <param name="CancleEvent">报警消失还是发生，true 代表报警消失，false代表报警发生</param>
        /// <returns></returns>
        public async Task Pushmessage(string eventinfo, bool CancleEvent)
        {
            await Clients.All.SendAsync("ReceiveMessage", eventinfo, CancleEvent);
        }
    }
}
