using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
    private const int sceneTransitionMaximumWaitDuration = 10000;
    private const float sceneTransitionMinimumLoadingProgress = 0.89f;

    [Header("Reference")]
    [SerializeField] private GameObject startUI;
    [SerializeField] private GameObject fadeCanvas;
    [SerializeField] private GameObject loadingCanvas;

    private StartUIManager startUIManager;
    private FadeManager fadeManager;
    private LoadingProgressManager loadingProgressManager;

    private bool isLoadingNextScene = false;

    // Start is called before the first frame update
    void Start()
    {
        startUIManager = startUI.GetComponent<StartUIManager>();
        fadeManager = fadeCanvas.GetComponent<FadeManager>();
        loadingProgressManager = loadingCanvas.GetComponent<LoadingProgressManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (startUIManager.IsPushedSoloPlayButton)
        {
            if (fadeManager.FadeStatus == FadeStatus.Idle)
            {
                fadeManager.StartFadeOut();
                loadingProgressManager.Display();
            }

            if (fadeManager.FadeStatus == FadeStatus.Finished && !isLoadingNextScene)
            {
                StartCoroutine(LoadSceneWithIndicator());
                isLoadingNextScene = true;
            }
        }

        if (startUIManager.IsPushedMultiPlayButton)
        {

        }
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
