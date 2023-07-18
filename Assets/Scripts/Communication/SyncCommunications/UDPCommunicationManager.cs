using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using Newtonsoft.Json;

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

    public void Listen()
    {
        udpClient.BeginReceive(new AsyncCallback(OnReceived), udpClient);
        isListening = true;
    }

    private void OnReceived(IAsyncResult result)
    {
        UdpClient receivingUdpClient = (UdpClient)result.AsyncState;
        IPEndPoint ipEndPoint = null;

        byte[] getByte = receivingUdpClient.EndReceive(result, ref ipEndPoint);
        string message = Encoding.UTF8.GetString(getByte);

        try
        {
            UDPDownloadUser udpDownloadUserData = JsonUtility.FromJson<UDPDownloadUser>(message);

            // todo: データを使って同期処理をやる
            Debug.Log(udpDownloadUserData.timestamp);
        }
        catch(Exception e)
        {
            Debug.LogError("UDP datagram parse error: " + e.Message);
        }

        receivingUdpClient.BeginReceive(OnReceived, receivingUdpClient);
    }
}

