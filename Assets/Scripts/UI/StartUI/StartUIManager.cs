using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartUIManager : MonoBehaviour
{
    [SerializeField] private Button soloPlayButton;

    [SerializeField] private Button multiPlayButton;

    private bool isPushedSoloPlayButton;
    public bool IsPushedSoloPlayButton
    {
        get { return isPushedSoloPlayButton; }
    }

    private bool isPushedMultiPlayButton;
    public bool IsPushedMultiPlayButton
    {
        get { return isPushedMultiPlayButton; }
    }

    // Start is called before the first frame update
    void Start()
    {
        soloPlayButton.onClick.AddListener(() => isPushedSoloPlayButton = true);

        // multiPlayButton.onClick.AddListener(() => isPushedMultiPlayButton = true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
