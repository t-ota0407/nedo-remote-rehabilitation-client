using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PostUserSignupRequestBody
{
    public string userName;
    public string password;

    public PostUserSignupRequestBody(string userName, string password)
    {
        this.userName = userName;
        this.password = password;
    }
}
