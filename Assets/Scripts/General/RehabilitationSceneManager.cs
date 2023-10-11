using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class RehabilitationSceneManager : MonoBehaviour
{
    private const int SCENE_TRANSITION_MAXIMUM_WAIT_DURATION = 7000;
    private const float SCENE_TRANSITION_MINIMUM_LOADING_PROGRESS = 0.89f;

    [SerializeField] private GameObject fadeCanvas;
    [SerializeField] private GameObject loadingCanvas;
    [SerializeField] private GameObject myAvatar;

    [SerializeField] private SyncCommunicationManager syncCommunicationManager;

    [SerializeField] private GameObject gameCanvas;

    private FadeManager fadeManager;
    private LoadingProgressManager loadingProgressManager;
    private MyAvatarManager myAvatarManager;

    public DateTime RehabilitationStartedAt { get { return rehabilitationStartedAt; } }
    private DateTime rehabilitationStartedAt;

    // Start is called before the first frame update
    void Start()
    {
        fadeManager = fadeCanvas.GetComponent<FadeManager>();
        loadingProgressManager = loadingCanvas.GetComponent<LoadingProgressManager>();
        myAvatarManager = myAvatar.GetComponent<MyAvatarManager>();

        string avatarAssetPath = AvatarTypeConverter.ToAssetPath(SingletonDatabase.Instance.avatarType);
        myAvatarManager.InitializeAvatar(avatarAssetPath);

        fadeManager.StartFadeIn();

        rehabilitationStartedAt = DateTime.Now;

        switch (SingletonDatabase.Instance.currentRehabilitationCondition)
        {
            case RehabilitationCondition.SIMPLE:
                gameCanvas.SetActive(false);
                break;
            case RehabilitationCondition.GAMIFICATION:
                break;
            case RehabilitationCondition.COMMUNICATION:
                syncCommunicationManager.StartSyncCommunication();
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator LoadStartSceneWithIndicator()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Start");
        asyncLoad.allowSceneActivation = false;
        DateTime sceneLoadStartTime = DateTime.Now;

        while (!asyncLoad.isDone)
        {
            loadingProgressManager.SetProgress(asyncLoad.progress);

            bool isWaitedEnough = (DateTime.Now - sceneLoadStartTime).TotalMilliseconds >= SCENE_TRANSITION_MAXIMUM_WAIT_DURATION;
            bool isLoadedEnough = asyncLoad.progress >= SCENE_TRANSITION_MINIMUM_LOADING_PROGRESS;

            if (isWaitedEnough && isLoadedEnough)
            {
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
