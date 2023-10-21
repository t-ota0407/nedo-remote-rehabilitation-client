using System;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class UDPCommunicationManager
{
    private UdpClient udpClient;

    public bool IsListening { get { return isListening; } }
    private bool isListening = false;

    public UDPCommunicationManager(string serverIPAddress, int serverPort, int clientPort)
    {
        try
        {
            udpClient = new UdpClient(clientPort);
            udpClient.Connect(serverIPAddress, serverPort);
        }
        catch (SocketException e)
        {
            Debug.LogError("UDP connection error: " + e.Message);
        }
    }

    public void Send(UDPUploadUser udpUploadUser)
    {
        string json = JsonUtility.ToJson(udpUploadUser);
        var message = Encoding.UTF8.GetBytes(json);

        udpClient.Send(message, message.Length);
    }

    public void Listen(Action<IAsyncResult> OnReceived)
    {
        udpClient.BeginReceive(new AsyncCallback(OnReceived), udpClient);
        isListening = true;
    }

    public void Close() {
        udpClient.Close();
    }
}

