using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject xrOrigin;

    [SerializeField] private GameObject spawnPosition0;
    [SerializeField] private GameObject spawnPosition1;
    [SerializeField] private GameObject spawnPosition2;
    [SerializeField] private GameObject spawnPosition3;
    [SerializeField] private GameObject spawnPosition4;
    [SerializeField] private GameObject spawnPosition5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnTo(SpawnPositionType spawnPositionType)
    {
        switch (spawnPositionType)
        {
            case SpawnPositionType.POSITION_0:
                xrOrigin.transform.position = spawnPosition0.transform.position;
                xrOrigin.transform.rotation = spawnPosition0.transform.rotation;
                break;
            case SpawnPositionType.POSITION_1:
                xrOrigin.transform.position = spawnPosition1.transform.position;
                xrOrigin.transform.rotation = spawnPosition1.transform.rotation;
                break;
            case SpawnPositionType.POSITION_2:
                xrOrigin.transform.position = spawnPosition2.transform.position;
                xrOrigin.transform.rotation = spawnPosition2.transform.rotation;
                break;
            case SpawnPositionType.POSITION_3:
                xrOrigin.transform.position = spawnPosition3.transform.position;
                xrOrigin.transform.rotation = spawnPosition3.transform.rotation;
                break;
            case SpawnPositionType.POSITION_4:
                xrOrigin.transform.position = spawnPosition4.transform.position;
                xrOrigin.transform.rotation = spawnPosition4.transform.rotation;
                break;
            case SpawnPositionType.POSITION_5:
                xrOrigin.transform.position = spawnPosition5.transform.position;
                xrOrigin.transform.rotation = spawnPosition5.transform.rotation;
                break;
        }
    }
}
