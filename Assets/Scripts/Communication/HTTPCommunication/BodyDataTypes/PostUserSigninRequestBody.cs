using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PostUserSigninRequestBody
{
    public string userName;
    public string password;

    public PostUserSigninRequestBody(string userName, string password)
    {
        this.userName = userName;
        this.password = password;
    }
}
