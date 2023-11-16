using WebSocketSharp.Server;
using WebSocketSharp;
using Newtonsoft.Json.Linq;
using SCCC;
using System.Globalization;

namespace SCCC
{
    public class SCCCManager
    {
        public bool ready = false;
        public bool server = false;
        public bool client = false;
        public WebSocketServer wssv;
        public WebSocket wscli;

        public SCCCManager()
        {
            ready = false;
        }

        public void StartServer(int port)
        {
            if (!ready)
            {
                server = true;
                wssv = new WebSocketServer(port, false);

                wssv.AddWebSocketService<SCCCcomm>("/");
                wssv.AddWebSocketService<CGIcomm>("/t0");
                wssv.Start();
                ready = true;
                MyView.state = SCCC.State.SERVER;
            }
            else
            {
                throw new Exception("This instance is already running!");
            }
        }
        public void StartClient(string addr)
        {
            if (!ready)
            {
                client = true;
                wscli = new WebSocket(addr);

                wscli.OnMessage += (sender, e) =>
                {
                    JObject o = JObject.Parse(e.Data);
                    switch (o["ident"].Value<int>())
                    {
                        case 0:
                            MyView.runtime.reciveT0Update(o["newT0"].Value<string>());
                            break;
                        default:
                            break;
                    };

                };
                wscli.Connect();
                ready = true;
            }
            else
            {
                throw new Exception("This instance is already running!");
            }
        }



        internal void stop()
        {
            if (wssv != null)
            {
                wssv.Stop();
            }

        }


        public void sendT0Update()
        {
            if (ready)
            {
                JObject o = new JObject
                {
                    new JProperty("ident",0),
                    new JProperty("content", MyView.tZero.ToString("R",CultureInfo.InvariantCulture))
                };
                if (server)
                {
                    wssv.WebSocketServices["/"].Sessions.BroadcastAsync(o.ToString(), () => { });
                    wssv.WebSocketServices["/t0"].Sessions.BroadcastAsync(MyView.tZero.Subtract(TimeSpan.FromHours(1)).ToString("R"), () => { });
                }
                else
                {
                    wscli.Send(o.ToString());
                }
            }
        }

        public void sendTimeTableUpdate(string input)
        {
            JObject o = new JObject()
            {
                new JProperty("ident",1),
                new JProperty("content", input)
            };
            if (server)
            {
                wssv.WebSocketServices["/"].Sessions.BroadcastAsync(o.ToString(), () => { });
            }
            else if(client && ready)
            {
                wscli.Send(o.ToString());
            }
        }

        public void reciveTimeTableUpdate(string input)
        {
            Program.view.updateTable(input);
        }

        public void reciveT0Update(string strDate)
        {
            DateTime dateTime = DateTime.ParseExact(strDate,"R",CultureInfo.InvariantCulture);
            MyView.tZero = dateTime;
            if (server)
            {
                wssv.WebSocketServices["/t0"].Sessions.BroadcastAsync(MyView.tZero.Subtract(TimeSpan.FromHours(1)).ToString("R"), () => { });
            }
        }
    }

    public class SCCCcomm : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            JObject o = JObject.Parse(e.Data);
            switch (o["ident"].Value<int>()) {
                case 0:
                    MyView.runtime.reciveT0Update(o["content"].Value<string>());
                    break;
                case 1:
                    MyView.runtime.reciveTimeTableUpdate(o["content"].Value<string>());
                    break;
                default:
                    break;
            };
        }

    }

    public class CGIcomm : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            Sessions.BroadcastAsync(MyView.tZero.ToString("R"), () => {});
        }
    }
}
