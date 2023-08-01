using System;

[Serializable]
public class PostRehabilitationSaveRequestBody
{
    public string userUuid;
    public RehabilitationSaveDataContent saveData;

    public PostRehabilitationSaveRequestBody(string userUuid, RehabilitationSaveDataContent saveData)
    {
        this.userUuid = userUuid;
        this.saveData = saveData;
    }
}
