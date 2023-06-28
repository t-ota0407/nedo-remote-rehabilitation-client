using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingProgressManager : MonoBehaviour
{
    [SerializeField] private Image progressIndicatorImage;
    [SerializeField] private Text percentageText;
    [SerializeField] private Text statusText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetProgress(float loadingProgress)
    {
        if (loadingProgress < 0)
            loadingProgress = 0;
        if (loadingProgress > 1)
            loadingProgress = 1;

        progressIndicatorImage.fillAmount = loadingProgress;
        percentageText.text = (loadingProgress * 100).ToString("F0");
    }
}
