using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FacilityCardManager : MonoBehaviour
{
    private const string UNKNOWN_NAME = "�H�H�H�H�H";

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
            // �S�����V���[�v�i�[�̂݉��s���܂ނ��A�C���X�y�N�^�[������s���w�肷��ƓK�؂ɕ\������Ȃ����߁A�ʂɑΉ��B
            facilityNameText.text = (facilityName.Equals("�S�����V���[�v�i�[")) ? "�S����\n�V���[�v�i�[" : facilityName;
            facilityAmountText.text = $"{amount}";
        }
    }
}
