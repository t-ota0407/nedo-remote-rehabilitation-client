using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class StartUIManager : MonoBehaviour
{
    [SerializeField] private Button simpleRehabilitationStartButton;
    [SerializeField] private Button gamificationRehabilitationStartButton;
    [SerializeField] private Button communicationRehabilitationStartButton;

    [SerializeField] private TMP_InputField userNameInputField;
    [SerializeField] private TMP_InputField passwordInputField;
    [SerializeField] private Toggle userCreationToggle;

    [SerializeField] private Text messageText;

    [SerializeField] private KeyboardManager keyboardManager;

    public bool IsClickedSimpleRehabilitationStartButton { get { return isClickedSimpleRehabilitationStartButton; } }
    private bool isClickedSimpleRehabilitationStartButton;

    public bool IsClickedGamificationRehabilitationStartButton { get { return isClickedGamificationRehabilitationStartButton; } }
    private bool isClickedGamificationRehabilitationStartButton;

    public bool IsClickedCommunicationRehabilitationStartButton { get { return isClickedCommunicationRehabilitationStartButton; } }
    private bool isClickedCommunicationRehabilitationStartButton;

    public bool IsUserCreation { get { return userCreationToggle.isOn; } }

    public string UserNameInputFieldText { get { return userNameInputField.text; } }

    public string PasswordInputFieldText { get { return passwordInputField.text; } }

    private TMP_InputField targetInputField;

    // Start is called before the first frame update
    void Start()
    {
        simpleRehabilitationStartButton.onClick.AddListener(() =>
        {
            DisableStartButtons();
            isClickedSimpleRehabilitationStartButton = true;
        });
        gamificationRehabilitationStartButton.onClick.AddListener(() =>
        {
            DisableStartButtons();
            isClickedGamificationRehabilitationStartButton = true;
        });
        communicationRehabilitationStartButton.onClick.AddListener(() =>
        {
            DisableStartButtons();
            isClickedCommunicationRehabilitationStartButton = true;
        });

        userNameInputField.onSelect.AddListener(text => targetInputField = userNameInputField);
        passwordInputField.onSelect.AddListener(text => targetInputField = passwordInputField);
    }

    // Update is called once per frame
    void Update()
    {
        if (keyboardManager.IsInputed)
        {
            KeyboardInput keyboardInput = keyboardManager.ReadKeyboardInput();
            ChangeTargetInputField(keyboardInput);
        }
    }

    public void SetMessageText(string message, bool isWarning)
    {
        messageText.text = message;

        if (isWarning)
        {
            messageText.color = new Color(1, 0.1f, 0.24f);
        }
        else
        {
            messageText.color = new Color(1, 1, 1);
        }
    }

    public void ResetStartButtons()
    {
        isClickedSimpleRehabilitationStartButton = false;
        isClickedGamificationRehabilitationStartButton = false;
        isClickedCommunicationRehabilitationStartButton = false;
        simpleRehabilitationStartButton.interactable = true;
        gamificationRehabilitationStartButton.interactable = true;
        communicationRehabilitationStartButton.interactable = true;
    }

    private void DisableStartButtons()
    {
        simpleRehabilitationStartButton.interactable = false;
        gamificationRehabilitationStartButton.interactable = false;
        communicationRehabilitationStartButton.interactable = false;
    }

    private void ChangeTargetInputField(KeyboardInput keyboardInput)
    {
        switch (keyboardInput.keyboardInputType)
        {
            case KeyboardInputType.CHARACTER:
                {
                    string currentText = targetInputField.text;
                    targetInputField.text = currentText + keyboardInput.keyboardInputCharacter;
                    break;
                }
            case KeyboardInputType.BACKSPACE:
                {
                    string currentText = targetInputField.text;
                    if (currentText.Length > 0)
                    {
                        targetInputField.text = currentText.Substring(0, currentText.Length - 1);
                    }
                    break;
                }
        }
    }
}
