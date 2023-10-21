using System;
using UnityEngine;

public class OthersAvatar
{
    public readonly string userUuid;

    private GameObject avatarModel;

    public GameObject vrikHeadTarget;
    private GameObject vrikLeftHandTarget;
    private GameObject vrikRightHandTarget;
    private GameObject vrikLeftLegTarget;
    private GameObject vrikRightLegTarget;

    public DateTime LastUpdateTimestamp { get { return lastUpdataTimestamp; } }
    private DateTime lastUpdataTimestamp = DateTime.Now;

    public OthersAvatar(string userUuid)
    {
        this.userUuid = userUuid;
    }

    public void InitializeAvatar(
        GameObject avatarModel,
        GameObject vrikHeadTarget,
        GameObject vrikLeftHandTarget,
        GameObject vrikRightHandTarget,
        GameObject vrikLeftLegTarget,
        GameObject vrikRightLegTarget)
    {
        this.avatarModel = avatarModel;
        this.vrikHeadTarget = vrikHeadTarget;
        this.vrikLeftHandTarget = vrikLeftHandTarget;
        this.vrikRightHandTarget = vrikRightHandTarget;
        this.vrikLeftLegTarget = vrikLeftLegTarget;
        this.vrikRightLegTarget = vrikRightLegTarget;
    }

    public void UpdateAvatar(DateTime timestamp, SyncCommunicationUser syncCommunicationUser)
    {
        if (timestamp > lastUpdataTimestamp)
        {
            Posture headPosture = syncCommunicationUser.headPosture;
            vrikHeadTarget.transform.position = headPosture.position;
            vrikHeadTarget.transform.rotation = Quaternion.Euler(headPosture.rotation);

            Posture leftHandPosture = syncCommunicationUser.leftHandPosture;
            vrikLeftHandTarget.transform.position = leftHandPosture.position;
            vrikLeftHandTarget.transform.rotation = Quaternion.Euler(leftHandPosture.rotation);

            Posture rightHandPosture = syncCommunicationUser.rightHandPosture;
            vrikRightHandTarget.transform.position = rightHandPosture.position;
            vrikRightHandTarget.transform.rotation = Quaternion.Euler(rightHandPosture.rotation);

            if (AvatarStateConverter.FromString(syncCommunicationUser.avatarState) == AvatarState.KnifeSharpening)
            {
                Posture leftLegPosture = syncCommunicationUser.leftLegPosture;
                vrikLeftLegTarget.transform.position = leftLegPosture.position;
                vrikLeftLegTarget.transform.rotation = Quaternion.Euler(leftLegPosture.rotation);

                Posture rightLegPosture = syncCommunicationUser.rightLegPosture;
                vrikRightLegTarget.transform.position = rightLegPosture.position;
                vrikRightLegTarget.transform.rotation = Quaternion.Euler(rightLegPosture.rotation);
            }

            lastUpdataTimestamp = timestamp;
        }
    }

    public void DeleteAvatar()
    {
        // todo: パフォーマンス最適化
        this.avatarModel.SetActive(false);
    }
}
