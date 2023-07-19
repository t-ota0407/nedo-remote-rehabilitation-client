using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncCommunicationManager : MonoBehaviour
{
    [SerializeField] private string serverIP;
    [SerializeField] private int serverHttpPort;
    [SerializeField] private int serverUdpPort;
    [SerializeField] private int clientUdpPort;

    private bool isSyncCommunicating = false;

    private UDPCommunicationManager udpCommunicationManager;

    void Awake()
    {
        udpCommunicationManager = new(serverIP, serverUdpPort, clientUdpPort);
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
            udpUploadUser.user.userUuid = "";
            udpUploadUser.user.rehabilitationCondition = "SIMPLE";
            udpUploadUser.user.headPosture = C();
            udpUploadUser.user.leftHandPosture = C();
            udpUploadUser.user.rightHandPosture = C();
            udpCommunicationManager.Send(udpUploadUser);
            Debug.Log("Send");
        }
    }

    private Posture C()
    {
        Posture posture = new Posture();
        posture.position = new Vector3();
        posture.rotation = new Vector3();
        return posture;
    }

    public void StartSyncCommunication()
    {
        isSyncCommunicating = true;
    }
}
