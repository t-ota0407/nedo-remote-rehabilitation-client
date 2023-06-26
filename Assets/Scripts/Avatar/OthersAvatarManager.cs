using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OthersAvatarManager : AvatarManager
{
    private string uuid;
    private Vector3 headPosition;
    private Vector3 headRotation;

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
        return headPosition;
    }

    public Vector3 HeadRotation()
    {
        return headRotation;
    }
}
