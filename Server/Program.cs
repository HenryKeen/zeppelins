using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
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
                    server.Bind(string.Format("tcp://192.168.43.121:{0}", port));

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
            return NetworkInterface.GetAllNetworkInterfaces().First(x => x.Name.ToLower() == "wi-fi").GetIPProperties().UnicastAddresses.First().Address;

            foreach (NetworkInterface netif in NetworkInterface.GetAllNetworkInterfaces())
            {
                Console.WriteLine("Network Interface: {0}", netif.Name);
                IPInterfaceProperties properties = netif.GetIPProperties();

                foreach (IPAddressInformation unicast in properties.UnicastAddresses)
                    Console.WriteLine("\tUniCast: {0}", unicast.Address);
            }
        }
    }
}
