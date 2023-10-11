using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class SyncCommunicationManager : MonoBehaviour
{
    [SerializeField] private MyAvatarManager myAvatarManager;
    [SerializeField] private OthersAvatarManager othersAvatarManager;

    private bool isSyncCommunicating = false;

    private UDPCommunicationManager udpCommunicationManager;

    void Awake()
    {
        udpCommunicationManager = new(Config.serverIP, Config.serverUdpPort, Config.clientUdpPort);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        if (isSyncCommunicating)
        {
            UDPUploadUser udpUploadUser = new UDPUploadUser();
            udpUploadUser.timestamp = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.ff");
            udpUploadUser.user = new SyncCommunicationUser();
            udpUploadUser.user.userUuid = SingletonDatabase.Instance.myUserUuid;
            udpUploadUser.user.userName = SingletonDatabase.Instance.myUserName;
            udpUploadUser.user.avatarType = AvatarTypeConverter.ToString(SingletonDatabase.Instance.avatarType);
            udpUploadUser.user.rehabilitationCondition = "SIMPLE";
            udpUploadUser.user.headPosture = myAvatarManager.HeadPosture;
            udpUploadUser.user.leftHandPosture = myAvatarManager.LeftHandPosture;
            udpUploadUser.user.rightHandPosture = myAvatarManager.RightHandPosture;
            udpCommunicationManager.Send(udpUploadUser);
        }
    }

    public void StartSyncCommunication()
    {
        isSyncCommunicating = true;

        udpCommunicationManager.Listen(OnUDPReceived);
    }

    private void OnUDPReceived(IAsyncResult result)
    {
        UdpClient receivingUdpClient = (UdpClient)result.AsyncState;
        IPEndPoint ipEndPoint = null;

        byte[] getByte = receivingUdpClient.EndReceive(result, ref ipEndPoint);
        string message = Encoding.UTF8.GetString(getByte);

        Debug.Log(message);

        try
        {
            UDPDownloadUser udpDownloadUserData = JsonUtility.FromJson<UDPDownloadUser>(message);
            Debug.Log("parse ok");

            othersAvatarManager.EnqueueOthersAvatarUpdate(udpDownloadUserData);
        }
        catch (Exception e)
        {
            Debug.LogError("UDP datagram parse error: " + e.Message);
            Debug.LogError(e.StackTrace);
        }

        receivingUdpClient.BeginReceive(OnUDPReceived, receivingUdpClient);
    }
}
