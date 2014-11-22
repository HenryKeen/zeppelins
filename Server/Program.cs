using System;
using System.Collections.Generic;
using System.Linq;
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

            using (NetMQContext ctx = NetMQContext.Create())
            {
                using (DealerSocket server = ctx.CreateDealerSocket())
                {
                    int port = new Random().Next(3000, 7000);
                    server.Bind(string.Format("tcp://127.0.0.1:{0}", port));

                    string receivedMessage = server.ReceiveString();
                    string endpoint = server.Options.GetLastEndpoint;
                    Console.WriteLine("Server received message '{0}' from '{1}'", receivedMessage, endpoint);

                    server.Send(string.Format("Zepplins-{0} {1}", Guid.NewGuid(), endpoint));
                }
            }
        }
    }
}
