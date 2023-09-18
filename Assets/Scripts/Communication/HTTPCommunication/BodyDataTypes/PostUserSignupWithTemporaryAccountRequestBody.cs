using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PostUserSignupWithTemporaryAccountRequestBody
{
    public string userName;

    public PostUserSignupWithTemporaryAccountRequestBody(string userName)
    {
        this.userName = userName;
    }
}
