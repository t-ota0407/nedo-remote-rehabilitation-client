using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeyboardManager : MonoBehaviour
{
    [SerializeField] private List<Button> characterKeys;
    [SerializeField] private Button backspaceKey;

    public bool IsInputed { get { return isInputed; } }
    private bool isInputed = false;

    private KeyboardInput keyboardInput = new(KeyboardInputType.CHARACTER);

    // Start is called before the first frame update
    void Start()
    {
        foreach (Button characterKey in characterKeys)
        {
            characterKey.onClick.AddListener(() =>
            {
                if (!isInputed)
                {
                    GameObject textMeshProObject = characterKey.transform.Find("Text (TMP)").gameObject;
                    TextMeshProUGUI textMeshPro = textMeshProObject.GetComponent<TextMeshProUGUI>();
                    keyboardInput = new KeyboardInput(KeyboardInputType.CHARACTER, textMeshPro.text);
                    isInputed = true;
                    Debug.Log("click");
                }
            });
        }

        backspaceKey.onClick.AddListener(() =>
        {
            if (!isInputed)
            {
                keyboardInput = new KeyboardInput(KeyboardInputType.BACKSPACE);
                isInputed = true;
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public KeyboardInput ReadKeyboardInput()
    {
        isInputed = false;
        return keyboardInput;
    }
}
