using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
    private const int sceneTransitionMaximumWaitDuration = 10000;
    private const float sceneTransitionMinimumLoadingProgress = 0.89f;

    [Header("Reference")]
    [SerializeField] private StartUIManager startUIManager;
    [SerializeField] private FadeManager fadeManager;
    [SerializeField] private LoadingProgressManager loadingProgressManager;
    [SerializeField] private HTTPCommunicationManager httpCommunicationManager;

    private List<TaskProgress<StartSceneTask>> taskProgressList;

    private bool isUserCreationSelected = false;

    // Start is called before the first frame update
    void Start()
    {
        taskProgressList = TaskProgress<StartSceneTask>.GenerateTaskProgressList();
    }

    // Update is called once per frame
    void Update()
    {
        var currentTaskProgress = TaskProgress<StartSceneTask>.GetCurrentTaskProgress(taskProgressList);

        switch (currentTaskProgress.task)
        {
            case StartSceneTask.SIGNUP_OR_SIGNIN:
                if (currentTaskProgress.progress == Progress.PENDING)
                {
                    if (startUIManager.IsClickedSimpleRehabilitationStartButton)
                    {
                        StartSignupOrSignin(currentTaskProgress);
                        currentTaskProgress.StartedTask();
                        SingletonDatabase.Instance.currentRehabilitationCondition = "SIMPLE";
                    }
                    if (startUIManager.IsClickedGamificationRehabilitationStartButton)
                    {
                        StartSignupOrSignin(currentTaskProgress);
                        currentTaskProgress.StartedTask();
                        SingletonDatabase.Instance.currentRehabilitationCondition = "GAMIFICATION";
                    }
                    if (startUIManager.IsClickedCommunicationRehabilitationStartButton)
                    {
                        StartSignupOrSignin(currentTaskProgress);
                        currentTaskProgress.StartedTask();
                        SingletonDatabase.Instance.currentRehabilitationCondition = "COMMUNICATION";
                    }
                    if (startUIManager.IsClickedSimpleRehabilitationWithTemporaryAccountStartButton)
                    {
                        StartSignupWithTemporaryAccount(currentTaskProgress);
                        currentTaskProgress.StartedTask();
                        SingletonDatabase.Instance.currentRehabilitationCondition = "SIMPLE";
                    }
                    if (startUIManager.IsClickedGamificationRehabilitationWithTemporaryAccountStartButton)
                    {
                        StartSignupWithTemporaryAccount(currentTaskProgress);
                        currentTaskProgress.StartedTask();
                        SingletonDatabase.Instance.currentRehabilitationCondition = "GAMIFICATION";
                    }
                    if (startUIManager.IsClickedCommunicationRehabilitationWithTemporaryAccountStartButton)
                    {
                        StartSignupWithTemporaryAccount(currentTaskProgress);
                        currentTaskProgress.StartedTask();
                        SingletonDatabase.Instance.currentRehabilitationCondition = "COMMUNICATION";
                    }
                }
                if (currentTaskProgress.progress == Progress.FAILED)
                {
                    startUIManager.SetMessageText(ErrorMessage.SERVER_COMMUNICATION_ERROR_MSG, true);
                    startUIManager.ResetStartButtons();
                    currentTaskProgress.RetryTask();
                }
                break;

            case StartSceneTask.GET_SAVE_DATA:
                if (currentTaskProgress.progress == Progress.PENDING)
                {
                    bool isSaveDataTaskSkipped = isUserCreationSelected || Config.IsTemporaryAccountMode;
                    if (isSaveDataTaskSkipped)
                    {
                        currentTaskProgress.StartedTask();
                        currentTaskProgress.FinishedTask();
                    }
                    else
                    {
                        Debug.Log("StartGetSaveData"); 
                        StartGetSaveData(currentTaskProgress);
                        currentTaskProgress.StartedTask();
                    }
                }
                break;

            case StartSceneTask.FADING_OUT:
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

            case StartSceneTask.SCENE_LOADING:
                if (currentTaskProgress.progress == Progress.PENDING)
                {
                    StartCoroutine(LoadSceneWithIndicator());
                    currentTaskProgress.StartedTask();
                }
                break;
        }
    }

    private void StartSignupOrSignin(TaskProgress<StartSceneTask> taskProgress)
    {
        string userName = startUIManager.UserNameInputFieldText;
        string password = startUIManager.PasswordInputFieldText;

        isUserCreationSelected = startUIManager.IsUserCreation;

        if (isUserCreationSelected)
        {
            StartCoroutine(httpCommunicationManager.PostUserSignup(userName, password, SaveUserUuidAndToken, taskProgress.FinishedTask, taskProgress.FailedTask));
        }
        else
        {
            StartCoroutine(httpCommunicationManager.PostUserSignin(userName, password, SaveUserUuidAndToken, taskProgress.FinishedTask, taskProgress.FailedTask));
        }
    }

    private void StartSignupWithTemporaryAccount(TaskProgress<StartSceneTask> taskProgress)
    {
        string userName = "";
        StartCoroutine(httpCommunicationManager.PostUserSignupWithTemporaryAccount(userName, SaveUserUuidAndToken, taskProgress.FinishedTask, taskProgress.FailedTask));
    }

    private void StartGetSaveData(TaskProgress<StartSceneTask> taskProgress)
    {
        string userUuid = SingletonDatabase.Instance.myUserUuid;

        StartCoroutine(httpCommunicationManager.GetRehabilitationSave(userUuid, SaveLoadedSaveData, taskProgress.FinishedTask, taskProgress.FailedTask));
    }

    private void SaveUserUuidAndToken(string userUuid, string token)
    {
        SingletonDatabase singletonDatabase = SingletonDatabase.Instance;
        singletonDatabase.myUserUuid = userUuid;
        singletonDatabase.myUserName = startUIManager.UserNameInputFieldText;
        singletonDatabase.myToken = token;
    }

    private void SaveLoadedSaveData(RehabilitationSaveDataContent loadedSaveData)
    {
        SingletonDatabase singletonDatabase = SingletonDatabase.Instance;
        singletonDatabase.loadedSaveData = loadedSaveData;
    }

    private IEnumerator LoadSceneWithIndicator()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Rehabilitation");
        asyncLoad.allowSceneActivation = false;
        DateTime sceneLoadStartTime = DateTime.Now;

        while (!asyncLoad.isDone)
        {
            loadingProgressManager.SetProgress(asyncLoad.progress);

            bool isWaitedEnough = (DateTime.Now - sceneLoadStartTime).TotalMilliseconds >= sceneTransitionMaximumWaitDuration;
            bool isLoadedEnough = asyncLoad.progress >= sceneTransitionMinimumLoadingProgress;

            if (isWaitedEnough && isLoadedEnough)
            {
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
