using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RootMotion.FinalIK;

public class OthersAvatarManager : MonoBehaviour
{
    private List<OthersAvatar> activeOthersAvatars = new();

    private Queue<UDPDownloadUser> othersAvatarUpdateQueue = new();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        while (othersAvatarUpdateQueue.Count > 0)
        {
            UDPDownloadUser udpDownloadUser = othersAvatarUpdateQueue.Dequeue();

            SyncCommunicationUser syncCommunicationUser = udpDownloadUser.user;
            DateTime timestamp = DateTime.ParseExact(udpDownloadUser.timestamp, "yyyy/MM/dd HH:mm:ss.fff", null);
            Debug.Log(timestamp.ToString());

            bool isAvatarExists = activeOthersAvatars.Any(avatar => avatar.userUuid == syncCommunicationUser.userUuid);

            if (isAvatarExists)
            {
                OthersAvatar targetOthersAvatar = activeOthersAvatars.Where(avatar => avatar.userUuid == syncCommunicationUser.userUuid).ToList()[0];
                
                targetOthersAvatar.UpdateAvatar(timestamp, syncCommunicationUser);
            }
            else
            {
                Debug.Log("null");
                OthersAvatar othersAvatar = new(syncCommunicationUser.userUuid);
                activeOthersAvatars.Add(othersAvatar);

                GameObject vrikHeadTarget = new GameObject($"{othersAvatar.userUuid}_head");
                GameObject vrikLeftHandTarget = new GameObject($"{othersAvatar.userUuid}_leftHand");
                GameObject vrikRightHandTarget = new GameObject($"{othersAvatar.userUuid}_rightHand");
                GameObject vrikLeftRegTarget = new GameObject($"{othersAvatar.userUuid}_leftReg");
                GameObject vrikRightRegTarget = new GameObject($"{othersAvatar.userUuid}_rightReg");

                string avatarAssetPath = "Prefabs/Avatars/Female_Adult_01 Variant";
                GameObject avatarModel = (GameObject)Resources.Load(avatarAssetPath);
                avatarModel = Instantiate(avatarModel, this.transform);
                avatarModel.AddComponent<VRIK>();
                VRIK vrik = avatarModel.GetComponent<VRIK>();
                vrik.solver.spine.headTarget = vrikHeadTarget.transform;
                vrik.solver.leftArm.target = vrikLeftHandTarget.transform;
                vrik.solver.rightArm.target = vrikRightHandTarget.transform;

                othersAvatar.InitializeAvatar(
                    avatarModel,
                    vrikHeadTarget,
                    vrikLeftHandTarget,
                    vrikRightHandTarget,
                    vrikLeftRegTarget,
                    vrikRightRegTarget);
            }
        }
    }

    public void EnqueueOthersAvatarUpdate(UDPDownloadUser udpDownloadUser)
    {
        othersAvatarUpdateQueue.Enqueue(udpDownloadUser);
    }

/*    public void CreateOrUpdateOthersAvatar(DateTime timestamp, SyncCommunicationUser syncCommunicationUser)
    {

        bool isAvatarExists = activeOthersAvatars.Any(avatar => avatar.userUuid == syncCommunicationUser.userUuid);

        if (isAvatarExists)
        {
            OthersAvatar targetOthersAvatar = activeOthersAvatars.Where(avatar => avatar.userUuid == syncCommunicationUser.userUuid).ToList()[0];

            if (targetOthersAvatar.IsInitialized)
            {
                targetOthersAvatar.UpdateAvatar(timestamp, syncCommunicationUser);
            }
        }
        else
        {
            Debug.Log("null");
            OthersAvatar othersAvatar = new(syncCommunicationUser.userUuid);
            activeOthersAvatars.Add(othersAvatar);
        }
    }*/
}
