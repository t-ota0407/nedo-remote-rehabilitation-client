using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PostUserSignupRequestBody
{
    public string userName;
    public string password;
    public string currentAvatarType;

    public PostUserSignupRequestBody(string userName, string password, string currentAvatarType)
    {
        this.userName = userName;
        this.password = password;
        this.currentAvatarType = currentAvatarType;
    }
}
