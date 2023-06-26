using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RehabilitationUIManager : MonoBehaviour
{
    private const int LogLineMaxLength = 21;

    [SerializeField] private Text sharpenedKnifeText;
    [SerializeField] private Text firstLineLogText;
    [SerializeField] private Text secondLineLogText;
    [SerializeField] private Text thirdLineLogText;
    [SerializeField] private Text fourthLineLogText;
    [SerializeField] private Text fifthLineLogText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateProgress(int sharpenedKnife)
    {
        sharpenedKnifeText.text = sharpenedKnife.ToString();
    }

    public void UpdateLogText(string message)
    {
        int logTextLength = message.Length;

        // 2行までに収まらないメッセージは受け付けていない
        if (logTextLength <= LogLineMaxLength)
        {
            firstLineLogText.text = secondLineLogText.text;
            secondLineLogText.text = thirdLineLogText.text;
            thirdLineLogText.text = fourthLineLogText.text;
            fourthLineLogText.text = fifthLineLogText.text;
            fifthLineLogText.text = message;
        }
        else if (logTextLength <= LogLineMaxLength * 2)
        {
            string newLine1 = message.Substring(0, LogLineMaxLength);
            string newLine2 = message.Substring(LogLineMaxLength, logTextLength - LogLineMaxLength);

            firstLineLogText.text = thirdLineLogText.text;
            secondLineLogText.text = fourthLineLogText.text;
            thirdLineLogText.text = fifthLineLogText.text;
            fourthLineLogText.text = newLine1;
            fifthLineLogText.text = newLine2;
        }
    }
}
