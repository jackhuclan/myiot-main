using System.Net;
using System.Net.Sockets;

namespace VgAutoDrill.Fundation.CNC.Other;

public class VgTcpIpComm
{
    private Socket socketClient;
    public bool Connect(string ipaddress, int port)
    {
        try
        {
            IPAddress ip = IPAddress.Parse(ipaddress);
            IPEndPoint point = new IPEndPoint(ip, port);
            socketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Close();
            socketClient.Connect(point);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
        return true;
    }
    public void Close()
    {
        try
        {
            if (socketClient.Connected)
            {
                socketClient.Close();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

    }
    public byte[] SendAndReceive(byte[] msg)
    {
        try
        {
            if (socketClient.Connected)
            {
                socketClient.Send(msg);
                byte[] buffer = new byte[1024];
                socketClient.ReceiveTimeout = 3000;
                int count = socketClient.Receive(buffer, SocketFlags.None);
                return buffer.Take(count).ToArray();
            }
        }
        catch (Exception e)
        {
            throw e;
        }
        return null;
    }
    public void Send(byte[] msg)
    {
        socketClient.Send(msg);
    }
}
