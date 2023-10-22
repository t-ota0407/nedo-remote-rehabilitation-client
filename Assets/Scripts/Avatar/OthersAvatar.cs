using System;
using UnityEngine;
using RootMotion.FinalIK;

public class OthersAvatar : MonoBehaviour
{
    public string userUuid;

    private GameObject avatarModel;

    public GameObject vrikHeadTarget;
    private GameObject vrikLeftHandTarget;
    private GameObject vrikRightHandTarget;
    private GameObject vrikLeftLegTarget;
    private GameObject vrikRightLegTarget;

    public DateTime LastUpdateTimestamp { get { return lastUpdataTimestamp; } }
    private DateTime lastUpdataTimestamp = DateTime.Now;

    private KnifeSharpeningSetupManager targetSharpeningSetupManager;

    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == ConstantObjectTag.KNIFE_SHARPENING_SETUP)
        {
            targetSharpeningSetupManager = other.transform.GetComponent<KnifeSharpeningSetupManager>();
        }
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

            VRIK vrik = avatarModel.GetComponent<VRIK>();
            if (AvatarStateConverter.FromString(syncCommunicationUser.avatarState) == AvatarState.KnifeSharpening)
            {
                Posture leftLegPosture = syncCommunicationUser.leftLegPosture;
                vrikLeftLegTarget.transform.position = leftLegPosture.position;
                vrikLeftLegTarget.transform.rotation = Quaternion.Euler(leftLegPosture.rotation);

                Posture rightLegPosture = syncCommunicationUser.rightLegPosture;
                vrikRightLegTarget.transform.position = rightLegPosture.position;
                vrikRightLegTarget.transform.rotation = Quaternion.Euler(rightLegPosture.rotation);

                vrik.solver.leftLeg.target = vrikLeftLegTarget.transform;
                vrik.solver.leftLeg.positionWeight = 0.9f;
                vrik.solver.rightLeg.target = vrikRightLegTarget.transform;
                vrik.solver.rightLeg.positionWeight = 0.9f;

                if (targetSharpeningSetupManager != null)
                {
                    Debug.Log("nullではない");
                    float reachingProgress = syncCommunicationUser.reachingProgress;
                    Vector3 minReachingPosition = targetSharpeningSetupManager.MinReachingOrigin.transform.position;
                    Vector3 maxReachingPosition = targetSharpeningSetupManager.MaxReachingOrigin.transform.position;
                    Vector3 reachingTargetPosition = Vector3.Lerp(minReachingPosition, maxReachingPosition, reachingProgress);

                    Debug.Log(reachingProgress);
                    GameObject knifeObject = targetSharpeningSetupManager.KnifeManager.gameObject;
                    knifeObject.transform.position = reachingTargetPosition;
                    knifeObject.transform.localPosition = knifeObject.transform.localPosition + new Vector3(0, -0.04f, -0.24f);
                }
            }
            else
            {
                vrik.solver.leftLeg.target = null;
                vrik.solver.leftLeg.positionWeight = 0;
                vrik.solver.rightLeg.target = null;
                vrik.solver.rightLeg.positionWeight = 0;
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
