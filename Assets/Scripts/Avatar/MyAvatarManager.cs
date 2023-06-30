using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyAvatarManager : MonoBehaviour, AvatarManager
{
    [SerializeField] private GameObject hmd;
    [SerializeField] private GameObject leftController;
    [SerializeField] private GameObject rightController;

    [SerializeField] private GameObject vrikHeadTarget;
    [SerializeField] private GameObject vrikLeftHandTarget;
    [SerializeField] private GameObject vrikRightHandTarget;

    private string uuid;

    void Awake()
    {
        uuid = Guid.NewGuid().ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateVrikTargetPosture();
    }

    public string UUID()
    {
        return uuid;
    }

    public Vector3 HeadPosition()
    {
        return hmd.transform.position;
    }

    public Vector3 HeadRotation()
    {
        return hmd.transform.rotation.eulerAngles;
    }

    private void UpdateVrikTargetPosture()
    {
        vrikHeadTarget.transform.position = hmd.transform.position;
        vrikHeadTarget.transform.rotation = Quaternion.Euler(hmd.transform.rotation.eulerAngles + new Vector3(0, -90, -90));

        vrikLeftHandTarget.transform.position = leftController.transform.position;
        vrikLeftHandTarget.transform.rotation = leftController.transform.rotation;

        vrikRightHandTarget.transform.position = rightController.transform.position;
        vrikRightHandTarget.transform.rotation = rightController.transform.rotation;
    }
}
