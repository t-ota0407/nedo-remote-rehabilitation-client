using System;

[Serializable]
public class PostRehabilitationResultRequestBody
{
    public string userUuid;
    public RehabilitationResultContent result;


    public PostRehabilitationResultRequestBody (string userUuid, RehabilitationResultContent result)
    {
        this.userUuid = userUuid;
        this.result = result;
    }
}
