using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllKnifeSharpeningSetupsManager : MonoBehaviour
{
    [SerializeField] private List<KnifeSharpeningSetupManager> gamificationSetups;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetGamificationSetupsVisibility(bool visibility)
    {
        foreach (KnifeSharpeningSetupManager knifeSharpeningSetupManager in gamificationSetups)
        {
            knifeSharpeningSetupManager.SetVisibility(visibility);
        }
    }
}
