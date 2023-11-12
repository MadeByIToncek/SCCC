using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp.Server;
using WebSocketSharp;
using Newtonsoft.Json.Linq;

namespace SCCC
{
    public class SCCCManager : WebSocketBehavior
    {
        public bool ready;
        public bool server;
        public WebSocketServer wssv;
        
        public SCCCManager() {
            ready = false;
        }
        public void StartServer(int port) {
            server = true;
            wssv = new WebSocketServer(port, false);

            wssv.AddWebSocketService<SCCCManager>("/");
            wssv.AddWebSocketService<T0>("/t0");
            wssv.Start();
            ready = true;
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            JObject obj = JObject.Parse(e.Data);
            Console.WriteLine(obj.ToString());
            Console.WriteLine(e.Data);
            //Send(msg);
        }
        protected override void OnClose(CloseEventArgs e)
        {
            ready = false;
        }

        public void UpdateT0()
        {
            wssv.WebSocketServices["/t0"].Sessions.BroadcastAsync(MyView.tZero.Subtract(TimeSpan.FromHours(1)).ToString("R"), () => { });
        }
    }
    public class T0 : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            Sessions.BroadcastAsync(MyView.tZero.ToString("R"), () => {});
        }
    }
}
