using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PostRehabilitationResultResponseBody
{
    public string uuid;

    public PostRehabilitationResultResponseBody(string uuid)
    {
        this.uuid = uuid;
    }
}
