using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FacilityCardManager : MonoBehaviour
{
    private const string UNKNOWN_NAME = "？？？？？";

    [SerializeField] private Image facilityImage;
    [SerializeField] private Image unknownImage;
    [SerializeField] private TextMeshProUGUI facilityNameText;
    [SerializeField] private TextMeshProUGUI facilityAmountText;
    [SerializeField] private string facilityName;

    // Start is called before the first frame update
    void Start()
    {
        facilityImage.enabled = false;
        unknownImage.enabled = true;
        facilityNameText.text = UNKNOWN_NAME;
        facilityAmountText.text = "0";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateCard(int amount)
    {
        if (amount <= 0)
        {
            unknownImage.enabled = true;
            facilityImage.enabled = false;
            facilityNameText.text = UNKNOWN_NAME;
            facilityAmountText.text = "0";
        }
        else
        {
            Debug.Log(facilityName);
            unknownImage.enabled = false;
            facilityImage.enabled = true;
            // 全自動シャープナーのみ改行を含むが、インスペクターから改行を指定すると適切に表示されないため、個別に対応。
            facilityNameText.text = (facilityName.Equals("全自動シャープナー")) ? "全自動\nシャープナー" : facilityName;
            facilityAmountText.text = $"{amount}";
        }
    }
}
