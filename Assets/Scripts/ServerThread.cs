using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public static class ServerThread
{
    private static readonly Thread ServerThr;
    public static event Action<float[]> DataArrived;

    static ServerThread()
    {
        ServerThr = new Thread(Runner);
    }

    private static void Runner(object obj)
    {
      
        var ipep = new IPEndPoint(IPAddress.Any, 4445);
        var SrvSock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        SrvSock.Bind(ipep);
        var sender = new IPEndPoint(IPAddress.Any, 4445);
        EndPoint Remote = sender;

        while (true)
        {
            var data = new byte[1024];
            var recv = SrvSock.ReceiveFrom(data, ref Remote);
            var str = Encoding.UTF8.GetString(data, 0, recv);
            if (str == "exit")
                break;
            try
            {
                var result = str.Split(',').Select(float.Parse).ToArray();
                if (result.Length == 2)
                    DataArrived?.Invoke(result);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }

            SrvSock.SendTo(data, data.Length, SocketFlags.None, Remote);
        }
    }

    public static void Start()
    {
        ServerThr.Start();
    }
}