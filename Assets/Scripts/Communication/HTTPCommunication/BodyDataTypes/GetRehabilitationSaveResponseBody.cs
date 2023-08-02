using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GetRehabilitationSaveResponseBody
{
    public string userUuid;
    public RehabilitationSaveDataContent saveData;

    public GetRehabilitationSaveResponseBody(string userUuid, RehabilitationSaveDataContent saveData)
    {
        this.userUuid = userUuid;
        this.saveData = saveData;
    }
}
