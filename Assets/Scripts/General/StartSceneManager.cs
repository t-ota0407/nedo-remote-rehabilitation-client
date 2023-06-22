using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private GameObject startUI;
    [SerializeField] private GameObject fadeCanvas;

    private StartUIManager startUIManager;
    private FadeManager fadeManager;

    private bool isLoadingNextScene = false;

    // Start is called before the first frame update
    void Start()
    {
        startUIManager = startUI.GetComponent<StartUIManager>();
        fadeManager = fadeCanvas.GetComponent<FadeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (startUIManager.IsPushedSoloPlayButton)
        {
            if (fadeManager.FadeStatus == FadeStatus.Idle)
            {
                fadeManager.StartFadeOut();
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

        while (!asyncLoad.isDone)
        {
            // indicator‚ð‰ñ‚·
            if (asyncLoad.progress > 0.95f)
            {
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }

    }
}
