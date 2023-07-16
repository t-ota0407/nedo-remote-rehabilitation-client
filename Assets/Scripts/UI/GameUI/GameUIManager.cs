using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    private const int LogLineMaxLength = 21;
    private const float CanvasRotationSpeed = 1.5f;

    [SerializeField] private TextMeshProUGUI sharpenedKnifeNumberText;
    [SerializeField] private TextMeshProUGUI knifePerSecondNumberText;
    [SerializeField] private TextMeshProUGUI firstLineLogText;
    [SerializeField] private TextMeshProUGUI secondLineLogText;
    [SerializeField] private TextMeshProUGUI thirdLineLogText;
    [SerializeField] private TextMeshProUGUI fourthLineLogText;
    [SerializeField] private TextMeshProUGUI fifthLineLogText;

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

    public void UpdateSharpenedKnifeNumber(int sharpenedKnife)
    {
        sharpenedKnifeNumberText.text = sharpenedKnife.ToString();
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
