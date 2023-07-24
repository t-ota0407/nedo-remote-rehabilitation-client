using System;
using UnityEngine;

public class OthersAvatar
{
    public readonly string userUuid;

    private GameObject avatarModel;

    public GameObject vrikHeadTarget;
    private GameObject vrikLeftHandTarget;
    private GameObject vrikRightHandTarget;
    private GameObject vrikLeftRegTarget;
    private GameObject vrikRightRegTarget;

    private DateTime lastDataTimestamp = DateTime.MinValue;

    public OthersAvatar(string userUuid)
    {
        this.userUuid = userUuid;
    }

    public void InitializeAvatar(
        GameObject avatarModel,
        GameObject vrikHeadTarget,
        GameObject vrikLeftHandTarget,
        GameObject vrikRightHandTarget,
        GameObject vrikLeftRegTarget,
        GameObject vrikRightRegTarget)
    {
        this.avatarModel = avatarModel;
        this.vrikHeadTarget = vrikHeadTarget;
        this.vrikLeftHandTarget = vrikLeftHandTarget;
        this.vrikRightHandTarget = vrikRightHandTarget;
        this.vrikLeftRegTarget = vrikLeftRegTarget;
        this.vrikRightRegTarget = vrikRightRegTarget;

        Debug.Log("initialized!!");
    }

    public void UpdateAvatar(DateTime timestamp, SyncCommunicationUser syncCommunicationUser)
    {
        Debug.Log("called");
        Debug.Log($"timestamp: {timestamp}");
        Debug.Log($"timestamp: {lastDataTimestamp}");
        if (timestamp > lastDataTimestamp)
        {
            Debug.Log("in");
            Posture headPosture = syncCommunicationUser.headPosture;
            Debug.Log(headPosture.position);
            vrikHeadTarget.transform.position = headPosture.position;
            vrikHeadTarget.transform.rotation = Quaternion.Euler(headPosture.rotation);

            Posture leftHandPosture = syncCommunicationUser.leftHandPosture;
            vrikLeftHandTarget.transform.position = leftHandPosture.position;
            vrikLeftHandTarget.transform.rotation = Quaternion.Euler(leftHandPosture.rotation);

            Posture rightHandPosture = syncCommunicationUser.rightHandPosture;
            vrikRightHandTarget.transform.position = rightHandPosture.position;
            vrikRightHandTarget.transform.rotation = Quaternion.Euler(rightHandPosture.rotation);

            // todo: leftReg‚ÆrightReg‚à“¯Šú‚³‚¹‚é

            lastDataTimestamp = timestamp;
        }
    }
}
