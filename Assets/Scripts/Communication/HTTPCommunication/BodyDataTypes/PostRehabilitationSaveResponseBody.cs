using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PostRehabilitationSaveResponseBody
{
    public string uuid;

    public PostRehabilitationSaveResponseBody(string uuid)
    {
        this.uuid = uuid;
    }
}
