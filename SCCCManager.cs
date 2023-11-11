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
        bool _ready;
        bool _server;
        
        public SCCCManager() {
            _ready = false;
        }
        public void StartServer(int port) {
            _server = true;
            var wssv = new WebSocketServer(port, false);

            wssv.AddWebSocketService<SCCCManager>("/");
            wssv.Start();
            _ready = true;
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
            _ready = false;
        }
    }
}
