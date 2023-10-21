using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    private const int LogLineMaxLength = 20;
    private const float CanvasRotationSpeed = 1.5f;

    [SerializeField] private TextMeshProUGUI sharpenedKnifeText;
    [SerializeField] private TextMeshProUGUI autoIncrementSharpenedknifePerSecondText;
    [SerializeField] private TextMeshProUGUI firstLineLogText;
    [SerializeField] private TextMeshProUGUI secondLineLogText;
    [SerializeField] private TextMeshProUGUI thirdLineLogText;
    [SerializeField] private TextMeshProUGUI fourthLineLogText;
    [SerializeField] private TextMeshProUGUI fifthLineLogText;
    [SerializeField] private TextMeshProUGUI sixthLineLogText;

    [SerializeField] private FacilityCardManager artisanCardManager;
    [SerializeField] private FacilityCardManager autoSharpenerCardManager;
    [SerializeField] private FacilityCardManager mineCardManager;
    [SerializeField] private FacilityCardManager factoryCardManager;
    [SerializeField] private FacilityCardManager bankCardManager;
    [SerializeField] private FacilityCardManager alchemistCardManager;
    [SerializeField] private FacilityCardManager wizardCardManager;

    private bool isRotating;
    private Quaternion rotationTargetPosture;

    // Start is called before the first frame update
    void Start()
    {
        firstLineLogText.text = "";
        secondLineLogText.text = "";
        thirdLineLogText.text = "";
        fourthLineLogText.text = "";
        fifthLineLogText.text = "";
        sixthLineLogText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (isRotating)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, rotationTargetPosture, CanvasRotationSpeed * Time.deltaTime);
        }
    }

    public void StartRotation(Quaternion targetPosture)
    {
        isRotating = true;
        rotationTargetPosture = targetPosture;
    }

    public void UpdateSharpenedKnife(int sharpenedKnife)
    {
        sharpenedKnifeText.text = sharpenedKnife.ToString();
    }

    public void UpdateAutoIncrementSharpenedKnifePerSecond(float autoIncrementSharpenedKnife)
    {
        autoIncrementSharpenedknifePerSecondText.text = autoIncrementSharpenedKnife.ToString("F1");
    }


    public void UpdateFacilityCard(FacilityType facilityType, int amount)
    {
        switch (facilityType)
        {
            case FacilityType.Artisan:
                artisanCardManager.UpdateCard(amount);
                break;
            case FacilityType.AutoSharpener:
                autoSharpenerCardManager.UpdateCard(amount);
                break;
            case FacilityType.Mine:
                mineCardManager.UpdateCard(amount);
                break;
            case FacilityType.Factory:
                factoryCardManager.UpdateCard(amount);
                break;
            case FacilityType.Bank:
                bankCardManager.UpdateCard(amount);
                break;
            case FacilityType.Alchemist:
                alchemistCardManager.UpdateCard(amount);
                break;
            case FacilityType.Wizard:
                wizardCardManager.UpdateCard(amount);
                break;
        }
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
            fifthLineLogText.text = sixthLineLogText.text;
            sixthLineLogText.text = message;
        }
        else if (logTextLength <= LogLineMaxLength * 2)
        {
            string newLine1 = message.Substring(0, LogLineMaxLength);
            string newLine2 = message.Substring(LogLineMaxLength, logTextLength - LogLineMaxLength);

            firstLineLogText.text = thirdLineLogText.text;
            secondLineLogText.text = fourthLineLogText.text;
            thirdLineLogText.text = fifthLineLogText.text;
            fourthLineLogText.text = sixthLineLogText.text;
            fifthLineLogText.text = newLine1;
            sixthLineLogText.text = newLine2;
        }
    }
}
