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

    // Start is called before the first frame update
    void Start()
    {
        taskProgressList = TaskProgress<StartSceneTask>.GenerateTaskProgressList();
    }

    // Update is called once per frame
    void Update()
    {
        TaskProgress<StartSceneTask> currentTaskProgress = TaskProgress<StartSceneTask>.GetCurrentTaskProgress(taskProgressList);

        switch (currentTaskProgress.task)
        {
            case StartSceneTask.HTTP_COMMUNICATION:
                if (currentTaskProgress.progress == Progress.PENDING)
                {
                    if ((startUIManager.IsClickedSimpleRehabilitationStartButton))
                    {
                        StartSignupOrSignin(currentTaskProgress);
                        currentTaskProgress.StartedTask();
                    }
                }
                if (currentTaskProgress.progress == Progress.FAILED)
                {
                    startUIManager.SetMessageText(ErrorMessage.SERVER_COMMUNICATION_ERROR_MSG, true);
                    startUIManager.ResetStartButtons();
                    currentTaskProgress.RetryTask();
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


        if (startUIManager.IsClickedGamificationRehabilitationStartButton)
        {

        }

        if (startUIManager.IsClickedCommunicationRehabilitationStartButton)
        {

        }
    }

    private void StartSignupOrSignin(TaskProgress<StartSceneTask> taskProgress)
    {
        string userName = startUIManager.UserNameInputFieldText;
        string password = startUIManager.PasswordInputFieldText;

        if (startUIManager.IsUserCreation)
        {
            StartCoroutine(httpCommunicationManager.PostUserSignup(userName, password, SaveUserUuidAndToken, taskProgress.FinishedTask, taskProgress.FailedTask));
        }
        else
        {
            StartCoroutine(httpCommunicationManager.PostUserSignin(userName, password, SaveUserUuidAndToken, taskProgress.FinishedTask, taskProgress.FailedTask));
        }
    }

    private void SaveUserUuidAndToken(string userUuid, string token)
    {
        SingletonDatabase singletonDatabase = SingletonDatabase.Instance;
        singletonDatabase.myUserUuid = userUuid;
        singletonDatabase.myUserName = startUIManager.UserNameInputFieldText;
        singletonDatabase.myToken = token;
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
