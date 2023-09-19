using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using RootMotion.FinalIK;

public class MyAvatarManager : MonoBehaviour
{
    private const int AVATAR_STATE_HOLDING_MILI_SECONDS = 1500;

    [SerializeField] private RehabilitationSceneManager rehabilitationSceneManager;

    [SerializeField] private FadeManager fadeManager;
    [SerializeField] private LoadingProgressManager loadingProgressManager;

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

    [SerializeField] private AtHandUIManager atHandUIManager;

    [SerializeField] private HTTPCommunicationManager httpCommunicationManager;

    private string uuid;

    private AvatarState avatarState;
    private DateTime avatarStateUpdatedAt;

    private AvatarCalibration avatarCalibration = new();

    private MSRBAvatarConfiguration msrbAvatarConfiguration;

    private VRIK vrik;

    private bool isInKnifeSharpeningSetupEnteringArea = false;
    private KnifeSharpeningSetupManager targetSharpeningSetupManager;

    private List<TaskProgress<FinishRehabilitationTask>> finishRehabilitationTaskProgressList;

    void Awake()
    {
        uuid = Guid.NewGuid().ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        avatarState = AvatarState.Walking;

        SetControllerAndRaysVisibility(false);

        finishRehabilitationTaskProgressList = TaskProgress<FinishRehabilitationTask>.GenerateTaskProgressList();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAvatarState();
        UpdateVrikTargetPosture();
        CheckAvatarCalibration();

        CheckFinishRehabilitationTask();
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

    public Posture HeadPosture { get { return GetGameObjectPosture(vrikHeadTarget); } }
    public Posture LeftHandPosture { get { return GetGameObjectPosture(vrikLeftHandTarget); } }
    public Posture RightHandPosture { get { return GetGameObjectPosture(vrikRightHandTarget); } }
    public Posture LeftRegPosture { get { return GetGameObjectPosture(vrikLeftRegTarget); } }
    public Posture RightRegPosture { get { return GetGameObjectPosture(vrikRightRegTarget); } }

    private Posture GetGameObjectPosture(GameObject gameObject)
    {
        Posture posture = new();
        posture.position = gameObject.transform.position;
        posture.rotation = gameObject.transform.rotation.eulerAngles;
        return posture;
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

                if (controllerInputManager.IsPressedButtonX
                    && (DateTime.Now - avatarStateUpdatedAt).TotalMilliseconds > AVATAR_STATE_HOLDING_MILI_SECONDS)
                {
                    SetControllerAndRaysVisibility(true);
                    atHandUIManager.ActivateUI("終了メニュー", "リハビリテーションを終了します。よろしいですか？", "終了しない", "終了する", CancelToFinishRehabilitation, ExecuteCommunicationToFinishRehabilitation);

                    avatarState = AvatarState.InteractingWithUI;
                    avatarStateUpdatedAt = DateTime.Now;
                }

                if (controllerInputManager.IsPressedRightHandTrigger
                    && isInKnifeSharpeningSetupEnteringArea
                    && (DateTime.Now - avatarStateUpdatedAt).TotalMilliseconds > AVATAR_STATE_HOLDING_MILI_SECONDS)
                {
                    xrOrigin.transform.position = targetSharpeningSetupManager.StandingOrigin.transform.position;
                    xrOrigin.transform.localRotation = targetSharpeningSetupManager.transform.rotation * Quaternion.Euler(0, 90, 0);

                    Debug.Log("calling continue()");
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
                if (controllerInputManager.IsPressedRightHandTrigger
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

                vrikLeftHandTarget.transform.position =　reachingTargetPosition;
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

    private void CheckFinishRehabilitationTask()
    {
        var currentTaskProgress = TaskProgress<FinishRehabilitationTask>.GetCurrentTaskProgress(finishRehabilitationTaskProgressList);
        switch (currentTaskProgress.task)
        {
            case FinishRehabilitationTask.POST_RESULT:
                if (currentTaskProgress.progress == Progress.FAILED)
                {
                    // todo: POST_RESULTからやりなおし
                    currentTaskProgress.RetryTask();
                }
                break;
            case FinishRehabilitationTask.POST_SAVE_DATA:
                if (currentTaskProgress.progress == Progress.PENDING)
                {
                    ExecuteCommunicationToFinishRehabilitation();
                    currentTaskProgress.StartedTask();
                }
                if (currentTaskProgress.progress == Progress.FAILED)
                {
                    // todo: POST_SAVE_DATAからやりなおし
                    currentTaskProgress.RetryTask();
                }
                break;
            case FinishRehabilitationTask.FADING_OUT:
                if (currentTaskProgress.progress == Progress.PENDING)
                {
                    fadeManager.StartFadeOut();
                    loadingProgressManager.Display();
                    currentTaskProgress.StartedTask();

                }
                else if (currentTaskProgress.progress == Progress.DOING)
                {
                    if (fadeManager.FadeStatus == FadeStatus.FADED_OUT)
                    {
                        currentTaskProgress.FinishedTask();
                    }
                }
                break;

            case FinishRehabilitationTask.SCENE_LOADING:
                if (currentTaskProgress.progress == Progress.PENDING)
                {
                    StartCoroutine(rehabilitationSceneManager.LoadStartSceneWithIndicator());
                    currentTaskProgress.StartedTask();
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

    private void CancelToFinishRehabilitation()
    {
        atHandUIManager.DeactivateUI();
        avatarState = AvatarState.Walking;
        SetControllerAndRaysVisibility(false);
    }

    private void ExecuteCommunicationToFinishRehabilitation()
    {
        var currentTaskProgress = TaskProgress<FinishRehabilitationTask>.GetCurrentTaskProgress(finishRehabilitationTaskProgressList);
        currentTaskProgress.StartedTask();

        switch (currentTaskProgress.task)
        {
            case FinishRehabilitationTask.POST_RESULT:
                Debug.Log("POST_RESULT");
                string userUuid = SingletonDatabase.Instance.myUserUuid;

                // todo: 一回通信失敗とかになっても大丈夫ようにキャッシュする
                string rehabilitationCondition = SingletonDatabase.Instance.currentRehabilitationCondition;
                string rehabilitationStartedAt = rehabilitationSceneManager.RehabilitationStartedAt.ToString("yyyy/MM/dd HH:mm:ss.ff");
                string rehabilitationFinishedAt = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.ff");
                int reachingTimes = 0; // todo: 適切な値を設定する
                int sharpenedKnifeBefore = 0; // todo: 適切な値を設定する
                int sharpenedKnifeAfter = 0; // todo: 適切な値を設定する
                RehabilitationResultContent result = new(rehabilitationCondition, rehabilitationStartedAt, rehabilitationFinishedAt, reachingTimes, sharpenedKnifeBefore, sharpenedKnifeAfter);

                StartCoroutine(httpCommunicationManager.PostRehabilitationResult(userUuid, result, currentTaskProgress.FinishedTask, currentTaskProgress.FailedTask));
                break;

            case FinishRehabilitationTask.POST_SAVE_DATA:
                Debug.Log("POST_SAVE_DATA");
                userUuid = SingletonDatabase.Instance.myUserUuid;
                int sharpenedKnife = gamificationManager.SharpenedKnife;
                RehabilitationSaveDataContent saveData = new(sharpenedKnife);

                StartCoroutine(httpCommunicationManager.PostRehabilitationSave(userUuid, saveData, currentTaskProgress.FinishedTask, currentTaskProgress.FailedTask));
                break;
        }
    }
}
