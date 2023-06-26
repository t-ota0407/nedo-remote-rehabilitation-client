using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface AvatarManager
{
    public string UUID();
    public Vector3 HeadPosition();
    public Vector3 HeadRotation();
}
