using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using RootMotion.FinalIK;

public class MyAvatarManager : MonoBehaviour, AvatarManager
{
    [SerializeField] private ControllerInputManager controllerInputManager;

    [SerializeField] private GameObject instantiationModelParent;

    [SerializeField] private GameObject hmd;
    [SerializeField] private GameObject leftController;
    [SerializeField] private GameObject rightController;

    [SerializeField] private GameObject vrikHeadTarget;
    [SerializeField] private GameObject vrikLeftHandTarget;
    [SerializeField] private GameObject vrikRightHandTarget;

    private string uuid;

    private AvatarState avatarState;

    private VRIK vrik;

    private bool isInKnifeSharpeningSetupEnteringArea = false;
    private KnifeSharpeningSetupManager targetSharpeningSetupManager;

    void Awake()
    {
        uuid = Guid.NewGuid().ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        avatarState = AvatarState.Walking;
    }

    // Update is called once per frame
    void Update()
    {
        switch (avatarState)
        {
            case AvatarState.Walking:
                
                if (controllerInputManager == null)
                {
                    controllerInputManager = this.transform.GetComponent<ControllerInputManager>();
                    Debug.Log("nulldayo");
                }
                if (controllerInputManager.IsPressedButtonA && isInKnifeSharpeningSetupEnteringArea)
                {
                    // モードの遷移
                    transform.position = targetSharpeningSetupManager.transform.position;
                    avatarState = AvatarState.KnifeSharpening;
                }
                break;
            case AvatarState.KnifeSharpening:
                if (controllerInputManager.IsPressedButtonB)
                {
                    // モードの遷移
                    avatarState = AvatarState.Walking;
                }
                break;
        }
        UpdateVrikTargetPosture();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "KnifeSharpeningSetup")
        {
            isInKnifeSharpeningSetupEnteringArea = true;
            targetSharpeningSetupManager = other.transform.GetComponent<KnifeSharpeningSetupManager>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "KnifeSharpeningSetup")
        {
            isInKnifeSharpeningSetupEnteringArea = false;
        }
    }

    public string UUID()
    {
        return uuid;
    }

    public Vector3 HeadPosition()
    {
        return hmd.transform.position;
    }

    public Vector3 HeadRotation()
    {
        return hmd.transform.rotation.eulerAngles;
    }

    public void InitializeAvatar(string avatarAssetPath)
    {
        GameObject avatarModel = (GameObject)Resources.Load(avatarAssetPath);
        avatarModel = Instantiate(avatarModel, instantiationModelParent.transform);
        avatarModel.AddComponent<VRIK>();
        vrik = avatarModel.GetComponent<VRIK>();
        vrik.solver.spine.headTarget = vrikHeadTarget.transform;
        vrik.solver.leftArm.target = vrikLeftHandTarget.transform;
        vrik.solver.rightArm.target = vrikRightHandTarget.transform;
    }

    private void UpdateVrikTargetPosture()
    {
        switch (avatarState)
        {
            case AvatarState.Walking:
                vrikHeadTarget.transform.position = hmd.transform.position;
                vrikHeadTarget.transform.rotation = Quaternion.Euler(hmd.transform.rotation.eulerAngles + new Vector3(0, -90, -90));

                vrikLeftHandTarget.transform.position = leftController.transform.position;
                vrikLeftHandTarget.transform.rotation = leftController.transform.rotation;

                vrikRightHandTarget.transform.position = rightController.transform.position;
                vrikRightHandTarget.transform.rotation = rightController.transform.rotation;

                break;

            case AvatarState.KnifeSharpening:
                vrikHeadTarget.transform.position = hmd.transform.position;
                vrikHeadTarget.transform.rotation = Quaternion.Euler(hmd.transform.rotation.eulerAngles + new Vector3(0, -90, -90));

                vrikLeftHandTarget.transform.position =　targetSharpeningSetupManager.MinReachingOrigin.transform.position;
                
                vrikRightHandTarget.transform.position = targetSharpeningSetupManager.MinReachingOrigin.transform.position;
                break;
        }

    }
}
