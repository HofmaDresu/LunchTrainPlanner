using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace LunchTrainWeb.Hubs
{
    public class LunchHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }
    }
}