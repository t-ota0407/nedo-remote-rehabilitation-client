using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RootMotion.FinalIK;

public class OthersAvatarManager : MonoBehaviour
{
    private const float AVATAR_DELETE_WAITING_MILISECONDS = 15000;

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

            // 一旦クライアントで生成した値を使うことにする。
            DateTime timestamp = DateTime.Now; // ParseDateTimeString(udpDownloadUser.timestamp);

            bool isAvatarExists = activeOthersAvatars.Any(avatar => avatar.userUuid == syncCommunicationUser.userUuid);

            if (isAvatarExists)
            {
                OthersAvatar targetOthersAvatar = activeOthersAvatars.Where(avatar => avatar.userUuid == syncCommunicationUser.userUuid).ToList()[0];
                
                targetOthersAvatar.UpdateAvatar(timestamp, syncCommunicationUser);
            }
            else
            {
                AvatarType avatarType = AvatarTypeConverter.FromString(syncCommunicationUser.avatarType);
                string avatarAssetPath = AvatarTypeConverter.ToAssetPath(avatarType);
                GameObject avatarModel = (GameObject)Resources.Load(avatarAssetPath);
                avatarModel = Instantiate(avatarModel, transform);

                OthersAvatar othersAvatar = avatarModel.gameObject.AddComponent<OthersAvatar>();
                othersAvatar.userUuid = syncCommunicationUser.userUuid;
                activeOthersAvatars.Add(othersAvatar);

                GameObject vrikHeadTarget = new GameObject($"{othersAvatar.userUuid}_head");
                GameObject vrikLeftHandTarget = new GameObject($"{othersAvatar.userUuid}_leftHand");
                GameObject vrikRightHandTarget = new GameObject($"{othersAvatar.userUuid}_rightHand");
                GameObject vrikLeftRegTarget = new GameObject($"{othersAvatar.userUuid}_leftReg");
                GameObject vrikRightRegTarget = new GameObject($"{othersAvatar.userUuid}_rightReg");

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

        int listLength = activeOthersAvatars.Count;
        for (var i = 0; i < listLength; i++)
        {
            OthersAvatar activeOthersAvatar = activeOthersAvatars[i];
            
            double notUpdatedDuration = (DateTime.Now - activeOthersAvatar.LastUpdateTimestamp).TotalMilliseconds;
            if (notUpdatedDuration > AVATAR_DELETE_WAITING_MILISECONDS)
            {
                activeOthersAvatar.DeleteAvatar();
                activeOthersAvatars.RemoveAt(i);
                listLength -= 1;
            }
        }
    }

    public void EnqueueOthersAvatarUpdate(UDPDownloadUser udpDownloadUser)
    {
        othersAvatarUpdateQueue.Enqueue(udpDownloadUser);
    }

    private DateTime ParseDateTimeString(string dateTimeString)
    {
        List<string> expectedFormats = new List<string>() { "yyyy/MM/dd HH:mm:ss.ff", "yyyy/MM/dd HH:mm:ss.fff" };
        DateTime parsedDateTime = DateTime.Now;
        bool isParsingSuccessed = false;

        foreach (string expectedFormat in expectedFormats)
        {
            try
            {
                parsedDateTime = DateTime.ParseExact(dateTimeString, expectedFormat, null);
                isParsingSuccessed = true;
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
        
        if (!isParsingSuccessed)
        {
            throw new FormatException("String was not recognized as a valid DateTime");
        }

        return parsedDateTime;
    }
}
