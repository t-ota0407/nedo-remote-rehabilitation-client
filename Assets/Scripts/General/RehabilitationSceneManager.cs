using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class RehabilitationSceneManager : MonoBehaviour
{

    [SerializeField] private GameObject fadeCanvas;
    [SerializeField] private GameObject loadingCanvas;
    [SerializeField] private GameObject myAvatar;

    private FadeManager fadeManager;
    private LoadingProgressManager loadingProgressManager;
    private MyAvatarManager myAvatarManager;

    // Start is called before the first frame update
    void Start()
    {
        fadeManager = fadeCanvas.GetComponent<FadeManager>();
        loadingProgressManager = loadingCanvas.GetComponent<LoadingProgressManager>();
        myAvatarManager = myAvatar.GetComponent<MyAvatarManager>();
        myAvatarManager.InitializeAvatar("Prefabs/Avatars/Female_Adult_01 Variant");

        fadeManager.StartFadeIn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
