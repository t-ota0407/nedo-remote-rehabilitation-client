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
        simpleRehabilitationStartButton.onClick.AddListener(() => isClickedSimpleRehabilitationStartButton = true);
        gamificationRehabilitationStartButton.onClick.AddListener(() => isClickedGamificationRehabilitationStartButton = true);
        communicationRehabilitationStartButton.onClick.AddListener(() => isClickedCommunicationRehabilitationStartButton = true);

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
