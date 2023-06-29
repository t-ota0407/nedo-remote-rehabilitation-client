using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RehabilitationSceneManager : MonoBehaviour
{
    [SerializeField] private GameObject fadeCanvas;
    [SerializeField] private GameObject loadingCanvas;

    private FadeManager fadeManager;
    private LoadingProgressManager loadingProgressManager;

    // Start is called before the first frame update
    void Start()
    {
        fadeManager = fadeCanvas.GetComponent<FadeManager>();
        loadingProgressManager = loadingCanvas.GetComponent<LoadingProgressManager>();

        fadeManager.StartFadeIn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
