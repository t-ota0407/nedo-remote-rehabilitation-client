using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostUserSigninRequestBody
{
    public string userName;
    public string password;
    public string deviceSecret;

    public PostUserSigninRequestBody(string userName, string password, string deviceSecret)
    {
        this.userName = userName;
        this.password = password;
        this.deviceSecret = deviceSecret;
    }
}
