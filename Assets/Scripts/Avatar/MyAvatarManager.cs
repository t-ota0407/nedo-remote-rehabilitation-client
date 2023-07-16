using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using RootMotion.FinalIK;


public class MyAvatarManager : MonoBehaviour, AvatarManager
{
    private const int AVATAR_STATE_HOLDING_MILI_SECONDS = 1500;

    [SerializeField] private ControllerInputManager controllerInputManager;

    [SerializeField] private GameObject xrOrigin;
    [SerializeField] private GameObject instantiationModelParent;

    [SerializeField] private GameObject hmd;
    [SerializeField] private GameObject leftController;
    [SerializeField] private GameObject rightController;

    [SerializeField] private GameObject leftRayInteractor;
    [SerializeField] private GameObject rightRayInteractor;

    [SerializeField] private GameObject vrikHeadTarget;
    [SerializeField] private GameObject vrikLeftHandTarget;
    [SerializeField] private GameObject vrikRightHandTarget;
    [SerializeField] private GameObject vrikLeftRegTarget;
    [SerializeField] private GameObject vrikRightRegTarget;

    [SerializeField] private GamificationManager gamificationManager;

    private string uuid;

    private AvatarState avatarState;
    private DateTime avatarStateUpdatedAt;

    private AvatarCalibration avatarCalibration = new();

    private MSRBAvatarConfiguration msrbAvatarConfiguration;

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

        SetControllerAndRaysVisibility(false);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAvatarState();
        UpdateVrikTargetPosture();
        CheckAvatarCalibration();
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

    public float ReachingProgress()
    {
        if (avatarState == AvatarState.Walking)
        {
            return 0;
        }

        float currentControllersDistance = Vector3.Distance(leftController.transform.position, rightController.transform.position);
        float reachingProgress = Mathf.Clamp01((avatarCalibration.maxReachedControllerDistance - currentControllersDistance) / (avatarCalibration.maxReachedControllerDistance - avatarCalibration.minReachedControllerDistance));

        return reachingProgress;
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
        msrbAvatarConfiguration = avatarModel.GetComponent<MSRBAvatarConfiguration>();
    }

    private void UpdateAvatarState()
    {
        switch (avatarState)
        {
            case AvatarState.Walking: // Walking => KnifeSharpening
                if (controllerInputManager == null)
                {
                    controllerInputManager = transform.GetComponent<ControllerInputManager>();
                }

                if (controllerInputManager.IsPressedTrigger
                    && isInKnifeSharpeningSetupEnteringArea
                    && (DateTime.Now - avatarStateUpdatedAt).TotalMilliseconds > AVATAR_STATE_HOLDING_MILI_SECONDS)
                {
                    xrOrigin.transform.position = targetSharpeningSetupManager.StandingOrigin.transform.position;
                    xrOrigin.transform.localRotation = targetSharpeningSetupManager.transform.rotation * Quaternion.Euler(0, 90, 0);

                    gamificationManager.ContinueGame(targetSharpeningSetupManager);

                    vrik.solver.leftLeg.target = vrikLeftRegTarget.transform;
                    vrik.solver.leftLeg.positionWeight = 0.9f;
                    vrik.solver.rightLeg.target = vrikRightRegTarget.transform;
                    vrik.solver.rightLeg.positionWeight = 0.9f;

                    avatarState = AvatarState.KnifeSharpening;
                    avatarStateUpdatedAt = DateTime.Now;
                }

                break;

            case AvatarState.KnifeSharpening: // KnifeSharpening => Walking
                if (controllerInputManager.IsPressedTrigger
                    && (DateTime.Now - avatarStateUpdatedAt).TotalMilliseconds > AVATAR_STATE_HOLDING_MILI_SECONDS)
                {
                    vrik.solver.leftLeg.target = null;
                    vrik.solver.leftLeg.positionWeight = 0;
                    vrik.solver.rightLeg.target = null;
                    vrik.solver.rightLeg.positionWeight = 0;

                    avatarState = AvatarState.Walking;
                    avatarStateUpdatedAt = DateTime.Now;
                }

                break;
        }
    }

    private void UpdateVrikTargetPosture()
    {
        switch (avatarState)
        {
            case AvatarState.Walking:
                vrikHeadTarget.transform.position = hmd.transform.position;
                vrikHeadTarget.transform.rotation = hmd.transform.rotation * Quaternion.Euler(0, -90, -90);

                vrikLeftHandTarget.transform.position = leftController.transform.position;
                vrikLeftHandTarget.transform.rotation = leftController.transform.rotation * Quaternion.Euler(90, 0, -90);

                vrikRightHandTarget.transform.position = rightController.transform.position;
                vrikRightHandTarget.transform.rotation = rightController.transform.rotation * Quaternion.Euler(-90, 0, 90);

                break;

            case AvatarState.KnifeSharpening:
                vrikHeadTarget.transform.position = hmd.transform.position;
                vrikHeadTarget.transform.rotation = Quaternion.Euler(hmd.transform.rotation.eulerAngles + new Vector3(0, -90, -90));

                float reachingProgress = ReachingProgress();
                Vector3 minReachingPosition = targetSharpeningSetupManager.MinReachingOrigin.transform.position;
                Vector3 maxReachingPosition = targetSharpeningSetupManager.MaxReachingOrigin.transform.position;
                Vector3 reachingTargetPosition = Vector3.Lerp(minReachingPosition, maxReachingPosition, reachingProgress);

                vrikLeftHandTarget.transform.position =Å@reachingTargetPosition;
                vrikRightHandTarget.transform.position = reachingTargetPosition;

                vrikLeftHandTarget.transform.localPosition = vrikLeftHandTarget.transform.localPosition + new Vector3(0, 0, 0.1f);
                vrikRightHandTarget.transform.localPosition = vrikRightHandTarget.transform.localPosition + new Vector3(-0.15f, -0.03f, 0.08f);

                vrikLeftHandTarget.transform.rotation = targetSharpeningSetupManager.transform.rotation * Quaternion.Euler(0, 0, 180f);
                vrikRightHandTarget.transform.rotation = targetSharpeningSetupManager.transform.rotation * Quaternion.Euler(0, 0, 180f);

                vrikLeftRegTarget.transform.position = targetSharpeningSetupManager.StandingOrigin.transform.position;
                vrikRightRegTarget.transform.position = targetSharpeningSetupManager.StandingOrigin.transform.position;

                vrikLeftRegTarget.transform.localPosition = vrikLeftRegTarget.transform.localPosition + new Vector3(0.15f, 0, 0);
                vrikRightRegTarget.transform.localPosition = vrikRightRegTarget.transform.localPosition + new Vector3(-0.15f, 0, 0);

                break;
        }
    }

    private void CheckAvatarCalibration()
    {
        switch (avatarState)
        {
            case AvatarState.Walking:
                if (controllerInputManager.IsPressedButtonA)
                {
                    avatarCalibration.seatedHeadHeight = hmd.transform.localPosition.y;

                    float offsetHeight = msrbAvatarConfiguration.StandardCameraHeight - avatarCalibration.seatedHeadHeight;
                    transform.localPosition = new Vector3(0, offsetHeight, 0);
                }

                break;

            case AvatarState.KnifeSharpening:
                if (controllerInputManager.IsPressedButtonA)
                {
                    avatarCalibration.maxReachedControllerDistance = Vector3.Distance(leftController.transform.position, rightController.transform.position);
                }

                if (controllerInputManager.IsPressedButtonB)
                {
                    avatarCalibration.minReachedControllerDistance = Vector3.Distance(leftController.transform.position, rightController.transform.position);
                }

                break;
        }
    }

    private void SetControllerAndRaysVisibility(bool visibility)
    {
        leftController.GetComponent<ActionBasedController>().hideControllerModel = !visibility;
        rightController.GetComponent<ActionBasedController>().hideControllerModel = !visibility;
        leftRayInteractor.GetComponent<XRRayInteractor>().enabled = visibility;
        rightRayInteractor.GetComponent<XRRayInteractor>().enabled = visibility;
    }
}
