using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PostUserSigninRequestBody
{
    public string userName;
    public string password;
    public string currentAvatarType;

    public PostUserSigninRequestBody(string userName, string password, string currentAvatarType)
    {
        this.userName = userName;
        this.password = password;
        this.currentAvatarType = currentAvatarType;
    }
}
