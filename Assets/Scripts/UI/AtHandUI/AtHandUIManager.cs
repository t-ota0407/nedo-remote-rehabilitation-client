using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AtHandUIManager : MonoBehaviour
{
    [SerializeField] private GameObject offset;
    [SerializeField] private GameObject mainCamera;

    [SerializeField] private Text headerText;
    [SerializeField] private Text contentText;
    [SerializeField] private Button negativeButton;
    [SerializeField] private Text negativeButtonText;
    [SerializeField] private Button positiveButton;
    [SerializeField] private Text positiveButtonText;

    public bool IsActive { get { return isActive; } }
    private bool isActive = false;

    private Canvas canvas;

    // Start is called before the first frame update
    void Start()
    {
        transform.localPosition = new Vector3(0, 0.9f, 0.9f);
        transform.localRotation = Quaternion.Euler(new Vector3(30, 0, 0));

        canvas = GetComponent<Canvas>();
        canvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        float cameraRotationY = mainCamera.transform.rotation.eulerAngles.y;
        offset.transform.rotation = Quaternion.Euler(0, cameraRotationY, 0);
    }

    public void ActivateUI(
        string headerText,
        string contentText,
        string negativeButtonText,
        string positiveButtonText,
        UnityAction negativeButtonAction,
        UnityAction positiveButtonAction
        )
    {
        this.headerText.text = headerText;
        this.contentText.text = contentText;
        this.negativeButtonText.text = negativeButtonText;
        this.positiveButtonText.text = positiveButtonText;
        
        negativeButton.onClick.AddListener(() =>
        {
            negativeButtonAction();
        });
        positiveButton.onClick.AddListener(() =>
        {
            positiveButtonAction();
        });

        canvas.enabled = true;
    }

    public void DeactivateUI()
    {
        canvas.enabled = false;
    }
}
