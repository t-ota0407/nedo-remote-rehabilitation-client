using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PostUserSignupWithTemporaryAccountRequestBody
{
    public string userName;
    public string currentAvatarType;

    public PostUserSignupWithTemporaryAccountRequestBody(string userName, string currentAvatarType)
    {
        this.userName = userName;
        this.currentAvatarType = currentAvatarType;
    }
}
