using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using NetMQ;
using NetMQ.Sockets;
using NetMQ.zmq;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            RunServer();
        }

        static void RunServer()
        {
            bool isDead = false;

            IPAddress ip = GetIpAddress();

            using (NetMQContext ctx = NetMQContext.Create())
            {
                using (DealerSocket server = ctx.CreateDealerSocket())
                {
                    const int port = 5556;
                    server.Bind(string.Format("tcp://{0}:{1}", ip, port));

                    while (true)
                    {
                        if (isDead)
                        {
                            server.Send(string.Format("I'm dead, stop poking me"));
                        }
                        else
                        {
                            var secret = string.Format("Zepplins-{0}", Guid.NewGuid());

                            Console.WriteLine("Trying to send the secret...");
                            server.Send(secret);
                            Console.WriteLine("...sent secret '{0}'", secret);
                            
                            isDead = true;
                        }
                    }
                }
            }
        }

        static IPAddress GetIpAddress()
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            return host.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
        }
    }
}
