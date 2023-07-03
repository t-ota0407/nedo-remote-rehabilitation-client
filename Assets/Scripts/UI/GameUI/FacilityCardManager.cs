using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FacilityCardManager : MonoBehaviour
{
    [SerializeField] private Image facilityImage;
    [SerializeField] private Image unknownImage;
    [SerializeField] private TextMeshProUGUI facilityNameText;
    [SerializeField] private TextMeshProUGUI facilityAmountText;

    // Start is called before the first frame update
    void Start()
    {
        facilityImage.enabled = false;
        unknownImage.enabled = true;
        facilityNameText.text = "？？？？？";
        facilityAmountText.text = "0";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
