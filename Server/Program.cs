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
            bool isDead = false;

            using (NetMQContext ctx = NetMQContext.Create())
            {
                using (DealerSocket server = ctx.CreateDealerSocket())
                {
                    const int port = 5556;
                    server.Bind(string.Format("tcp://192.168.43.121:{0}", port));

                    while (true)
                    {
                        if (isDead)
                        {
                            server.Send(string.Format("I'm dead, stop poking me"));
                        }
                        else
                        {
                            Console.WriteLine("Trying to send the secret");
                            server.Send(string.Format("Zepplins-{0}", Guid.NewGuid()));
                            Console.WriteLine("Sent the secret");
                            isDead = true;
                        }
                    }
                }
            }
        }
    }
}
