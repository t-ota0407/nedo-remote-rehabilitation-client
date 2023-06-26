using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyAvatarManager : MonoBehaviour, AvatarManager
{
    [SerializeField] private GameObject head;
    [SerializeField] private GameObject leftHand;
    [SerializeField] private GameObject rightHand;

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

    }

    public string UUID()
    {
        return uuid;
    }

    public Vector3 HeadPosition()
    {
        return head.transform.position;
    }

    public Vector3 HeadRotation()
    {
        return head.transform.rotation.eulerAngles;
    }
}
